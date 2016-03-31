using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {
	// wire up GameManager
	GameObject gameManager;

	// wire up global game info
	GlobalDataHook globalDataHook;

	public const int maxHealth = 50;

	[SyncVar]
	public int health = maxHealth;

	void Awake(){
		globalDataHook = GetComponent<GlobalDataHook> ();
		health = maxHealth;
	}

	void Start(){
		
	}

	[ClientRpc]
	void RpcDamage(GameManager.Team whoGotHit, int amount){
		Debug.Log (GameManager.teamString(whoGotHit) + " got hit" + amount.ToString());
	}

	// server executes this: we know who got hit, and by how much
	public void TakeDamage(GameManager.Team teamGotHit, int amount){
		if (!isServer)
			return;

        health = globalDataHook.getParam(teamGotHit, GlobalDataController.Param.Health);
		// reduce health on the server first
        globalDataHook.setParam(teamGotHit, GlobalDataController.Param.Health, health-amount);

		// tell everyone who got hit
		RpcDamage (teamGotHit, amount);

		// also update its copy of global data
		if (globalDataHook.getParam(teamGotHit, GlobalDataController.Param.Health) <= 0)
		{
			globalDataHook.setParam(teamGotHit, GlobalDataController.Param.Health, 0);
			Debug.Log("Dead!");
		}
	}
}
