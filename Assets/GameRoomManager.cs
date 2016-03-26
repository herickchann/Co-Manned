using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameRoomManager : NetworkBehaviour {

	public NetworkIdentity netId;

	[SyncVar(hook="OnChange")] // user name for rendering (might not be unique)
	public string test = "";

	private void OnChange(string x) {
		test = x;
		Debug.LogError("test has been changed to " + x);
	}

	/*[ClientRpc]
	public void RpcUpdate(string x) {
		test = x;
	}*/

	/*[Command]
	public void CmdUpdate(string x) {
		//netId.AssignClientAuthority(connectionToClient);
		test = x;
		Debug.LogError("cmd");
		RpcUpdate(x);
		//netId.RemoveClientAuthority(connectionToClient);
	}*/

	public void updateTest(string x) {
		test = x;
		Debug.LogError("updateTest");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
