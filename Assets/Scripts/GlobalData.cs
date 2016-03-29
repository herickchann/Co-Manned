using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


// this is where we will keep track information of all players
public class GlobalData : NetworkBehaviour {

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
		blueHealth = 70;
		redHealth = 80;
	}
		
	// helper functions
	public string paramString(GlobalData.Param param){
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
	public int getParam(GameManager.Team team, GlobalData.Param param){
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
		
	// set param to the input amount for specific team
	public void setParam(GameManager.Team team, GlobalData.Param param, int amount){
		if (!isLocalPlayer)
			return;

		CmdAllUpdateParam (team, param, amount);
	}
		

	[Command]
	public void CmdAllUpdateParam(GameManager.Team team, GlobalData.Param param, int amount){
		RpcAllUpdateParam (team, param, amount);
	}
		

	[ClientRpc]
	public void RpcAllUpdateParam(GameManager.Team team, GlobalData.Param param, int amount){
		if (!isClient)
			return;

		Debug.Log (GameManager.teamString(team) + "updating param: " + paramString(param) + "to " + amount.ToString());
		CmdUpdateParam (team, param, amount);
	}
		

	[Command]
	public void CmdUpdateParam(GameManager.Team team, GlobalData.Param param, int amount){
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

	// 0. HEALTH: get health
	public int getHealth(GameManager.Team team){
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
	}
	// HEALTH: 1. tell server we need to update all globalData instances for players
	public void setHealth(GameManager.Team team, int amount){

		// client telling server to update health on ALL clients
		if (!isLocalPlayer)
			return;

		CmdAllUpdate (team, amount);
	}

	// HEALTH: 2. server tells all clients to update their instances
	[Command]
	public void CmdAllUpdate(GameManager.Team team, int amount){
		RpcAllUpdate (team, amount);
	}

	// HEALTH: 3. client then tell the server to update its own instane
	[ClientRpc]
	public void RpcAllUpdate(GameManager.Team team, int amount){
		if (!isClient)
			return;

		Debug.Log (GameManager.teamString(team) + "got hit");
		CmdUpdateHealth (team, amount);
	}
	// HEALTH: 4. server update individual instance
	[Command]
	public void CmdUpdateHealth(GameManager.Team team, int amount){
		int newHealth = 0;
		switch (team) {
		case GameManager.Team.Blue:
			blueHealth -= amount;
			newHealth = blueHealth;
			break;
		case GameManager.Team.Red:
			redHealth -= amount;
			newHealth = redHealth;
			break;
		case GameManager.Team.None:
			break;
		default:
			break;
		}
		RpcSendMsg (GameManager.teamString (team) + " got hit and got updated to: " + newHealth.ToString ());
	}

}
