﻿using UnityEngine;
using System.Collections;
using CnControls;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PilotMechController : NetworkBehaviour
{
	// Metch physics
    public float speed;
    private Rigidbody rb;
    private RectTransform joystickTrans;
	private float moveH;
	private float moveV;
	private Vector3 statusTextOffset;
	private float nextFire;
	public float fireRate;

	// Mech related objects
	public GameObject bullet;
	public GameObject statusText;
	public Transform bulletSpawn;
	public GameObject gameManager;
	public GameObject serverData;

	// Mech team info
	public GameManager.Team team;
	public GameManager.Role role;
	    
	// Mech camera stuff
    private Vector3 camOffset;

    // Game related info
    [SyncVar]
    public int ammo;
    [SyncVar]
    public int powerupType;
    [SyncVar]
    public int fuel;
    private Vector3 lastPosition;

	void Awake(){
		rb = GetComponent<Rigidbody>();
		gameManager = GameObject.Find ("GameManager");

		// init team info on load
		if (gameManager != null) {
			var gameData = GameManager.instance;
			if (gameData != null) {
				team = gameData.getTeamSelection ();
				role = gameData.getRoleSelection ();
			}
		} else {
			Debug.Log ("game manager not found");
		}
	}


    void Start () {
		// set up physics
     
		statusTextOffset = transform.position - statusText.transform.position;
        powerupType = 0;
		// set up manager to pull data from game room

		// test set up string
		// set up name
		statusText.GetComponent<TextMesh>().text = "<" + team+ ">:" + GetComponent<Combat>().health.ToString ();


		lastPosition = rb.position;
		// set up camera
        SetCamera();
    }

	/*
	public void loadStatusText(){
		if (!isLocalPlayer)
			return;
		int health = GetComponent<Combat> ().health;
		Debug.Log ("loaded health: " + health.ToString());
		statusText.GetComponent<TextMesh> ().text = health.ToString();
	}*/

    void Update () {
		if(!isLocalPlayer)
			return;

		statusText.transform.position = transform.position + statusTextOffset;

		moveH = CnInputManager.GetAxis("Horizontal");
        moveV = CnInputManager.GetAxis("Vertical");

        Move();
        Turn();
        Fire();
    }

    private void SetCamera () {
        if(isLocalPlayer) { 
            Camera camera = Camera.main;
            if (camera != null) {
                PilotCamera followScript = camera.GetComponent("PilotCamera") as PilotCamera;
                if (followScript != null) {
                    followScript.player = transform;
                }
            }
        }
    }

    private void Move () {
        if (lastPosition != rb.position) {
            lastPosition = rb.position;
            //fuel--;
        }
        if (fuel > 0) { 
            Vector3 movement = new Vector3(moveH, 0.0f, moveV);
            rb.velocity = movement * speed;
        } else {
            fuel = 0;
        }
    }

    private void Turn () {
        float angle = Mathf.Atan2(moveH, moveV) * Mathf.Rad2Deg;
        if (angle != 0) {
            rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }
		
	[Command]
	void CmdFire(){
        if (ammo != 0) { 
		    nextFire = Time.time + fireRate;
		    var b = (GameObject)Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

		    b.GetComponent<Rigidbody> ().velocity = transform.forward * speed;
			b.GetComponent<BulletBehaviour> ().shooter = this.team;
		    NetworkServer.Spawn (b);
		    Destroy (b, 2.0f);
            ammo--;
        }
	}

    void Fire () {
		// this is for touch
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                if (touch.position.x > (Screen.width / 2) && Time.time > nextFire) {
					CmdFire ();
                }
            }
        }

		// this is for mouse click
        if (Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.x > (Screen.width / 2) && Time.time > nextFire) {
				CmdFire ();
            }
        }
    }
}
