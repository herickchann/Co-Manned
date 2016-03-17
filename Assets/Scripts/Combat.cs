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
		mechController.statusText.GetComponent<TextMesh>().text = health.ToString ();
		/*
		gameManager = GameObject.Find ("GameManager");
		if (gameManager != null) {
			var gameData = gameManager.GetComponent<GameManager> ();
			if (gameData != null) {
				GameManager.Team team = gameData.teamSelection;
				health = gameData.getHealth(team);

			} else {
				Debug.Log ("fail to load game data");
			}
		} else {
			Debug.Log ("fail to load game manager");
		}*/


	}

	[ClientRpc]
	void RpcDamage(int amount){	
		var statusText = GetComponent<PilotMechController>().statusText;
		Debug.Log ("updated health to: " + health.ToString ());
		statusText.GetComponent<TextMesh> ().text = health.ToString ();
	}
		

	public void TakeDamage(int amount){
		if (!isServer)
			return;

		health -= amount;
		Debug.Log ("took damage, health is now: " + health.ToString());
		//GameManager.Team team = GetComponent<PilotMechController> ().team;
		//GameObject.Find ("GameManager").GetComponent<GameManager> ().ReduceHealth (team, amount);
		RpcDamage (amount);

		if (health <= 0)
		{
			health = 0;
			Debug.Log("Dead!");
		}
	}
}
