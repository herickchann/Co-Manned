using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour {

	public GameRoomScreenScript GameRoomUI; // reference for when we want to push UI changes
	public GameRoomSlots roomSlots; // rooms slots that are sync'd on all clients

	public string myUserName = "";
	public string myPid = "";
	// note: my local selection is not sync'd
	[SyncVar]
	public GameManager.Team team;
	[SyncVar]
	public GameManager.Role role;

	void Start(){
		Debug.Log("Lobby Player spawned");
		// set up player identifiers
		this.myUserName = GameManager.instance.userName;
		// for dev purposes, if no username exists, use "dev"
		if (this.myUserName == "") {this.myUserName = "dev";}
		this.myPid = netId.ToString();
		setTeamInfo(GameManager.Team.None, GameManager.Role.None);

		// attach to UI
		GameObject UIEmpty = GameObject.Find("/UIEmpty");
		Debug.Assert(UIEmpty);
		GameRoomScreenScript GameRoomScreenUI = UIEmpty.GetComponent<GameRoomScreenScript>();
		Debug.Assert(GameRoomScreenUI);
		this.GameRoomUI = GameRoomScreenUI;
		// register listeners for selection (button push means I want this role)
		GameRoomUI.RedPilot.onClick.AddListener(() => selectRedPilot());
		GameRoomUI.RedEngineer.onClick.AddListener(() => selectRedEngineer());
		GameRoomUI.BluePilot.onClick.AddListener(() => selectBluePilot());
		GameRoomUI.BlueEngineer.onClick.AddListener(() => selectBlueEngineer());
		GameRoomUI.ClearSelection.onClick.AddListener(() => clearSelection());
		GameRoomUI.PlayButton.onClick.AddListener(() => startGame());
		if (isServer) { // default is ready
			GameRoomUI.PlayButton.GetComponentInChildren<Text>().text = "Start Game";
		}

		// attach to the shared room object
		GameObject GameRoomObj = GameObject.Find("/GameRoomSlots");
		Debug.Assert(GameRoomObj, "Game Room Slots not found");
		GameRoomSlots gameRoomSlots = GameRoomObj.GetComponent<GameRoomSlots>();
		Debug.Assert(gameRoomSlots);
		this.roomSlots = gameRoomSlots;
	}

	void Update () {
		// constantly push updated data to UI
		if (GameRoomUI == null) return;
		for(int idx = 0; idx < GameRoomSlots.maxPlayers; idx++ ){
			GameRoomUI.unameArray[idx] = roomSlots.unameList[idx];
		}
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

	[Server]
	public void releaseRoomSlot(int idx) {
		// only the server has authority to change the shared game room slots
		roomSlots.releaseSlot(idx);
	}

	[Command]
	public void CmdReleaseRoomSlot(int idx) {
		releaseRoomSlot(idx); // tell server to release the slot
	}

	private void releaseCurrentRoleSelection() {
		if(!isLocalPlayer) return;
		if (team == GameManager.Team.None && role == GameManager.Role.None) return; // nothing to release
		int idx = getTeamRoleIndex(team, role);
		Debug.Assert(0 <= idx && idx < GameRoomSlots.maxPlayers, "Bad team/role index: " + idx.ToString());
		//Debug.LogError("attempting to release slot " + idx.ToString());
		CmdReleaseRoomSlot(idx);
		setTeamInfo(GameManager.Team.None, GameManager.Role.None);
	}

	[Server]
	public void takeRoomSlot(int idx, string uname, string pid) {
		// only the server has authority to change the shared game room slots
		roomSlots.takeSlot(idx, uname, pid);
	}

	[Command]
	public void CmdTakeRoomSlot(int idx, string uname, string pid) {
		takeRoomSlot(idx, uname, pid); // tell server to book the slot
	}

	public void selectTeamRole(GameManager.Team selectedTeam, GameManager.Role selectedRole) {
		if(!isLocalPlayer) return;
		int idx = getTeamRoleIndex(selectedTeam, selectedRole);
		Debug.Assert(0 <= idx && idx < GameRoomSlots.maxPlayers, "Bad team/role index: " + idx.ToString());
		if (roomSlots.pidList[idx] == "") { // if slot is available
			releaseCurrentRoleSelection();
			//Debug.LogError("attempting to take slot " + idx.ToString());
			CmdTakeRoomSlot(idx, myUserName, myPid);
			setTeamInfo(selectedTeam, selectedRole);
		}
	}

	// selection functions
	public void selectRedPilot() {
		selectTeamRole(GameManager.Team.Red, GameManager.Role.Pilot);
		GameRoomUI.showRedPilot();
	}
	public void selectRedEngineer() {
		selectTeamRole(GameManager.Team.Red, GameManager.Role.Engineer);
		GameRoomUI.showRedEngineer();
	}
	public void selectBluePilot() {
		selectTeamRole(GameManager.Team.Blue, GameManager.Role.Pilot);
		GameRoomUI.showBluePilot();
	}
	public void selectBlueEngineer() {
		selectTeamRole(GameManager.Team.Blue, GameManager.Role.Engineer);
		GameRoomUI.showBlueEngineer();
	}
	public void clearSelection() {
		releaseCurrentRoleSelection();
		GameRoomUI.showNone();
		//GetComponent<NetworkLobbyPlayer>().SendNotReadyToBeginMessage(); // does not work (pilot despawns)
		//Debug.LogError("Client not ready");
	}

	public void startGame() {
		if(!isLocalPlayer) return;
		//GetComponent<NetworkLobbyPlayer>().SendReadyToBeginMessage(); // for host client too - does not work either
		//Debug.LogError("Client ready");
		if(isServer) { // only host can start the game
			Debug.LogError("Starting game...");
			GameObject.Find("LobbyManager").GetComponent<LobbyManager>().startGame();
		}
	}

	public override void OnStartClient() {
		Debug.Log("OnStartClient called");
		base.OnStartClient(); // disable to see that exception is gone
	}

	[Command]
	public void CmdSetTeamInfo (string id, GameManager.Team selectedTeam, GameManager.Role selectedRole){
		Debug.Log ("SERVER updating team and role: user: ["+ id +"] picking: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));
		// set player object team and role on the server
		team = selectedTeam;
		role = selectedRole;
	}

	public void setTeamInfo(GameManager.Team selectedTeam, GameManager.Role selectedRole){
		if (!isLocalPlayer){
			Debug.Log ("not client");
			return;
		}
		// let the server know
		CmdSetTeamInfo (GetComponent<NetworkIdentity>().netId.ToString(), selectedTeam, selectedRole);
	}
}
