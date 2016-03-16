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
