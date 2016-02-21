using UnityEngine;
using System.Collections;

public class MechController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
	private Quaternion _lookRotation;
	private Vector3 _direction;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }
		
	void Update() {
		foreach (Touch touch in Input.touches) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(touch.position);

			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.name == "GroundPlane") {
					if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
						transform.LookAt(hit.point);
					}
				}
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {

				Debug.Log (hit.transform.name);
				if (hit.transform.name == "GroundPlane") {
					transform.LookAt(hit.point);
				}
		
			}
		}
	}

    void FixedUpdate() {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveH, 0.0f, moveV);

//		Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, Input.acceleration.y);
		movement = transform.TransformDirection(movement);
		rb.velocity = movement * speed;
    }
}
