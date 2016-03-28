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
    private Vector3 statusTextOffset;
    private float nextFire;
    public float fireRate;

    // Mech related objects
    public GameObject bullet;
    public GameObject statusText;
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
    [SyncVar]
    public int ammo;
    [SyncVar]
    public int powerupType;
    [SyncVar]
    public int fuel;
    private Combat combat;
    [SyncVar]
    private Light mechLight;

	private GlobalData globalData;

    void Awake () {
        rb = GetComponent<Rigidbody>();
		globalData = GetComponent<GlobalData> ();
    }

    void Start () {
        if (this.role == GameManager.Role.Engineer) {
            Camera.main.gameObject.SetActive(false);
        }
        else {
            GameObject.FindGameObjectWithTag("Engineer").SetActive(false);
        }

        // set up physics
        statusTextOffset = transform.position - statusText.transform.position;
        powerupType = 0;
        // set up manager to pull data from game room

        // test set up string
        // set up name

		int blue = globalData.getHealth (GameManager.Team.Blue); 
		int red = globalData.getHealth (GameManager.Team.Red); 
		statusText.GetComponent<TextMesh> ().text = "B:" + blue.ToString () + "R:" + red.ToString ();

        //used for switching arms for shooting
        altShoot = false;
        combat = transform.GetComponent<Combat>();
        //set up camera
        SetCamera();
        //set up animations
        anim = transform.GetComponent<Animator>();
        //set up network animations
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        //get mech light
        mechLight = GetComponentInChildren<Light>();

		if(isClient) CmdNotifyNewPlayer (team, role);

    }

	public void updateStatusText(){
		var globalData = GetComponent<GlobalData>();
		int blue = globalData.getHealth (GameManager.Team.Blue); 
		int red = globalData.getHealth (GameManager.Team.Red); 
		statusText.GetComponent<TextMesh> ().text = "B:" + blue.ToString () + "R:" + red.ToString ();
	}

	void Update () {
        if (!isLocalPlayer)
            return;

        rb.isKinematic = true;
        statusText.transform.position = transform.position + statusTextOffset;

        moveH = CnInputManager.GetAxis("Horizontal");
        moveV = CnInputManager.GetAxis("Vertical");

		if (globalData.getHealth(this.team) > 0) {
            Move();
            Turn();
            Fire();
        } else {
            anim.SetBool("death", true);
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
        if (moveH != 0 || moveV != 0) {
            rb.isKinematic = false;
            //fuel--;
        }

        if (fuel > 0) {
            Vector3 movement = new Vector3(moveH, 0.0f, moveV);
            anim.SetFloat("inputH", moveH);
            anim.SetFloat("inputV", moveV);
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
	void CmdFire (GameManager.Team shooter) {
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
		Debug.Log (GameManager.teamString(team) + " " + GameManager.roleString(role) + " joined the game");
	}

	public void setTeamInfo(GameManager.Team team, GameManager.Role role){		
		Debug.Log ("Setting mech team to " + GameManager.teamString(team));
		this.team = team;
		this.role = role;
	}
		
}
