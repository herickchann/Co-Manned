using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameRoomPlayer : NetworkBehaviour {

	// synchronized fields
	// player id slots (for underlying selection)
	[SyncVar] // user name for rendering (might not be unique)
	public SyncListString unameList = new SyncListString();

	public string myUserName = "";

	// Game Room UI reference is set in Start()
	public GameRoomScreenScript GameRoomUI;
	public GameRoomManager GameRoomMgr;

	private const int maxPlayers = 4;

	// Use this for initialization
	void Start () {
		Debug.Log("Game Room Player spawned");
		this.myUserName = GameManager.instance.userName;
		// start with no selection
		GameManager.instance.roleSelection = GameManager.Role.None;
		GameManager.instance.teamSelection = GameManager.Team.None;
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
		if (unameList.Count != maxPlayers) { // if list is not initialized
			for(int i=0;i<maxPlayers;i++) { unameList.Add(""); }
		}
		unameList.Callback = OnUnameListChanged;

		GameObject GameRoomObj = GameObject.Find("/GameRoomManager");
		Debug.Assert(GameRoomObj, "Game Room Manager not found");
		GameRoomManager GameRoomMan = GameRoomObj.GetComponent<GameRoomManager>();
		Debug.Assert(GameRoomMan);
		this.GameRoomMgr = GameRoomMan;

	}

	public void OnUnameListChanged(SyncListString.Operation op, int index) {
		Debug.LogError("list op " + op + " on idx " + index.ToString());
	}

	// Update is called once per frame
	void Update () {
		// update UI data
		GameRoomUI.RPuname = unameList[0];
		GameRoomUI.REuname = unameList[1];
		GameRoomUI.BPuname = unameList[2];
		GameRoomUI.BEuname = unameList[3];
	}

	// hardcode index mappings for now
	public int getTeamRoleIndex(GameManager.Team team, GameManager.Role role) {
		if (team == GameManager.Team.Red  && role == GameManager.Role.Pilot) 	return 0;
		if (team == GameManager.Team.Red  && role == GameManager.Role.Engineer) return 1;
		if (team == GameManager.Team.Blue && role == GameManager.Role.Pilot) 	return 2;
		if (team == GameManager.Team.Blue && role == GameManager.Role.Engineer) return 3;
		Debug.LogError("Bad team / role selection");
		return -1;
	}
		
	[Command]
	void CmdUpdateUnameList(int idx, string uname) {
		unameList[idx] = uname;
		unameList.Dirty(idx);
		Debug.Log("Tried to update entry " + idx.ToString() + " with uname " + uname);
	}
	
	private void releaseCurrentRoleSelection() {
		Debug.Assert((GameManager.instance.teamSelection == GameManager.Team.None &&
			GameManager.instance.roleSelection == GameManager.Role.None) ||
			(GameManager.instance.teamSelection != GameManager.Team.None &&
				GameManager.instance.roleSelection != GameManager.Role.None),
			"Invalid selection state: team and role must be both None or both not None");
		if (GameManager.instance.teamSelection == GameManager.Team.None &&
			GameManager.instance.roleSelection == GameManager.Role.None) return; // nothing to release
		GameManager.Team myTeam = GameManager.instance.teamSelection;
		GameManager.Role myRole = GameManager.instance.roleSelection;
		int idx = getTeamRoleIndex(myTeam, myRole);
		Debug.Assert(0 <= idx && idx < maxPlayers, "Bad team/role index: " + idx.ToString());
		if (isServer) {
			unameList[idx] = "";
			unameList.Dirty(idx);
			Debug.Log("release as server");
		} else {
			CmdUpdateUnameList(idx, "");
			Debug.Log("release as client");
		}
		GameManager.instance.teamSelection = GameManager.Team.None;
		GameManager.instance.roleSelection = GameManager.Role.None;
	}

	public void selectTeamRole(GameManager.Team selectedTeam, GameManager.Role selectedRole) {
		int idx = getTeamRoleIndex(selectedTeam, selectedRole);
		Debug.Assert(0 <= idx && idx < maxPlayers, "Bad team/role index: " + idx.ToString());
		if (unameList[idx] == "") { // if slot is available
			releaseCurrentRoleSelection();
			if (isServer) {
				unameList[idx] = myUserName;
				unameList.Dirty(idx);
				Debug.Log("select as server");
			} else {
				CmdUpdateUnameList(idx, myUserName);
				Debug.Log("select as client");
			}
			GameManager.instance.teamSelection = selectedTeam;
			GameManager.instance.roleSelection = selectedRole;
			Debug.LogError("selection successful");
		}
	}

	[Server]
	public void updateSharedVar(string x) {
		// only the server has authority to change the shared game room manager
		this.GameRoomMgr.updateTest(x);
	}

	[Command]
	public void CmdUpdateSharedVar(string x) {
		updateSharedVar(x);
	}

	// selection functions
	public void selectRedPilot() {
		if(!isLocalPlayer) return;
		Debug.Log("My username is " + myUserName);
		this.CmdUpdateSharedVar(myUserName);
		//selectTeamRole(GameManager.Team.Red, GameManager.Role.Pilot);
	}

	public void selectRedEngineer() {
		if (!isLocalPlayer) return;
		selectTeamRole(GameManager.Team.Red, GameManager.Role.Engineer);
	}

	public void selectBluePilot() {
		if (!isLocalPlayer) return;
		selectTeamRole(GameManager.Team.Blue, GameManager.Role.Pilot);
	}

	public void selectBlueEngineer() {
		if (!isLocalPlayer) return;
		selectTeamRole(GameManager.Team.Blue, GameManager.Role.Engineer);
	}
}
