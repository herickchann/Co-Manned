using UnityEngine;
using System.Collections;
using CnControls;

public class MechController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private RectTransform joystickTrans;

	public GameObject bullet;
	public Transform bulletSpawn;
	private float nextFire;
	public float fireRate;

    private float moveH;
    private float moveV;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        moveH = CnInputManager.GetAxis("Horizontal");
        moveV = CnInputManager.GetAxis("Vertical");

        Move();
        Turn();
        Fire();
    }

    private void Move () {
        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        //Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, Input.acceleration.y);
        //movement = transform.TransformDirection(movement);
        rb.velocity = movement * speed;
    }

    private void Turn () {
        float angle = Mathf.Atan2(moveH, moveV) * Mathf.Rad2Deg;
        if (angle != 0) {
            rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    private void Fire () {
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                if (touch.position.x > (Screen.width / 2) && Time.time > nextFire) {
                    nextFire = Time.time + fireRate;
                    Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.x > (Screen.width / 2) && Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                Instantiate (bullet, bulletSpawn.position, bulletSpawn.rotation);
            }
        }
    }
}
