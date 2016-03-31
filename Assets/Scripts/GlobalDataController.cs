using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// this is the shared global object that contains the game information
public class GlobalDataController : NetworkBehaviour {

	// health information
	public const int maxHealth = 100;
	public const int maxFuel = 100;
	public const int maxAmmo = 25;

	// Health
	[SyncVar]
	private int blueHealth = maxHealth;
	[SyncVar]
	private int redHealth = maxHealth;

	// Ammo
	[SyncVar]
	private int blueAmmo = maxAmmo;
	[SyncVar]
	private int redAmmo = maxAmmo;

	// Powerup type
	[SyncVar]
	private int bluePowerupType;
	[SyncVar]
	private int redPowerupType;

	// Fuel
	[SyncVar]
	private int blueFuel = maxFuel;
	[SyncVar]
	private int redFuel = maxFuel;

	[SyncVar]
	private int blueDefBoost;
	[SyncVar]
	private int redDefBoost;

	[SyncVar]
	private int blueSpdBoost;
	[SyncVar]
	private int redSpdBoost;

	[SyncVar]
	private int blueAtkBoost;
	[SyncVar]
	private int redAtkBoost;


	// list of params to be synched between engineers and pilots
	public enum Param{Health, Ammo, PowerupType, Fuel,DefBoost,SpdBoost,AtkBoost};


	void Start(){
		DontDestroyOnLoad (this);
		Debug.Log ("global data started");
		GameObject.Find ("LobbyManager").GetComponent<LobbyManager> ().globalDataId = this.netId;
		Debug.Log ("let lobby manager know my id as " + this.netId.Value);

	}

	void OnEnable(){
		Debug.Log ("global data enabled");
	}

	void OnDisable(){
		Debug.Log ("global data disabled");
	}

	// helper functions
	public string paramString(GlobalDataController.Param param){
		switch(param){
		case Param.Ammo:
			return "ammo";
		case Param.Fuel:
			return "fuel";
		case Param.Health:
			return "health";
		case Param.PowerupType:
			return "powerup type";
		case Param.DefBoost:
			return "defence boost";
		case Param.SpdBoost:
			return "speed boost";
		case Param.AtkBoost:
			return "attack boost";
		default:
			return "default";
		}
	}

	// returning specific param based on team
	public int getParam(GameManager.Team team, GlobalDataController.Param param){
		switch (param) {
		case Param.Ammo:
			switch (team) {
			case GameManager.Team.Blue:
				return blueAmmo;
			case GameManager.Team.Red:
				return redAmmo;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.Fuel:
			switch (team) {
			case GameManager.Team.Blue:
				return blueFuel;
			case GameManager.Team.Red:
				return redFuel;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.Health:
			switch (team) {
			case GameManager.Team.Blue:
				return blueHealth;
			case GameManager.Team.Red:
				return redHealth;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.PowerupType:
			switch (team) {
			case GameManager.Team.Blue:
				return bluePowerupType;
			case GameManager.Team.Red:
				return redPowerupType;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.DefBoost:
			switch (team) {
			case GameManager.Team.Blue:
				return blueDefBoost;
			case GameManager.Team.Red:
				return redDefBoost;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.AtkBoost:
			switch (team)
			{
			case GameManager.Team.Blue:
				return blueAtkBoost;
			case GameManager.Team.Red:
				return redAtkBoost;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		case Param.SpdBoost:
			switch (team)
			{
			case GameManager.Team.Blue:
				return blueSpdBoost;
			case GameManager.Team.Red:
				return redSpdBoost;
			case GameManager.Team.None:
				return 0;
			default:
				return 0;
			}
		default:
			return 0;
		}


	}


	[Server]
	public void setParam(GameManager.Team team, GlobalDataController.Param param, int amount){
		int newValue = 0;

		switch (param) {
		case Param.Ammo:
			switch (team) {
			case GameManager.Team.Blue:
				blueAmmo = amount;
				newValue = blueAmmo;
				break;
			case GameManager.Team.Red:
				redAmmo = amount;
				newValue = redAmmo;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.Fuel:
			switch (team) {
			case GameManager.Team.Blue:
				blueFuel = amount;
				newValue = blueFuel;
				break;
			case GameManager.Team.Red:
				redFuel = amount;
				newValue = redFuel;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.Health:
			switch (team) {
			case GameManager.Team.Blue:
				blueHealth = amount;
				newValue = blueHealth;
				break;
			case GameManager.Team.Red:
				redHealth = amount;
				newValue = redHealth;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.PowerupType:
			switch (team) {
			case GameManager.Team.Blue:
				bluePowerupType = amount;
				newValue = bluePowerupType;
				break;
			case GameManager.Team.Red:
				redPowerupType = amount;
				newValue = redPowerupType;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.DefBoost:
			switch (team)
			{
			case GameManager.Team.Blue:
				blueDefBoost = amount;
				newValue = blueDefBoost;
				break;
			case GameManager.Team.Red:
				redDefBoost = amount;
				newValue = redDefBoost;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.SpdBoost:
			switch (team)
			{
			case GameManager.Team.Blue:
				blueSpdBoost = amount;
				newValue = blueSpdBoost;
				break;
			case GameManager.Team.Red:
				redSpdBoost = amount;
				newValue = redSpdBoost;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		case Param.AtkBoost:
			switch (team)
			{
			case GameManager.Team.Blue:
				blueAtkBoost = amount;
				newValue = blueAtkBoost;
				break;
			case GameManager.Team.Red:
				redAtkBoost = amount;
				newValue = redAtkBoost;
				break;
			case GameManager.Team.None:
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}

		RpcSendMsg (GameManager.teamString (team) + " updating para: "+ paramString(param)+" to: " + newValue.ToString ());
	}



	[ClientRpc]
	public void RpcSendMsg(string message){
		Debug.Log (message);
	}
		
}
