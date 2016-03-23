using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {
        // keep track of spawn points
    private Transform[] powerupSpawns;
    private GameObject[] powerUps;

    public override void OnStartServer () {
        base.OnStartServer();
        SpawnPowerups();
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
