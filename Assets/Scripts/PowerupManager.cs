using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {

    public int type;
    public int respawnTime;
    private float timer;
    private bool onCooldown;
    private GameObject parent;
	
    void Start () {
        onCooldown = false;
        parent = transform.parent.gameObject;
    }

    void Update () {
        if (onCooldown) { 
            timer += Time.deltaTime;
            if (timer > respawnTime) {
                timer = 0;
                ShowPowerup();
                onCooldown = false;
            }
        }
    }

    private void OnTriggerEnter (Collider other) {
        var hit = other.gameObject;
		var hitMech = hit.GetComponent<PilotMechController> ();
        Debug.Log("POWERUP HIT");
        if(hitMech != null && !onCooldown) {
            hitMech.powerupType = type;
            HidePowerup();
            onCooldown = true;
        }
    }

    private void HidePowerup () {
        parent.SetActive(false);
    }
    
    private void ShowPowerup () {
        parent.SetActive(true);
    }
}
