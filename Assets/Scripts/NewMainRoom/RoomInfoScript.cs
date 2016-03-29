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
		this.address = data[0];
		this.port = int.Parse (data [1]);
		Debug.Log ("parsed: " + address + " " + port.ToString());
	}
}
