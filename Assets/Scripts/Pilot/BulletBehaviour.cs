using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletBehaviour : MonoBehaviour {

	public float speed;
	private float lifeTime = 2.0f;
	public GameObject globalData;
	public GameManager.Team shooter;

	void Awake(){
		globalData = GameObject.Find("GlobalGameData");
	}
	
	void Start() {
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		Destroy (gameObject, lifeTime);
	}
	

	private void OnTriggerEnter (Collider other) {
		var hit = other.gameObject;
		var hitMech = hit.GetComponent<PilotMechController> ();

		if(hitMech != null){
			var combat = hitMech.GetComponent<Combat> ();
			if (combat != null) {
				Debug.Log (GameManager.teamString(shooter) + " shoots " + GameManager.teamString(hitMech.team));
				// reduce health on server then replicate to client
				combat.TakeDamage (hitMech.team, 10);
				Destroy (gameObject); // delete bullet
			} else {
				Debug.Log ("Error: combat is null");
			}
		}



	}
}
