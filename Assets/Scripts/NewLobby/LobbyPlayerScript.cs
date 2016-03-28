using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkLobbyPlayer {

	[SyncVar]
	public GameManager.Team team;
	[SyncVar]
	public GameManager.Role role;

	[Command]
	public void CmdNewPlayer(string netid){
		RpcNewPlayer (netid);
	}

	[ClientRpc]
	public void RpcNewPlayer(string netid){
		Debug.Log("new player: "+ netid +" has joined");
	}


	void Start(){
		// hook lobby player to room for interaction
		if(isLocalPlayer){
			Debug.Log("my lobby slot is " + slot.ToString());
			CmdNewPlayer (GetComponent<NetworkIdentity>().netId.ToString());
			GameObject.Find ("GameRoomUIEmpty").GetComponent<GameRoomScreenScript>().lobbyPlayer = GetComponent<NetworkLobbyPlayer> ();
		}
	}

	[ClientRpc]
	public void RpcLockSelection(string id, GameManager.Team selectedTeam, GameManager.Role selectedRole){
		Debug.Log ("CLIENT: user:[" + id +"] picked: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));
		GameObject.Find("GameRoomUIEmpty").GetComponent<GameRoomScreenScript>().lockButton(selectedTeam, selectedRole);
	}

	[ClientRpc]
	public void RpcUnlockSelection(string id, GameManager.Team selectedTeam, GameManager.Role selectedRole) {
		Debug.Log ("CLIENT: user:[" + id +"] released: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));
		GameObject.Find("GameRoomUIEmpty").GetComponent<GameRoomScreenScript>().unlockButton(selectedTeam, selectedRole);
	}

	[Command]
	public void CmdSetTeamInfo (string id, GameManager.Team selectedTeam, GameManager.Role selectedRole){
		Debug.Log ("SERVER updating team and role: user: ["+ id +"] picking: " + GameManager.teamString(selectedTeam) + " " + GameManager.roleString(selectedRole));

		// release current selection
		if (team != GameManager.Team.None) {
			RpcUnlockSelection(id, team, role);
		}

		// set player object team and role on the server
		team = selectedTeam;
		role = selectedRole;

		// let everyone else know what role has been picked
		RpcLockSelection (id, team, role);

	}

	// detects ui interaction, need to know everything what team/role the player picked
	public void setTeamInfo(GameManager.Team selectedTeam, GameManager.Role selectedRole){
		if (!isLocalPlayer){
			Debug.Log ("not client");
			return;
		}
		// let the server know
		CmdSetTeamInfo (GetComponent<NetworkIdentity>().netId.ToString(), selectedTeam, selectedRole);
	}


}
