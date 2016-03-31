using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {

    public int type;
    public int respawnTime;
    private float timer;
    private bool onCooldown;
	
    void Start () {
        onCooldown = false;
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
        var globalData = hit.GetComponent<GlobalDataHook>();
        Debug.Log("POWERUP HIT");
        if(hitMech != null && !onCooldown) {
            globalData.setParam(hitMech.team, GlobalDataController.Param.PowerupType, type);
            HidePowerup();
            onCooldown = true;
        }
    }

    private void HidePowerup () {
		transform.gameObject.SetActive(false);
    }
    
    private void ShowPowerup () {
		transform.gameObject.SetActive(true);
    }
}
