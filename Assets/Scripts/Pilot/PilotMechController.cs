using UnityEngine;
using System.Collections;
using CnControls;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PilotMechController : NetworkBehaviour {
    // Metch physics
    public float speed;
    private Rigidbody rb;
    private RectTransform joystickTrans;
    private float moveH;
    private float moveV;
    private float nextFire;
    public float fireRate;

    // Mech related objects
    public GameObject bullet;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;

    public Animator anim;
    private bool altShoot;

    // Mech team info
	[SyncVar]
    public GameManager.Team team;
	[SyncVar]
    public GameManager.Role role;

    // Mech camera stuff
    private Vector3 camOffset;

    // Game related info
    private int health;
    private int ammo;
    private int fuel;
    private int defBoost;
    private int speedBoost;
    private int atkBoost;
	private GlobalDataHook globalData;
    private int fuelDepleteRate;
    private float timer;
    private bool notify;

    void Awake () {
        rb = GetComponent<Rigidbody>();
		globalData = GetComponent<GlobalDataHook> ();
    }

    void Start () {
        //used for switching arms for shooting
        altShoot = false;
        fuelDepleteRate = 5;
        //set up camera
        SetCamera();
        //set up animations
        anim = transform.GetComponent<Animator>();
        //set up network animations
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);

        health = globalData.getParam(team, GlobalDataController.Param.Health);
        fuel = globalData.getParam(team, GlobalDataController.Param.Fuel); 
    }

	void Update () {
        if (!isLocalPlayer)
            return;

        moveH = CnInputManager.GetAxis("Horizontal");
        moveV = CnInputManager.GetAxis("Vertical");

        timer += Time.deltaTime;
		if (health > 0 && fuel > 0) {
            Move();
            Turn();
            Fire();
            anim.SetBool("fuel", true);
        } else if (health <= 0) {
            anim.SetBool("death", true);
        } else if (fuel <= 0) {
            rb.isKinematic = true;
            anim.SetBool("fuel", false);
        }
    }

    void FixedUpdate () {
        if (!isLocalPlayer)
            return; 

        health = globalData.getParam(team, GlobalDataController.Param.Health);
        fuel = globalData.getParam (team, GlobalDataController.Param.Fuel); 
    }

    void LateUpdate() {
        if (!notify) { 
		    if(isLocalPlayer) CmdNotifyNewPlayer (team, role);
            notify = true;
        }
        if (!isLocalPlayer)
            return; 

        if (this.role == GameManager.Role.Pilot) { 
            /*GameObject[] engineers = GameObject.FindGameObjectsWithTag("Engineer");
            foreach (GameObject engineer in engineers) {
                engineer.SetActive(false);
            }*/
            GameObject engCam = GameObject.Find("Engineer Camera");
            if (engCam != null) engCam.SetActive(false);
        }    

        if (this.role == GameManager.Role.Engineer) { 
            /*GameObject[] pilots = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject pilot in pilots) {
                pilot.SetActive(false);
            }*/

            GameObject cCanvas = GameObject.Find("ControllerCanvas");
            GameObject pilotCamera = GameObject.Find("CameraRig");
            GameObject pilotMap = GameObject.Find("Map");

            if (cCanvas != null)
            cCanvas.SetActive(false);
            if (pilotCamera != null)
            pilotCamera.SetActive(false);
            if (pilotMap != null)
            pilotMap.SetActive(false);

            GameObject[] engineers = GameObject.FindGameObjectsWithTag("Engineer");
            foreach (GameObject engineer in engineers) {
                var engScript = engineer.GetComponent<MechBehaviour>();
                if (engScript.team != this.team) {
                    engineer.GetComponent<Camera>().gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetCamera () {
        if (isLocalPlayer) {
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
        if (moveH != 0 || moveV != 0 && timer > fuelDepleteRate) {
            //rb.isKinematic = false;
            globalData.setParam (team, GlobalDataController.Param.Fuel, fuel-1); 
        }
        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        anim.SetFloat("inputH", moveH);
        anim.SetFloat("inputV", moveV);
        rb.velocity = movement * speed;
    }

    private void Turn () {
        float angle = Mathf.Atan2(moveH, moveV) * Mathf.Rad2Deg;
        if (angle != 0) {
            rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    [Command]
	void CmdFire (GameManager.Team shooter) {
        ammo = globalData.getParam (team, GlobalDataController.Param.Ammo); 
        Debug.Log(ammo);
        if (ammo != 0) {
            nextFire = Time.time + fireRate;
            GameObject b;
            if (altShoot) {
                b = (GameObject)Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                altShoot = false;
            } else {
                b = (GameObject)Instantiate(bullet, bulletSpawn2.position, bulletSpawn2.rotation);
                altShoot = true;
            }
            //transform.Translate(new Vector3(0, 0, -0.05f));
            b.GetComponent<Rigidbody>().velocity = transform.forward * speed;
			RpcClienMessage (GameManager.teamString(shooter) + " shot a bullet");
			b.GetComponent<BulletBehaviour> ().shooter = shooter;
            NetworkServer.Spawn(b);
            Destroy(b, 2.0f);
            ammo--;
            globalData.setParam (team, GlobalDataController.Param.Ammo, ammo); 
        }
    }

	[ClientRpc]
	public void RpcClienMessage(string message){
		Debug.Log (message);
	}

    void Fire () {
        // this is for touch
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                if (touch.position.x > (Screen.width / 2) && Time.time > nextFire) {
					CmdFire(this.team);
                }
            }
        }

        // this is for mouse click
        if (Input.GetMouseButtonDown(0)) {
            if (Input.mousePosition.x > (Screen.width / 2) && Time.time > nextFire) {
				CmdFire(this.team);
            }
        }
    }

	[Command]
	public void CmdNotifyNewPlayer(GameManager.Team team, GameManager.Role role){
		RpcNotifyNewPlayer (team, role);
	}

	[ClientRpc]
	public void RpcNotifyNewPlayer(GameManager.Team team, GameManager.Role role){
		Debug.LogError (GameManager.teamString(team) + " " + GameManager.roleString(role) + " joined the game");
	}

	public void setTeamInfo(GameManager.Team team, GameManager.Role role){		
		Debug.Log ("Setting mech team to " + GameManager.teamString(team));
		this.team = team;
		this.role = role;
	}
		
}
