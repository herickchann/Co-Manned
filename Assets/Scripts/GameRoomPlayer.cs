using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameRoomPlayer : NetworkBehaviour {

	// synchronized fields
	// unfortunately, syncvars only works with straight primitives so we shall write out the fields
	// until the need for further abstraction arises
	// Unity does not support pointers so a lot of explicit calls to specific get/set methods are necessary
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

	// identifiers that should not change once in the game room
	public string myPid = "";
	public string myUserName = "";

	// Game Room UI reference is set in Start()
	public GameRoomScreenScript GameRoomUI;

	// Use this for initialization
	void Start () {
		Debug.Log("Game Room Player spawned");
		Debug.Assert(playerControllerId >= 0);
		this.myPid = playerControllerId.ToString();
		this.myUserName = GameManager.instance.userName;
		// for dev purposes, if no username exists, use "me"
		if (this.myUserName == "") {this.myUserName = "me";}
		// register button update functions
		GameObject UIEmpty = GameObject.Find("/UIEmpty");
		Debug.Assert(UIEmpty);
		GameRoomScreenScript GameRoomScreenUI = UIEmpty.GetComponent<GameRoomScreenScript>();
		Debug.Assert(GameRoomScreenUI);
		this.GameRoomUI = GameRoomScreenUI;
		// register listeners for selection
		GameRoomUI.RPButton.onClick.AddListener(() => selectRedPilot());
		GameRoomUI.REButton.onClick.AddListener(() => selectRedEngineer());
		GameRoomUI.BPButton.onClick.AddListener(() => selectBluePilot());
		GameRoomUI.BEButton.onClick.AddListener(() => selectBlueEngineer());
	}
	
	// Update is called once per frame
	void Update () {
		// update UI data
		GameRoomUI.RPuname = this.RPuname;
		GameRoomUI.REuname = this.REuname;
		GameRoomUI.BPuname = this.BPuname;
		GameRoomUI.BEuname = this.BEuname;
	}

	// verbose boilerplate to support syncvars
	// cmd = client -> server
	// rpc = server -> clients
	// player id sync functions
	[Command]
	void CmdSetRPpid(string pid) { RPpid = pid; RpcSetRPpid(pid); }
	[ClientRpc]
	void RpcSetRPpid(string pid) { RPpid = pid; }
	[Command]
	void CmdSetREpid(string pid) { REpid = pid; RpcSetREpid(pid); }
	[ClientRpc]
	void RpcSetREpid(string pid) { REpid = pid; }
	[Command]
	void CmdSetBPpid(string pid) { BPpid = pid; RpcSetBPpid(pid); }
	[ClientRpc]
	void RpcSetBPpid(string pid) { BPpid = pid; }
	[Command]
	void CmdSetBEpid(string pid) { BEpid = pid; RpcSetBEpid(pid); }
	[ClientRpc]
	void RpcSetBEpid(string pid) { BEpid = pid; }
	// username sync functions
	[Command]
	void CmdSetRPuname(string uname) { RPuname = uname; RpcSetRPuname(uname); }
	[ClientRpc]
	void RpcSetRPuname(string uname) { RPuname = uname; }
	[Command]
	void CmdSetREuname(string uname) { REuname = uname; RpcSetREuname(uname); }
	[ClientRpc]
	void RpcSetREuname(string uname) { REuname = uname; }
	[Command]
	void CmdSetBPuname(string uname) { BPuname = uname; RpcSetBPuname(uname); }
	[ClientRpc]
	void RpcSetBPuname(string uname) { BPuname = uname; }
	[Command]
	void CmdSetBEuname(string uname) { BEuname = uname; RpcSetBEuname(uname); }
	[ClientRpc]
	void RpcSetBEuname(string uname) { BEuname = uname; }

	// TODO: find a way to abstract calls to all the set methods
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
			Debug.Log("I released Red Engineer");
		} else if (myTeam == GameManager.Team.Blue && myRole == GameManager.Role.Pilot) {
			if(isClient) {
				CmdSetBPpid("");
				CmdSetBPuname("");
			} else {
				RpcSetBPpid("");
				RpcSetBPuname("");
			}
			Debug.Log("I released Blue Pilot");
		} else if (myTeam == GameManager.Team.Blue && myRole == GameManager.Role.Engineer) {
			if(isClient) {
				CmdSetBEpid("");
				CmdSetBEuname("");
			} else {
				RpcSetBEpid("");
				RpcSetBEuname("");
			}
			Debug.Log("I released Blue Engineer");
		} // else do nothing
	}

	/*public void select(role) {
	 *	if role is Available
	 *		if isClient
	 *			CmdSet
	 *		else
	 *			RpcSet
	 *		Update my team and role
	 *		Log selection
	*/

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
}
