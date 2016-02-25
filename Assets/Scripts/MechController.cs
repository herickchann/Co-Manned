using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MechController : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private RectTransform joystickTrans;

	public GameObject bullet;
    public GameObject joystick;
	public Transform bulletSpawn;
	private float nextFire;
	public float fireRate;

    private float moveH;
    private float moveV;
    //private bool joystickHidden = true;
    //private Vector3 joystickLocation;

    void Start () {
        rb = GetComponent<Rigidbody>();
        //joystickTrans = joystick.GetComponent<RectTransform>();
        //joystickLocation = joystickTrans.position;
    }

    void Update () {
        //joystick.SetActive(joystickHidden);
        //joystickTrans.position = joystickLocation;

        moveH = CrossPlatformInputManager.GetAxis("Horizontal");
        moveV = CrossPlatformInputManager.GetAxis("Vertical");

        Move();
        Turn();
        Fire();
        //ToggleJoystick();
    }

    private void Move () {
        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        //Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, Input.acceleration.y);
        //movement = transform.TransformDirection(movement);
        rb.velocity = movement * speed;
    }

    private void Turn () {
        float angle = Mathf.Atan2(moveH, moveV) * Mathf.Rad2Deg;
        if (angle != -180) {
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

    //private void ToggleJoystick () {
    //    foreach (Touch touch in Input.touches) {
    //        if (touch.phase == TouchPhase.Began) {
    //            if (touch.position.x < (Screen.width / 2)) {
    //                joystickTrans.position = touch.position;
    //            }
    //        } 
    //    }

    //    if (Input.GetMouseButtonDown(0)) {
    //        if (Input.mousePosition.x < (Screen.width / 2)) {
    //            joystickLocation = Input.mousePosition;
    //            joystickHidden = true;
    //        }
    //    }
    //    if (Input.GetMouseButtonUp(0)) {
    //        joystickHidden = true;
    //    }

    //    joystick.SetActive(joystickHidden);
    //    joystickTrans.position = joystickLocation;
    //}
}
