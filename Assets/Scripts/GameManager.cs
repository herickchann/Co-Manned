using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManager : NetworkManager {

    public static GameManager instance = null;
    // TODO: please re-assess if these belong here
    public string userName = "";
    public string gameName = "";
    public string gamePass = "";

    public enum Team { Red, Blue, None };
    public enum Role { Pilot, Engineer, None };
    public Team teamSelection = Team.None;
    public Role roleSelection = Role.None;

    private GameObject[] spawnPoints;
    private int numSpawnsPoints;

    // Use this for initialization
    public void Join () {
		StartClient ();
	}

	public void Host () {
		StartHost ();
	}

	public string getTeamSelection(){
		if (teamSelection == Team.Red) {
			return "Red";
		} else if (teamSelection == Team.Blue) {
			return "Blue";
		} else {
			return "None";
		}
	}

	public string getRoleSelection(){
		if (roleSelection == Role.Engineer) {
			return "Engineer";
		} else if (roleSelection == Role.Pilot) {
			return "Pilot";
		} else {
			return "None";
		}
	}


	public override void OnServerConnect(NetworkConnection conn) {
		Debug.Log ("player connected");
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        //Randomly select spawn point
        Transform spawnPoint = GenerateSpawnPoint();
        
        //Spawn Player randomly on map
        GameObject player = (GameObject)GameObject.Instantiate(playerPrefab, spawnPoint.position, new Quaternion(0.0f, Random.Range(0,360), 0.0f, 0.0f));
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //Setup Camera
        GameObject camera = GameObject.FindWithTag("MainCamera");
        if (camera != null) {
            PilotCamera followScript = camera.GetComponent("PilotCamera") as PilotCamera;

            if (followScript != null) {
                followScript.player = player;
            }
        }
    }

    private Transform GenerateSpawnPoint () {
        if (spawnPoints == null) { 
            //Load all children from under the Spawn points group 
            Transform spawns = GameObject.FindWithTag("Spawn").transform;
            numSpawnsPoints = spawns.childCount;
            spawnPoints = new GameObject[numSpawnsPoints];
            for (int i = 0; i < numSpawnsPoints; i++) {
                spawnPoints[i] = spawns.GetChild(i).gameObject;
            }
        }
        //Generate different numbers to ensure true RNG
        Random.seed = (Random.Range(Random.Range(Random.Range(Random.Range(0, 25), Random.Range(324, 5673)), Random.Range(Random.Range(53, 2378), Random.Range(50, 423))), Random.Range(Random.Range(Random.Range(23, 2354), Random.Range(1, 3456)), Random.Range(Random.Range(7, 32421), Random.Range(8, 23472)))));
        int rand = Random.Range(0, numSpawnsPoints-1);
        Transform selectedPoint = spawnPoints[rand].transform;

        spawnPoints[rand].SetActive(false);
        Debug.Log("player spawned at point: " + rand);
        return selectedPoint;
    }

    void Awake () {
		DontDestroyOnLoad (this);
        if (instance == null) { // instance not set?
			instance = this; // set instance to this
		} else if (instance != this) { // already exists?
			GameObject.Destroy(gameObject); // destroy this one to enforce singleton
		}
		GameObject.DontDestroyOnLoad(gameObject); // persist between scenes
    }

    // Update is called once per frame
    void Update () {

    }
}
