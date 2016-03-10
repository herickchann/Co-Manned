using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PilotCamera : MonoBehaviour {
    public Transform player;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        if (player == null) {
            offset = transform.position - new Vector3(0,0,0);
        } else {
            offset = transform.position - player.position;
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (player != null)
            transform.position = player.position + offset;
	}
}
