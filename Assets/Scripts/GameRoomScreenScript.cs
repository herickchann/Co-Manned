using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameRoomScreenScript : NetworkBehaviour {

	// synchronized fields
	// unfortunately, syncvars only works with straight primitives so we shall write out the fields
	// until the need for further abstraction arises
	// player id slots (for underlying selection)
	[SyncVar]
	public string RPpid = "";
	[SyncVar]
	public string REpid = "";
	[SyncVar]
	public string BPpid = "";
	[SyncVar]
	public string BEpid = "";
	// user name slots (for rendering selection data)
	[SyncVar]
	public string RPuname = "";
	[SyncVar]
	public string REuname = "";
	[SyncVar]
	public string BPuname = "";
	[SyncVar]
	public string BEuname = "";

	// drag and dropped from the editor
	public Text RoomName;
	public Button RPButton; // red pilot
	public Button REButton; // red engineer
	public Button BPButton; // blue pilot
	public Button BEButton; // blue engineer

	// identifiers that should not change once in the game room
	public string myPid = "";
	public string myUserName = "";

	// Use this for initialization
	void Start () {
		RoomName.text = "Room: " + GameManager.instance.gameName;
		this.myPid = playerControllerId.ToString(); //netId.ToString();
		this.myUserName = GameManager.instance.userName;
		// for dev purposes, if not username exists, use "me"
		if (this.myUserName == "") {this.myUserName = "me";}
	}
	
	// Update is called once per frame
	void Update () {
		// until more abstraction needed, we will hardcode UI updates
		string label = "";
		// red pilot
		if (this.RPpid != "" && this.RPuname != "") {
			label = "Red Pilot\n" + this.RPuname;
			buttonBooked(RPButton, label);
		} else {
			label = "Red Pilot\n---";
			buttonAvailable(RPButton, label);
		}
		// red engineer
		if (this.REpid != "" && this.REuname != "") {
			label = "Red Engineer\n" + this.REuname;
			buttonBooked(REButton, label);
		} else {
			label = "Red Engineer\n---";
			buttonAvailable(REButton, label);
		}
		// blue pilot
		if (this.BPpid != "" && this.BPuname != "") {
			label = "Blue Pilot\n" + this.BPuname;
			buttonBooked(BPButton, label);
		} else {
			label = "Blue Pilot\n---";
			buttonAvailable(BPButton, label);
		}
		// blue engineer
		if (this.BEpid != "" && this.BEuname != "") {
			label = "Blue Engineer\n" + this.BPuname;
			buttonBooked(BEButton, label);
		} else {
			label = "Blue Engineer\n---";
			buttonAvailable(BEButton, label);
		}
	}

	private void buttonAvailable(Button b, string label) {
		b.enabled = true;
		b.image.color = Color.white;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
	}

	private void buttonBooked(Button b, string label) {
		b.enabled = false;
		b.image.color = Color.gray;
		Text btex = b.GetComponentInChildren<Text>();
		btex.text = label;
	}

	// verbose boilerplate to support syncvars
	// cmd = client -> server
	// rpc = server -> clients
	// player id sync functions
	[Command]
	void CmdSetRPpid(string pid) { RpcSetRPpid(pid); }
	[ClientRpc]
	void RpcSetRPpid(string pid) { RPpid = pid; }
	[Command]
	void CmdSetREpid(string pid) { RpcSetREpid(pid); }
	[ClientRpc]
	void RpcSetREpid(string pid) { REpid = pid; }
	[Command]
	void CmdSetBPpid(string pid) { RpcSetBPpid(pid); }
	[ClientRpc]
	void RpcSetBPpid(string pid) { BPpid = pid; }
	[Command]
	void CmdSetBEpid(string pid) { RpcSetBEpid(pid); }
	[ClientRpc]
	void RpcSetBEpid(string pid) { BEpid = pid; }
	// username sync functions
	[Command]
	void CmdSetRPuname(string uname) { RpcSetRPuname(uname); }
	[ClientRpc]
	void RpcSetRPuname(string uname) { RPuname = uname; }
	[Command]
	void CmdSetREuname(string uname) { RpcSetREuname(uname); }
	[ClientRpc]
	void RpcSetREuname(string uname) { REuname = uname; }
	[Command]
	void CmdSetBPuname(string uname) { RpcSetBPuname(uname); }
	[ClientRpc]
	void RpcSetBPuname(string uname) { BPuname = uname; }
	[Command]
	void CmdSetBEuname(string uname) { RpcSetBEuname(uname); }
	[ClientRpc]
	void RpcSetBEuname(string uname) { BEuname = uname; }

	private void releaseCurrentRoleSelection() {
		// we regressed from the beautiful 2d role array to this ugliness accommodate the unity syncvars
		// abstract this if we every go beyond 4 players
		GameManager.Team myTeam = GameManager.instance.teamSelection;
		GameManager.Role myRole = GameManager.instance.roleSelection;
		if (myTeam == GameManager.Team.Red && myRole == GameManager.Role.Pilot) {
			if(isClient) {
				CmdSetRPpid("");
				CmdSetRPuname("");
			} else {
				RpcSetRPpid("");
				RpcSetRPuname("");
			}
			Debug.Log("I released Red Pilot");
		} else if (myTeam == GameManager.Team.Red && myRole == GameManager.Role.Engineer) {
			if(isClient) {
				CmdSetREpid("");
				CmdSetREuname("");
			} else {
				RpcSetREpid("");
				RpcSetREuname("");
			}
			Debug.Log("I selected Red Engineer");
		} else if (myTeam == GameManager.Team.Blue && myRole == GameManager.Role.Pilot) {
			if(isClient) {
				CmdSetBPpid("");
				CmdSetBPuname("");
			} else {
				RpcSetBPpid("");
				RpcSetBPuname("");
			}
			Debug.Log("I selected Blue Pilot");
		} else if (myTeam == GameManager.Team.Blue && myRole == GameManager.Role.Engineer) {
			if(isClient) {
				CmdSetBEpid("");
				CmdSetBEuname("");
			} else {
				RpcSetBEpid("");
				RpcSetBEuname("");
			}
			Debug.Log("I selected Blue Engineer");
		} // else do nothing
	}

	// selection functions
	public void selectRedPilot() {
		if (this.RPpid == "" && this.RPuname == "") { // role available
			releaseCurrentRoleSelection(); // release selection
			if(isClient) { // update everyone
				CmdSetRPpid(this.myPid);
				CmdSetRPuname(this.myUserName);
			} else {
				RpcSetRPpid(this.myPid);
				RpcSetRPuname(this.myUserName);
			} // record selection
			GameManager.instance.teamSelection = GameManager.Team.Red;
			GameManager.instance.roleSelection = GameManager.Role.Pilot;
			Debug.Log("I selected Red Pilot");
		}
	}

	public void selectRedEngineer() {
		if (this.REpid == "" && this.REuname == "") { // role available
			releaseCurrentRoleSelection(); // release selection
			if(isClient) { // update everyone
				CmdSetREpid(this.myPid);
				CmdSetREuname(this.myUserName);
			} else {
				RpcSetREpid(this.myPid);
				RpcSetREuname(this.myUserName);
			} // record selection
			GameManager.instance.teamSelection = GameManager.Team.Red;
			GameManager.instance.roleSelection = GameManager.Role.Engineer;
			Debug.Log("I selected Red Engineer");
		}
	}

	public void selectBluePilot() {
		if (this.BPpid == "" && this.BPuname == "") { // role available
			releaseCurrentRoleSelection(); // release selection
			if(isClient) { // update everyone
				CmdSetBPpid(this.myPid);
				CmdSetBPuname(this.myUserName);
			} else {
				RpcSetBPpid(this.myPid);
				RpcSetBPuname(this.myUserName);
			} // record selection
			GameManager.instance.teamSelection = GameManager.Team.Blue;
			GameManager.instance.roleSelection = GameManager.Role.Pilot;
			Debug.Log("I selected Blue Pilot");
		}
	}

	public void selectBlueEngineer() {
		if (this.BEpid == "" && this.BEuname == "") { // role available
			releaseCurrentRoleSelection(); // release selection
			if(isClient) { // update everyone
				CmdSetBEpid(this.myPid);
				CmdSetBEuname(this.myUserName);
			} else {
				RpcSetBEpid(this.myPid);
				RpcSetBEuname(this.myUserName);
			} // record selection
			GameManager.instance.teamSelection = GameManager.Team.Blue;
			GameManager.instance.roleSelection = GameManager.Role.Engineer;
			Debug.Log("I selected Blue Engineer");
		}
	}

	public void playerReady () {
		GameManager.Role myRole = GameManager.instance.roleSelection;
		if (myRole == GameManager.Role.Pilot) {
			Debug.Log("Loading pilot mode...");
			SceneManager.LoadScene("pilot");
		} else if (myRole == GameManager.Role.Engineer) {
			Debug.Log("Loading engineer mode...");
			SceneManager.LoadScene("engineer");
		} // None selection does nothing
	}

	// returns to lobby screen
	public void backToLobby () {
		GameManager.instance.gameName = "";
		GameManager.instance.gamePass = "";
		GameManager.instance.teamSelection = GameManager.Team.None;
		GameManager.instance.roleSelection = GameManager.Role.None;
		SceneManager.LoadScene("GameLobbyScreen");
		// detach from game instance
	}
}
