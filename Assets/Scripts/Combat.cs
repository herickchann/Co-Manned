using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Combat : NetworkBehaviour {
	public const int maxHealth = 100;

	[SyncVar]
	public int health = maxHealth;

	[ClientRpc]
	void RpcDamage(int amount){
		var statusText = GetComponent<PilotMechController> ().statusText;
		statusText.GetComponent<TextMesh>().text = health.ToString();
	}
	public void TakeDamage(int amount){
		if (!isServer)
			return;

		health -= amount;
		RpcDamage (amount);

		if (health <= 0)
		{
			health = 0;
			Debug.Log("Dead!");
		}
	}
}
