using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerDataScript : NetworkBehaviour {


	// Mech health stuff
	const int maxHealth = 100;
	[SyncVar]
	int blueHealth = maxHealth;
	[SyncVar]
	int redHealth = maxHealth;

	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad (this);
	}
		
	public int getHealth(GameManager.Team team){
		if(team == GameManager.Team.Blue){
			Debug.Log ("reading health for blue mech");
			return blueHealth;
		}
		else if(team == GameManager.Team.Red){
			Debug.Log ("reading health for red mech");
			return redHealth;
		}
		else{
			return 0;
		}
			
	}

	[Command]
	public void CmdReduceHealth(int amount, GameManager.Team team){
		if(team == GameManager.Team.Blue){
			Debug.Log ("Blue took damage");
			blueHealth -= amount;
		}
		else if(team == GameManager.Team.Red){
			Debug.Log ("Red took damage");
			redHealth -= amount;
		}
	}

}
