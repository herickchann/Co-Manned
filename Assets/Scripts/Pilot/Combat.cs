using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {
	// wire up GameManager
	GameObject gameManager;
	PilotMechController mechController;

	public const int maxHealth = 50;

	[SyncVar]
	public int health = maxHealth;

	void Awake(){
		mechController = GetComponent<PilotMechController> ();
		health = maxHealth;
	}

	void Start(){
		
	}

	[ClientRpc]
	void RpcDamage(int amount){
		var statusText = GetComponent<PilotMechController>().statusText;
	}

	public void TakeDamage(GameManager.Team team, int amount){
		if (!isServer)
			return;

		health -= amount;
		//CmdNotifyHit (team);

		RpcDamage (amount);

		// also update its copy of global data
		if (health <= 0)
		{
			health = 0;
			Debug.Log("Dead!");
		}
	}
}
