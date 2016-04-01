using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// this is the shared global object that contains the game information
public class GlobalDataController : NetworkBehaviour {

	// health information
	public const int maxHealth = 100;
	public const int maxFuel = 1000;
	public const int maxAmmo = 25;

	// Health
	[SyncVar(hook="OnBlueHealthChange")]
	private int blueHealth = maxHealth;
	[SyncVar(hook="OnRedHealthChange")]
	private int redHealth = maxHealth;

	// Ammo
	[SyncVar(hook="OnBlueAmmoChange")]
	private int blueAmmo = maxAmmo;
	[SyncVar(hook="OnRedAmmoChange")]
	private int redAmmo = maxAmmo;

	// Powerup type
	[SyncVar(hook="OnBluePowerupChange")]
	private int bluePowerupType;
	[SyncVar(hook="OnRedPowerupChange")]
	private int redPowerupType;

	// Fuel
	[SyncVar(hook="OnBlueFuelChange")]
	private int blueFuel = maxFuel;
	[SyncVar(hook="OnRedFuelChange")]
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

	// counters for the results screen (updated by server only)
	// fetched at the end of the match
	private int blueTotalDmgTaken = 0;
	private int redTotalDmgTaken = 0;
	private int blueTotalShotsFired = 0;
	private int redTotalShotsFired = 0;
	private int blueTotalTimesHit = 0;
	private int redTotalTimesHit= 0;
	private int blueTotalPickups = 0;
	private int redTotalPickups = 0;
	private int blueTotalFuelBurned = 0;
	private int redTotalFuelBurned = 0;

	// update hooks (would be nice to abstract these)
	private void OnBlueHealthChange(int amount) {
		if (blueHealth > amount) {
			blueTotalDmgTaken += blueHealth - amount;
			blueTotalTimesHit += 1;
		}
		blueHealth = amount;
	}
	private void OnRedHealthChange(int amount) {
		if (redHealth > amount) {
			redTotalDmgTaken += redHealth - amount;
			redTotalTimesHit += 1;
		}
		redHealth = amount;
	}
	private void OnBlueAmmoChange(int amount) {
		if (blueAmmo > amount) {
			blueTotalShotsFired += blueAmmo - amount;
		}
		blueAmmo = amount;
	}
	private void OnRedAmmoChange(int amount) {
		if (redAmmo > amount) {
			redTotalShotsFired += redAmmo - amount;
		}
		redAmmo = amount;
	}
	// 0 means empty - else a type is being conveyed
	private void OnBluePowerupChange(int type) {
		if (type != 0) {
			blueTotalPickups += 1;
		}
		bluePowerupType = type;
	}
	private void OnRedPowerupChange(int type) {
		if (type != 0) {
			redTotalPickups += 1;
		}
		redPowerupType = type;
	}
	private void OnBlueFuelChange(int amount) {
		if (blueFuel > amount) {
			blueTotalFuelBurned += blueFuel - amount;
		}
		blueFuel = amount;
	}
	private void OnRedFuelChange(int amount) {
		if (redFuel > amount) {
			redTotalFuelBurned += redFuel - amount;
		}
		redFuel = amount;
	}

	// list of params to be synched between engineers and pilots
	public enum Param{Health, Ammo, PowerupType, Fuel,DefBoost,SpdBoost,AtkBoost};

	static GlobalDataController s_instance;

	void Awake(){
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this.gameObject);
		}
	}
		
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

	public Dictionary<string, Dictionary<string, int>> getResultsDict() {
		Dictionary<string, Dictionary<string, int>> resultsDict = new Dictionary<string, Dictionary<string, int>>();

		Dictionary<string, int> redDict = new Dictionary<string, int>();
		redDict.Add("endHealth", redHealth);
		redDict.Add("dmgTaken", redTotalDmgTaken);
		redDict.Add("timesHit", redTotalTimesHit);
		redDict.Add("shotsFired", redTotalShotsFired);
		redDict.Add("pickups", redTotalPickups);
		redDict.Add("fuelBurned", redTotalFuelBurned);
		resultsDict.Add("red", redDict);

		Dictionary<string, int> blueDict = new Dictionary<string, int>();
		blueDict.Add("endHealth", blueHealth);
		blueDict.Add("dmgTaken", blueTotalDmgTaken);
		blueDict.Add("timesHit", blueTotalTimesHit);
		blueDict.Add("shotsFired", blueTotalShotsFired);
		blueDict.Add("pickups", blueTotalPickups);
		blueDict.Add("fuelBurned", blueTotalFuelBurned);
		resultsDict.Add("blue", blueDict);

		return resultsDict;
	}

	[ClientRpc]
	public void RpcSendMsg(string message){
		Debug.Log (message);
	}
		
}
