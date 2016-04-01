using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {
        // keep track of spawn points
    private Transform[] powerupSpawns;
    private GameObject[] powerUps;
    private GameObject[] spawnedPowerups;
    private int arrayIndex;

    public override void OnStartServer () {
        base.OnStartServer();
        SpawnPowerups();
    }

    /*
    void Update () {
        foreach (GameObject powerup in spawnedPowerups) {
            var powerupScript = powerup.GetComponent<PowerupManager>();
            Debug.LogError(Time.time + 3);
            if (powerupScript != null && powerupScript.onCooldown && powerupScript.respawnTimer > 15) {
                powerupScript.ShowPowerup();
                powerupScript.onCooldown = false;
            } else if (powerupScript.onCooldown) {
                powerupScript.respawnTimer += Time.deltaTime;
            }
        }
    }*/

    private void SpawnPowerups() {
        arrayIndex = 0;
        powerUps = Resources.LoadAll<GameObject>("Powerups");
        Transform spawns = GameObject.Find("PowerupSpawnPoints").transform;
        spawnedPowerups = new GameObject[spawns.childCount];
        powerupSpawns = new Transform[spawns.childCount];
        for (int i = 0; i < spawns.childCount; i++) {
            powerupSpawns[i] = spawns.GetChild(i);
        }

        if (powerupSpawns != null) {
            foreach (Transform powerupPoint in powerupSpawns) { 
                //Randomize powerup types
                int index = Random.Range(0, powerUps.Length);

                GameObject powerUp = (GameObject)Instantiate(powerUps[index], powerupPoint.position, powerupPoint.rotation);
                spawnedPowerups[arrayIndex] = powerUp;
                arrayIndex++;
                NetworkServer.Spawn(powerUp);
            }
        }
    }
}
