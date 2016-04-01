using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameRoomSlots : NetworkBehaviour {

	// singleton
	static GameRoomSlots s_instance;

	[SyncVar] // user names for rendering buttons (might not be unique)
	public SyncListString unameList = new SyncListString();
	[SyncVar] // player id for uniquely identifying players (in case of duplicate user names)
	public SyncListString pidList = new SyncListString();

	public const int maxPlayers = 4;

	public void OnUnameListChanged(SyncListString.Operation op, int index) {
		Debug.LogError("list op " + op + " on idx " + index.ToString()); // log updates
	}

	public void OnPidListChanged(SyncListString.Operation op, int index) {
		Debug.LogError("list op " + op + " on idx " + index.ToString()); // log updates
	}
    void Awake () {
		if (s_instance == null) {
			s_instance = this;
			
		} else {
			Destroy (gameObject);
		}
        DontDestroyOnLoad(this);
    }
	// Use this for initialization
	void Start () {
		if (unameList.Count != maxPlayers) { // if list is not initialized
			for(int i=0;i<maxPlayers;i++) { unameList.Add(""); } // create 4 empty slots
		}
		if (pidList.Count != maxPlayers) { // if list is not initialized
			for(int i=0;i<maxPlayers;i++) { pidList.Add(""); } // create 4 empty slots
		}
		//unameList.Callback = OnUnameListChanged; // attach unameList callback
		//pidList.Callback = OnPidListChanged; // attach pidList callback
		Debug.Log("Game Room Slots created");
	}

	// note: list.Dirty(idx) means that element has been modified and should be updated on clients (i.e. push change)

	[Server]
	public void releaseId(string pid) {
		for(int idx=0; idx<maxPlayers; idx++) { // iterate through the list until we find the id we want
			if (pidList[idx] == pid) {
				releaseSlot(idx); // release the slot and exit (only expecting 1 or 0 copies of this id)
				break;
			}
		}
	}

	public void releaseSlot(int idx) {
		Debug.Assert(0 <= idx && idx < maxPlayers, "Tried to release slot with invalid index " + idx.ToString());
		unameList[idx] = "";
		unameList.Dirty(idx);
		pidList[idx] = "";
		pidList.Dirty(idx);
		//Debug.LogError("user released slot " + idx.ToString());
	}

	public void takeSlot(int idx, string uname, string pid) {
		Debug.Assert(0 <= idx && idx < maxPlayers, "Tried to take slot with invalid index " + idx.ToString());
		unameList[idx] = uname;
		unameList.Dirty(idx);
		pidList[idx] = pid;
		pidList.Dirty(idx);
		//Debug.LogError("user " + uname + " with pid " + pid + " updated slot " + idx.ToString());
	}

	public void lookupTeamRole(string pid, out GameManager.Team team, out GameManager.Role role) {
		// team and role none by default
		team = GameManager.Team.None;
		role = GameManager.Role.None;
		for(int idx=0; idx<maxPlayers; idx++) { // iterate through the list until we find the id we want
			if (pidList[idx] == pid) {
				switch(idx) { // depending on slot idx, return the team and role
				case 0:
					team = GameManager.Team.Red;
					role = GameManager.Role.Pilot;
					break;
				case 1:
					team = GameManager.Team.Red;
					role = GameManager.Role.Engineer;
					break;
				case 2:
					team = GameManager.Team.Blue;
					role = GameManager.Role.Pilot;
					break;
				case 3:
					team = GameManager.Team.Blue;
					role = GameManager.Role.Engineer;
					break;
				}
				return; // found it! exit loop
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

}
