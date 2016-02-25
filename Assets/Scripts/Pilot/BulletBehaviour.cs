using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	public float speed;
	private float lifeTime = 2.0f;

	void Start() {
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		Destroy (gameObject, lifeTime);
	}

	private void OnTriggerEnter (Collider other) {
		Destroy (gameObject);
	}
}
