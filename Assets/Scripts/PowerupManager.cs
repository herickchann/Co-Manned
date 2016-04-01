using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {

    public int type;
    public float respawnTimer;
    private float timer;
    public bool onCooldown;
	
    void Start () {
        onCooldown = false;
    }

    private void OnTriggerEnter (Collider other) {
        var hit = other.gameObject;
		var hitMech = hit.GetComponent<PilotMechController> ();
        var globalData = hit.GetComponent<GlobalDataHook>();
        if(hitMech != null && !onCooldown) {
            globalData.setParam(hitMech.team, GlobalDataController.Param.PowerupType, type);
            HidePowerup();
            onCooldown = true;
            respawnTimer = Time.time;
        }
    }

    public void HidePowerup () {
		transform.gameObject.SetActive(false);
    }
    
    public void ShowPowerup () {
		transform.gameObject.SetActive(true);
    }
}
