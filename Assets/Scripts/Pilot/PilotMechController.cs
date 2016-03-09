using UnityEngine;
using System.Collections;
using CnControls;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PilotMechController : NetworkBehaviour
{
    public float speed;
    private Rigidbody rb;
    private RectTransform joystickTrans;

	public GameObject bullet;
	public GameObject statusText;
	public GameObject gameManager;
	public Transform bulletSpawn;
	private Vector3 offset;
	private float nextFire;
	public float fireRate;

    private float moveH;
    private float moveV;
	private GameObject gameManagerData;

    void Start () {
        rb = GetComponent<Rigidbody>();
		offset = transform.position;
		statusText.GetComponent<TextMesh> ().text = GetComponent<Combat> ().health.ToString ();
        rb.isKinematic = true;
		gameManager = GameObject.Find ("GameManager");
		if (gameManager == null) {
			Debug.Log ("cannot find player info object");
		} else {
			string playerTeam = gameManager.GetComponent<GameManager> ().getTeamSelection ();
			Debug.Log (playerTeam);
			string playerRole = gameManager.GetComponent<GameManager> ().getRoleSelection ();
			Debug.Log (playerRole);

			if(playerRole.Equals("Engineer")){
				SceneManager.LoadScene ("engineer");
			}
		}
		
    }

    void Update () {
		if (!isLocalPlayer)
			return;
        if (rb.IsSleeping()) {
            rb.isKinematic = true;
        }

		statusText.transform.position = transform.position + offset;

		moveH = CnInputManager.GetAxis("Horizontal");
        moveV = CnInputManager.GetAxis("Vertical");

        Move();
        Turn();
        Fire();
    }

    private void Move () {
        rb.isKinematic = false;

        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        rb.velocity = movement * speed;
    }

    private void Turn () {
        float angle = Mathf.Atan2(moveH, moveV) * Mathf.Rad2Deg;
        if (angle != 0) {
            rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }
		
	[Command]
	void CmdFire(){
		nextFire = Time.time + fireRate;

		var b = (GameObject)Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

		b.GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		NetworkServer.Spawn (b);
		Destroy (b, 2.0f);
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
