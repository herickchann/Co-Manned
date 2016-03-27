using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {
	// wire up GameManager
	GameObject gameManager;

	// wire up global game info
	GlobalData globalData;

	public const int maxHealth = 50;

	[SyncVar]
	public int health = maxHealth;

	void Awake(){
		globalData = GetComponent<GlobalData> ();
		health = maxHealth;
	}

	void Start(){
		
	}

	[ClientRpc]
	void RpcDamage(GameManager.Team whoGotHit, int amount){
		var statusText = GetComponent<PilotMechController>().statusText;
		Debug.Log (GameManager.teamString(whoGotHit) + " got hit" + amount.ToString());
	}

	// server executes this: we know who got hit, and by how much
	public void TakeDamage(GameManager.Team teamGotHit, int amount){
		if (!isServer)
			return;

		// reduce health on the server first
		health -= amount;

		// tell everyone who got hit
		RpcDamage (teamGotHit, amount);

		// also update its copy of global data
		if (globalData.getHealth(teamGotHit) <= 0)
		{
			health = 0;
			Debug.Log("Dead!");
		}
	}
}
