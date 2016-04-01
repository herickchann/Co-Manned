using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class MechBehaviour : NetworkBehaviour
{
	public TimingMiniGameBehaviour tmg;
	private const int noBoosts = 3; 
	public Text[] boostText= new Text[noBoosts];
	public Button[] boostButtons = new Button[noBoosts];
	private Color[] BaseColors = new Color[noBoosts];
	public AudioSource restorationSounds;
	public AudioClip[] soundEffects = new AudioClip[4];
	public Text healthBayText;
	public Text ammoBayText;
	public Text fuelBayText;
	public GameObject timingMiniGame;
	public GameObject restartMiniGame;
	Object energyCell;
	GameObject ammoIcon;
	const int holdingBaySize = 10;
	int[] holdingBayArray = new int[holdingBaySize];
	Transform holdingBay;
	Transform StatusPanel;
	Transform ammoPanel;
	Vector3 energyCellScale;
	Image healthBar;
	Image fuelBar;
	Text healthText;
	Text fuelText;
	//[SyncVar (hook="test")]
	int fuel;
	//[SyncVar(hook = "test")]
	int health;
	//const int maxHealth = 100;
	//const int maxFuel = 100;
	Vector3 baseCellPosition;
	//[SyncVar]
	int ammoCount;
	//const int maxAmmoCount = 25;
	GameObject[] ammoIcons = new GameObject[GlobalDataController.maxAmmo];
	int lastAmmoCount;
	int[] loaded = new int[3];
	int[] boostLoaded = new int[3];
	float[] boostTime = new float[3];
	float[] boostEndTime = new float[3];
    bool[] boostUp = new bool[3];
	public Material[] mat;
	bool restart;
    float updateTime;
    const float updateInterval = (float)0.2;
    public Sprite BlueBackground;
    bool isInitialized;
    public Text TeamText;

	// Wire up game manager
	GameObject gameManager;
	// public GameObject serverData;
    [SyncVar]
	public GameManager.Team team;
    [SyncVar]
	public GameManager.Role role;

	private GlobalDataHook globalData;
    private bool notify;

	void Awake()
	{
		globalData = GetComponent<GlobalDataHook>();
		gameManager = GameObject.Find("GameManager");
	}

	void Start()
	{
		ammoIcon = (GameObject)Resources.Load("AmmoIcon");
		energyCell = Resources.Load("EnergyCell");
		holdingBay = GetComponent<Transform>().Find("Canvas").Find("Holding Bay");
		StatusPanel = GetComponent<Transform>().Find("Canvas").Find("StatusBarPanel");
		ammoPanel = GetComponent<Transform>().Find("Canvas").Find("AmmoPanel");
		healthBar = StatusPanel.Find("HealthBar").gameObject.GetComponent<Image>();
		fuelBar = StatusPanel.Find("FuelBar").gameObject.GetComponent<Image>();
		healthText = StatusPanel.Find("HealthText").gameObject.GetComponent<Text>();
		fuelText = StatusPanel.Find("FuelText").gameObject.GetComponent<Text>();
		energyCellScale = GetComponent<Transform>().Find("Canvas").Find("FuelConverterBackground").Find("FuelConverter").localScale;
		baseCellPosition = holdingBay.position;
		baseCellPosition.x -= (float)5;
		baseCellPosition.y -= (float)-1;
		/*for (int x = 0; x < holdingBaySize; x++)
		{
			CreateEnergyCell(0);
		}*/
        /*for (int x = 0; x < 4; x++)
        {
            CreateEnergyCell(x);
        }*/
        ///health = globalData.getParam("");
        health = GlobalDataController.maxHealth;//maxHealth * 3 / 5;
		fuel = GlobalDataController.maxFuel;//maxFuel * 3 / 5;
		ammoCount = GlobalDataController.maxAmmo;//maxAmmoCount-1;
        /*for (int x = 0; x < ammoCount; x++)
        {
            AddAmmoIcon(x);
        }
        lastAmmoCount = ammoCount;*/
        lastAmmoCount = 0;

		for (int x = 0; x < noBoosts; x++)
		{
			BaseColors[x] = boostButtons[x].colors.normalColor;
		}
		restartMiniGame.gameObject.SetActive(false);
		restart = false;
        updateTime = Time.time;
		//        SetCamera();
	}

	void SetCamera()
	{
		//Camera.main.gameObject.SetActive(false);
		if (isLocalPlayer)
		{
			int noCameras = Camera.allCamerasCount;
			Camera[] allCameras = new Camera[noCameras];
			Camera.GetAllCameras(allCameras);
			for (int x = 0; x < noCameras; x++)
			{
				Camera c = allCameras[x];
				if (x != noCameras - 1)
				{

					allCameras[x].gameObject.SetActive(false);
				}
				else
				{
					allCameras[x].gameObject.SetActive(true);
					GetComponent<Transform>().Find("Canvas").gameObject.GetComponent<Canvas>().worldCamera = allCameras[x];
				}
			}
		}
	}

    [Command]
	public void CmdNotifyNewPlayer(GameManager.Team team, GameManager.Role role){
		RpcNotifyNewPlayer (team, role);
	}

	[ClientRpc]
	public void RpcNotifyNewPlayer(GameManager.Team team, GameManager.Role role){
		Debug.LogError (GameManager.teamString(team) + " " + GameManager.roleString(role) + " joined the game");
	}

	// Update is called once per frame
	void Update()
	{
        if (updateTime < Time.time)
        {
            if (!isLocalPlayer)
                return;

            if (!isInitialized)
            {
                for (int x = 0; x < 4; x++)
                {
                    CreateEnergyCell(x);
                }
                isInitialized = true;
            }

            health = globalData.getParam(team, GlobalDataController.Param.Health);
            fuel = globalData.getParam(team, GlobalDataController.Param.Fuel);
            ammoCount = globalData.getParam(team, GlobalDataController.Param.Ammo);
            healthText.text = "Health: " + health + "/" + GlobalDataController.maxHealth;
            fuelText.text = "Fuel: " + fuel + "/" + GlobalDataController.maxFuel ;
            healthBar.fillAmount = (float)health / GlobalDataController.maxHealth;
            fuelBar.fillAmount = (float)fuel / GlobalDataController.maxFuel;
            fuelBar.color = new Color(1, (float)fuel / GlobalDataController.maxFuel, 0);

            if (fuel == 0 && restart == false)
            {
                restart = true;
                foreach (int x in loaded)
                {
                    if (x != 0) { restart = false; }
                }
                if (restart)
                {
                    timingMiniGame.gameObject.SetActive(false);
                    restartMiniGame.gameObject.SetActive(true);
                    restartMiniGame.gameObject.GetComponent<RestartMiniGame>().Setup();
                }
            }
            else if (fuel != 0)
            {
                timingMiniGame.gameObject.SetActive(true);
                restartMiniGame.gameObject.SetActive(false);
            }

            for (int x = 0; x < boostTime.Length; x++)
            {
                if (boostTime[x] > 0)
                {
                    boostText[x].text = "" + System.Math.Round((boostTime[x]), 2);
                    if (boostUp[x])
                    {
                        if (boostEndTime[x] > Time.time)
                        {
                            boostTime[x] = boostEndTime[x] - Time.time;
                        }
                        else
                        {
                            toggleBoost(x);
                        }
                    }
                }
                else
                {
                    boostText[x].text = "0";
                }
            }

            if (((double)health / GlobalDataController.maxHealth) >= 0.50)
            {
                healthBar.color = new Color(1 - ((float)health / GlobalDataController.maxHealth) / (float)2.5, 1, 0);
            }
            else
            {
                healthBar.color = new Color(1, (float)health / GlobalDataController.maxHealth, 0);
            }

            //Updates the ammo display
            if (ammoCount > lastAmmoCount)
            {
                for (int x = lastAmmoCount; x < ammoCount; x++)
                {
                    AddAmmoIcon(x);
                }
            }
            else if (ammoCount < lastAmmoCount)
            {
                for (int x = lastAmmoCount; x > ammoCount; x--)
                {
                    DeleteAmmoIcon(x - 1);
                }
            }
            lastAmmoCount = ammoCount;

            for (int x = 0; x < loaded.Length; x++)
            {
                if (loaded[x] > 0)
                {
                    //tmg.StartBar(x);
                    timingMiniGame.GetComponent<TimingMiniGameBehaviour>().StartBar(x);
                }
            }
            if (boostLoaded[0] > 0)
            {
                healthBayText.color = Color.red;
            }
            else { healthBayText.color = Color.white; }
            healthBayText.text = "" + loaded[0];
            if (boostLoaded[1] > 0)
            {
                fuelBayText.color = Color.red;
            }
            else {
                fuelBayText.color = Color.white;
            }
            fuelBayText.text = "" + loaded[1];
            if (boostLoaded[2] > 0)
            {
                ammoBayText.color = Color.red;
            }
            else {
                ammoBayText.color = Color.white;
            }
            ammoBayText.text = "" + loaded[2];

            int newCellType = globalData.getParam(team, GlobalDataController.Param.PowerupType);
            if (newCellType != 0)
            {
                globalData.setParam(team, GlobalDataController.Param.PowerupType, 0);
                CreateEnergyCell(newCellType-1);
            }
            updateTime += updateInterval;
        }
	}

	GameObject CreateEnergyCell(int type)
	{
		int index = holdingBaySize;
		for (int x = 0; x < holdingBaySize; x++)
		{
			if (holdingBayArray[x] == 0)
			{
				index = x;
				break;
			}
		}
		if (index == holdingBaySize)
		{
			return null;
		}
		holdingBayArray[index] = 1;

		GameObject temp = (GameObject)Instantiate(energyCell, holdingBay.position, holdingBay.rotation);
		temp.GetComponent<Transform>().SetParent(holdingBay);
		temp.GetComponent<AmmoBehaviour>().index = index;
		temp.GetComponent<AmmoBehaviour>().mech = this;
		float w = holdingBay.localScale.x * holdingBay.gameObject.GetComponent<RectTransform>().rect.width;
		float h = holdingBay.localScale.y * holdingBay.gameObject.GetComponent<RectTransform>().rect.height;
		Vector3 pos = new Vector3((-1 * w * (float)0.5), (h * (float)0.15), 0);
		Vector3 scale = new Vector3((float)(energyCellScale.x/5.5), (float)(energyCellScale.y / 5.5), (float)(energyCellScale.z / 5.5));
		if (index < holdingBaySize / 2)
		{
			pos.x += (w / ((holdingBaySize + 2) / 2) * ((holdingBaySize / 2) - index));//2 * (index);
		}
		else
		{
			pos.x += ((w / ((holdingBaySize + 2) / 2)) * (index - (holdingBaySize / 2)+1));//2 * (index);
			pos.y -= ((h / (holdingBaySize / (holdingBaySize / 2))));//2 * (index);(float)2.2;
		}

		temp.GetComponent<Transform>().localPosition = pos;
		temp.GetComponent<Transform>().localScale = scale;
		temp.GetComponent<Transform>().localEulerAngles = new Vector3(270,90,0);
		temp.GetComponent<MeshRenderer>().material = mat[type];
        temp.GetComponent<AmmoBehaviour>().setType(type);
		return temp;
	}

	public void DeleteCell(int index)
	{
		holdingBayArray[index] = 0;
	}

	void AddAmmoIcon(int index)
	{
		float w = ammoPanel.localScale.x * ammoPanel.gameObject.GetComponent<RectTransform>().rect.width;
		float h = ammoPanel.localScale.y * ammoPanel.gameObject.GetComponent<RectTransform>().rect.height;
		//Vector3 pos = ammoPanel.position;
		//Vector3 pos = new Vector3((float)(index*0.5 + 1.2), 0,0);
		Vector3 pos = new Vector3(/*(float)(w- (w/(maxAmmoCount-(maxAmmoCount-index))))*/w * (float)0.3, (float)-0.35*h, 0);
		//pos.x -= (float)0.75 * (w / (maxAmmoCount) * ((maxAmmoCount) - index - 1));
		pos.x -= (float)0.75 * (w / (5) * ((5) - (index)%5 -1));
		pos.y += (float) (w / (5) * ((5) - (index) / 5 - 1));
		//pos.x += (float)index;
		ammoIcons[index] = (GameObject)Instantiate(ammoIcon, ammoPanel.position, ammoPanel.rotation);
		ammoIcons[index].GetComponent<Transform>().parent = null;
		//ammoIcons[index].GetComponent<Transform>().parent = ammoPanel;
		ammoIcons[index].GetComponent<Transform>().SetParent(ammoPanel);
		ammoIcons[index].GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
		ammoIcons[index].GetComponent<Transform>().localPosition = pos;
	}

    void LateUpdate() {
        if (!notify) { 
		    if(isLocalPlayer) CmdNotifyNewPlayer (team, role);
            notify = true;
        }
        if (!isLocalPlayer)
            return;

        if (this.role == GameManager.Role.Pilot) {
            GameObject[] engineers = GameObject.FindGameObjectsWithTag("Engineer");
            foreach (GameObject engineer in engineers) {
                engineer.SetActive(false);
            }
        }

        if (this.role == GameManager.Role.Engineer) { 
            GameObject[] pilots = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject pilot in pilots) {
                pilot.SetActive(false);
            }

            GameObject cCanvas = GameObject.Find("ControllerCanvas");
            GameObject pilotCamera = GameObject.Find("CameraRig");
            GameObject pilotMap = GameObject.Find("Map");

            if (cCanvas != null) cCanvas.SetActive(false);
            if (pilotCamera != null) pilotCamera.SetActive(false);
            if (pilotMap != null) pilotMap.SetActive(false);

            GameObject[] engineers = GameObject.FindGameObjectsWithTag("Engineer");
            foreach (GameObject engineer in engineers) {
                var engScript = engineer.GetComponent<MechBehaviour>();
                if (engScript.team != this.team) {
                    engineer.SetActive(false);
                }
            }
        }
    }

	void DeleteAmmoIcon(int index)
	{
		Destroy(ammoIcons[index]);
	}

	public bool AmmoFull()
	{
		return ammoCount == GlobalDataController.maxAmmo;
	}

	public void AddAmmo(int val)
	{

		ammoCount += val;
		if (ammoCount > GlobalDataController.maxAmmo)
			ammoCount = GlobalDataController.maxAmmo;

		globalData.setParam(team, GlobalDataController.Param.Ammo,ammoCount);
	}

	public bool HealthFull()
	{
		return health == GlobalDataController.maxHealth;
	}

	public void AddHealth(int val)
	{
		health += val;
		if (health > GlobalDataController.maxHealth)
			health = GlobalDataController.maxHealth;
		if (health < 0)
		{
			health = 0;
		}

		globalData.setParam(team, GlobalDataController.Param.Health, health);
	}

	public bool FuelFull()
	{
		return fuel == GlobalDataController.maxFuel;
	}

	public void AddFuel(int val)
	{
		fuel += val;
		if (fuel > GlobalDataController.maxFuel)
			fuel = GlobalDataController.maxFuel;
		if (fuel < 0)
		{
			fuel = 0;
		}
		globalData.setParam(team, GlobalDataController.Param.Fuel, fuel);
	}

	public void Load(int val, int index)
	{
		loaded[index] += 1;
		restorationSounds.PlayOneShot(soundEffects[index], 1);
		if (val == index)
		{
			boostLoaded[index]+=1;
		}
	}

	public void Convert(double healthMod, double fuelMod, double ammoMod)
	{

		if (healthMod == 2)
		{
			boostTime[0] += (float)((boostLoaded[0]+5)*ammoMod) + (float)((boostLoaded[0] + 5) * fuelMod);
		}
		if (fuelMod == 2)
		{
			boostTime[1] += (float)((boostLoaded[1] + 5) * ammoMod) + (float)((boostLoaded[1] + 5) * healthMod);
		}
		if (ammoMod == 2)
		{
			boostTime[2] += (float)((boostLoaded[2] + 5) * healthMod) + (float)((boostLoaded[2] + 5) * fuelMod);
		}
		AddHealth((int)(loaded[0] * healthMod * GlobalDataController.maxHealth / 5));
		AddFuel((int)(loaded[1] * fuelMod * GlobalDataController.maxFuel / 5));
		AddAmmo((int)(loaded[2] *ammoMod* GlobalDataController.maxAmmo / 5));
		System.Array.Clear(loaded, 0, 3);
		System.Array.Clear(boostLoaded, 0, 3);
		restorationSounds.PlayOneShot(soundEffects[3], 1);
	}

	public void reboot()
	{
		fuel = GlobalDataController.maxFuel / 3;
        restart = false;
        globalData.setParam(team, GlobalDataController.Param.Fuel, fuel);
		timingMiniGame.gameObject.SetActive(true);
		restartMiniGame.gameObject.SetActive(false);
		restorationSounds.PlayOneShot(soundEffects[3], 1);
	}

	public void PlaySound(int index)
	{
		restorationSounds.PlayOneShot(soundEffects[index], 1);
	}

	public void PlaySound(int index, float volume)
	{
		restorationSounds.PlayOneShot(soundEffects[index], volume);
	}

	public void toggleBoost(int type)
	{
		int isActive = 0;
		if (boostEndTime[type] > Time.time)
		{
			boostEndTime[type] = Time.time;
			var cb = boostButtons[type].colors;
			cb.normalColor = BaseColors[type];
			boostButtons[type].colors = cb;
		}
		else if (boostTime[type]>0)
		{
			boostEndTime[type] = Time.time+boostTime[type];
			isActive = 1;
			var cb = boostButtons[type].colors;
			cb.normalColor = Color.red;
			boostButtons[type].colors = cb;
		}
        else
        {
            return;
        }

        boostUp[type] = isActive==1;

		if (type == 0)
		{
			globalData.setParam(team, GlobalDataController.Param.DefBoost, isActive);

		}
		else if (type == 1)
		{
			globalData.setParam(team, GlobalDataController.Param.SpdBoost, isActive);
		}
		else
		{
			globalData.setParam(team, GlobalDataController.Param.AtkBoost, isActive);
		}
	}

    public void setTeamInfo(GameManager.Team team, GameManager.Role role){		
		Debug.Log ("Setting mech team to " + GameManager.teamString(team));
		this.team = team;
		this.role = role;
        if (this.team == GameManager.Team.Blue && this.role == GameManager.Role.Engineer)
        {
            GetComponent<Transform>().Find("Canvas").gameObject.GetComponent<Image>().sprite = BlueBackground;
            TeamText.text = "Blue Team";
            TeamText.color = Color.blue;
        }
        if (this.team == GameManager.Team.Red && this.role == GameManager.Role.Engineer)
        {
            TeamText.text = "Red Team";
            TeamText.color = Color.red;
        }
    }
}