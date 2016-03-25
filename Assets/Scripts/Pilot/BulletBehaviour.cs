using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BulletBehaviour : MonoBehaviour {

	public float speed;
	public GameObject globalData;
	public GameManager.Team shooter;
    public ParticleSystem explosion;

	void Awake(){
		globalData = GameObject.Find("GlobalGameData");
	}
	
	void Start() {
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
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
			} else {
				Debug.Log ("Error: combat is null");
			}
		}

        // Unparent the particles from the shell.
        explosion.transform.parent = null;

        // Play the particle system.
        explosion.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        Destroy (explosion.gameObject, explosion.duration);
		Destroy (gameObject); // delete bullet
	}
}
