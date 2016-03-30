using UnityEngine;
using System.Collections;

public class RoomInfoScript : MonoBehaviour {
	
	public enum Role {Host, Player, None};
	// address, port, and what the player decided to do
	public Role role;
	public string address;
	public int port;
	public string gamename;
	public string password;

	void Start(){
		DontDestroyOnLoad (this); // persists between scenes
		role = Role.None;
		address = "";
		port = 0;
		gamename = "";
		password = "";
	}

	public void setRoomInfo(Role role, string address, int port, string gamename, string password) {
		this.role = role;
		this.address = address;
		this.port = port;
		this.gamename = gamename; // used by host
		this.password = password;
	}

	// Protocol: address:port
	/*public void parseAddressInfo(string info){
		string[] data = info.Split (':');

		string address = "";
		// handle different version of ips, last portion will always be the port
		for(int i=0; i<data.Length-1; ++i){
			if(i==data.Length-1) break;
			address += data [i];

		}
		this.address = address;
		this.port = int.Parse (data [data.Length-1]);
		Debug.Log ("parsed: " + address + " " + port.ToString());
	}*/
}
