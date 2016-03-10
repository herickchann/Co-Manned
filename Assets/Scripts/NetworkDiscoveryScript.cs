using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkDiscoveryScript : NetworkDiscovery {
		
	public override void OnReceivedBroadcast(string fromAddress, string data) {
		Debug.Log("fromAddress = " + fromAddress);
		Debug.Log("data = " + data);
		base.OnReceivedBroadcast(fromAddress, data);
	}

}
