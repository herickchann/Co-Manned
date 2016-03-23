using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GlobalGameDataScript : NetworkBehaviour {

	// global game data
	private const int maxHealth = 100;
	private const int maxAmmo = 100;
	[SyncVar]
	public int blueHealth = maxHealth;
	[SyncVar]
	public int redHealth = maxHealth;
	[SyncVar]
	public int blueAmmo = maxAmmo;
	[SyncVar]
	public int redAmmo = maxAmmo;


	// wire up references to 
	void Awake(){
		// make it persit through scenes
		DontDestroyOnLoad(this);
	}

	// getter functions
	public int getHealth(GameManager.Team team){
		if (team == GameManager.Team.Blue) {
			printAllHealth ();
			return blueHealth;
		} else if (team == GameManager.Team.Red) {
			printAllHealth ();
			return redHealth;
		} else {
			Debug.Log ("returning 0 with no team");
			return 0;
		}
	}

	public void printAllHealth(){
		Debug.Log ("[all health] red: " + redHealth + " blue: " + blueHealth);
	}

	// setter functions
	public void RpcReduceHealth(GameManager.Team team, int damage){
		if (team == GameManager.Team.Blue) {
			blueHealth -= damage;
		} else if (team == GameManager.Team.Red) {
			redHealth -= damage;
		}
		Debug.Log ("[updated health] red: " + redHealth + " blue: " + blueHealth);
	}

	[ClientRpc]
	public void RpcNotifyHit(GameManager.Team team){
		Debug.Log (GameManager.teamString(team) + " got hit");
	}

	[Command]
	public void CmdNotifyHit(GameManager.Team team, int amount){
		if(team == GameManager.Team.Blue){
			blueHealth -= amount;
		}
		else if(team == GameManager.Team.Red){
			redHealth -= amount;
		}
		RpcNotifyHit (team);
	}

	[Command]
	public void CmdNotifyConnect(GameManager.Team team, GameManager.Role role){
		Debug.Log (GameManager.roleString(role) + " on team " + GameManager.teamString(team) + " has joined the game");
	}
}
