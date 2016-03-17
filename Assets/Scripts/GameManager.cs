using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public static GameManager instance = null;
	// TODO: please re-assess if these belong here
	public string userName = "";
	public string gameName = "";
	public string gamePass = "";

	public enum Team{Red, Blue, None};
	public enum Role{Pilot, Engineer, None};
	public Team teamSelection = Team.None;
	public Role roleSelection = Role.None;


	// Shared variables across clients
	/*const int maxHealth = 10;

	public int blueHealth = 60;
	public int redHealth = 80;

	public int getHealth(Team team){
		if (team == Team.Red) {
			Debug.Log ("loaded red health: " + redHealth.ToString());
			return redHealth;
		} else if(team == Team.Blue){
			Debug.Log ("loaded blue health: " + blueHealth.ToString());
			return blueHealth;
		}
		else{
			return 0;
		}
	}
		
	public void ReduceHealth(Team team, int amount){
		if (team == Team.Red) {
			redHealth -= amount;
			Debug.Log ("red took damage and it is now: " + redHealth.ToString());
		} else if(team == Team.Blue){
			blueHealth -= amount;
			Debug.Log ("blue took damage and it is now: " + blueHealth.ToString());
		}
		else{
		}
	}*/
		

	// Use this for initialization
	void Awake () {
		if (instance == null) { // instance not set?
			instance = this; // set instance to this
		} else if (instance != this) { // already exists?
			GameObject.Destroy(gameObject); // destroy this one to enforce singleton
		}
		GameObject.DontDestroyOnLoad(gameObject); // persist between scenes
		GameObject.DontDestroyOnLoad(this);

	}
		
		
	public GameManager.Team getTeamSelection(){
		return teamSelection;
	}

	public GameManager.Role getRoleSelection(){
		return roleSelection;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
