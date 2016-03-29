using UnityEngine;
using System.Collections;

public class RoomInfoScript : MonoBehaviour {
	
	public enum Role {Host, Player, None};
	// address, port, and what the player decided to do
	public string address;
	public int port;
	public string other;
	public Role role;

	void Start(){
		DontDestroyOnLoad (this);
		address = "";
		port = 0;
		role = Role.None;
	}

	// Protocol: address:port
	public void parseAddressInfo(string info){
		string[] data = info.Split (':');

		string address = "";
		// handle different version of ips, last portion will always be the port
		for(int i=0; i<data.Length-2; ++i){
			address += data [i];
		}
		this.address = address;
		this.port = int.Parse (data [data.Length-1]);
		Debug.Log ("parsed: " + address + " " + port.ToString());
	}
}
