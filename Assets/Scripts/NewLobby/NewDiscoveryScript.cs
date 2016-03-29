using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NewDiscoveryScript : NetworkDiscovery {

	List<string> addresses = new List<string>();

	void Awake(){
		showGUI = false;
	}
	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
		string info = fromAddress + ":" + data;
		addresses.Add (info);
		Debug.Log ("broadcast received string: " + info);
	}

	// return most recent room
	public string getRoom(){
		return addresses[addresses.Count-1];
	}
		
}
