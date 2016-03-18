using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkManagerScript : NetworkManager {

    // keep track of spawn points
    private Transform[] powerupSpawns;
    private GameObject[] powerUps;

    // wire up game manager
    public GameObject gameManager;

    void Awake () {
        gameManager = GameObject.Find("GameManager");
    }

    // Use this for initialization
    public void Join () {
        StartClient();
    }

    public void Host () {
        StartHost();
    }

    //void Update () {
    //    //Debug.Log(Time.deltaTime);
    //}

    public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId) {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        if (numPlayers > 1) {
            SpawnPowerups();
        }
    }

    public override void OnServerConnect (NetworkConnection conn) {

    }

    public override void OnClientSceneChanged (NetworkConnection conn) {
        GameManager.Role role = gameManager.GetComponent<GameManager>().getRoleSelection();

        if (role == GameManager.Role.Pilot) {
            base.OnClientSceneChanged(conn);
        }
    }

    public override void OnClientConnect (NetworkConnection conn) {
        Debug.Log("player connected");
        GameManager.Role role = gameManager.GetComponent<GameManager>().getRoleSelection();

        if (role == GameManager.Role.Engineer) {
            Debug.Log("entered as engineer");
            //ClientScene.RemovePlayer (0);
            SceneManager.LoadScene("engineer");
        }
    }
    
    private void SpawnPowerups() {
        powerUps = Resources.LoadAll<GameObject>("Powerups");
        Transform spawns = GameObject.Find("PowerupSpawnPoints").transform;
        powerupSpawns = new Transform[spawns.childCount];
        for (int i = 0; i < spawns.childCount; i++) {
            powerupSpawns[i] = spawns.GetChild(i);
        }

        if (powerupSpawns != null) {
            foreach (Transform powerupPoint in powerupSpawns) { 
                //Randomize powerup types
                int index = Random.Range(0, powerUps.Length);

                GameObject powerUp = (GameObject)Instantiate(powerUps[index], powerupPoint.position, powerupPoint.rotation);
                NetworkServer.Spawn(powerUp);
            }
        }
    }
}
