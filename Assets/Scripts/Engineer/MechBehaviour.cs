using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class MechBehaviour : NetworkBehaviour
{
    public TimingMiniGameBehaviour tmg;
    public Text healthBayText;
    public Text ammoBayText;
    public Text fuelBayText;
    Object energyCell;
    GameObject ammoIcon;
    const int holdingBaySize = 12;
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
    const int maxHealth = 100;
    const int maxFuel = 100;
    Vector3 baseCellPosition;
    //[SyncVar]
    int ammoCount;
    const int maxAmmoCount = 25;
    GameObject[] ammoIcons = new GameObject[maxAmmoCount];
    int lastAmmoCount;
    float height;
    float width;
    int[] loaded = new int[3];
    float shieldTime = 0;
    float accelTime = 0;
    float attackTime = 0;
    public Text shieldText;
    public Text accelText;
    public Text attackText;


    // Wire up game manager
    GameObject gameManager;
    public GameObject serverData;
    public GameManager.Team team;
    public GameManager.Role role;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        serverData = GameObject.Find("ServerData");
        if (serverData == null)
        {
            Debug.Log("ServerData not found");
        }

        // init team info on load
        if (gameManager != null)
        {
            var gameData = gameManager.GetComponent<GameManager>();
            if (gameData != null)
            {
                team = gameData.getTeamSelection();
                role = gameData.getRoleSelection();
            }
        }
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
        for (int x = 0; x < holdingBaySize; x++)
        {
            CreateEnergyCell(1);
        }
        health = maxHealth * 3 / 5;
        fuel = maxFuel * 3 / 5;
        ammoCount = maxAmmoCount-1;
        for (int x = 0; x < ammoCount; x++)
        {
            AddAmmoIcon(x);
        }
        lastAmmoCount = ammoCount;
    }

    void test(int f)
    {
        fuel = f;

    }

    // Update is called once per frame
    void Update()
    {

        healthText.text = "Health: " + health + "/100";
        fuelText.text = "Fuel: " + fuel + "/100";
        healthBar.fillAmount = (float)health / maxHealth;
        fuelBar.fillAmount = (float)fuel / maxFuel;
        fuelBar.color = new Color(1, (float)fuel / maxFuel, 0);

        if (shieldTime > Time.time)
        {
            shieldText.text = ""+ System.Math.Round((shieldTime - Time.time),2);
        }
        else
        {
            shieldText.text = "";
        }

        if (accelTime > Time.time)
        {
            accelText.text = "" + System.Math.Round((accelTime - Time.time),2);
        }
        else
        {
            accelText.text = "";
        }

        if (attackTime > Time.time)
        {
            attackText.text = "" + System.Math.Round((attackTime - Time.time),2);
        }
        else
        {
            attackText.text = "";
        }

        if (((double)health / maxHealth) >= 0.50)
        {
            healthBar.color = new Color(1 - ((float)health / maxHealth) / (float)2.5, 1, 0);
        }
        else
        {
            healthBar.color = new Color(1, (float)health / maxHealth, 0);
        }
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
                DeleteAmmoIcon(x);
            }
        }
        lastAmmoCount = ammoCount;

        for (int x = 0; x < loaded.Length; x++)
        {
            if (loaded[x] > 0)
            {
                tmg.StartBar(x);
            }
        }
        healthBayText.text = "" + loaded[0];
        fuelBayText.text = "" + loaded[1];
        ammoBayText.text = "" + loaded[2];
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
        holdingBayArray[index] = type;

        GameObject temp = (GameObject)Instantiate(energyCell, holdingBay.position, holdingBay.rotation);
        temp.GetComponent<Transform>().parent = null;
        temp.GetComponent<Transform>().parent = holdingBay;
        temp.GetComponent<AmmoBehaviour>().index = index;
        temp.GetComponent<AmmoBehaviour>().mech = this;
        //Vector3 pos = baseCellPosition;
        float w = holdingBay.localScale.x * holdingBay.gameObject.GetComponent<RectTransform>().rect.width;
        float h = holdingBay.localScale.y * holdingBay.gameObject.GetComponent<RectTransform>().rect.height;
        Vector3 pos = new Vector3(-1 * w * (float)0.42, h * (float)0.25, 0);
        Vector3 scale = new Vector3(energyCellScale.x, energyCellScale.y, energyCellScale.z);
        if (index < holdingBaySize / 2)
        {
            pos.x += (w / (holdingBaySize / 2) * ((holdingBaySize / 2) - index - 1));//2 * (index);
        }
        else
        {
            pos.x += ((w / (holdingBaySize / 2)) * (index - (holdingBaySize / 2)));//2 * (index);
            pos.y -= ((h / (holdingBaySize / (holdingBaySize / 2))));//2 * (index);(float)2.2;
        }
        temp.GetComponent<Transform>().localScale = scale;
        temp.GetComponent<Transform>().localPosition = pos;

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

    void DeleteAmmoIcon(int index)
    {
        Destroy(ammoIcons[index]);
    }

    public bool AmmoFull()
    {
        return ammoCount == maxAmmoCount;
    }

    public void AddAmmo(int val)
    {
        ammoCount += val;
        if (ammoCount > maxAmmoCount)
            ammoCount = maxAmmoCount;
    }

    public bool HealthFull()
    {
        return health == maxHealth;
    }

    public void AddHealth(int val)
    {
        health += val;
        if (health > maxHealth)
            health = maxHealth;
        if (health < 0)
        {
            health = 0;
        }
    }

    public bool FuelFull()
    {
        return fuel == maxFuel;
    }

    public void AddFuel(int val)
    {
        fuel += val;
        if (fuel > maxFuel)
            fuel = maxFuel;
        if (fuel < 0)
        {
            fuel = 0;
        }
    }

    public void Load(int val, int index)
    {
        loaded[index] += val;
    }

    public void Convert(double healthMod, double fuelMod, double ammoMod)
    {
        if (healthMod == 2)
        {
            shieldTime = Time.time + (float)(2*ammoMod) + (float)(2*fuelMod);
        }
        if (fuelMod == 2)
        {
            accelTime = Time.time + (float)(2 * ammoMod) + (float)(2 * healthMod);
        }
        if (ammoMod == 2)
        {
            attackTime = Time.time + (float)(2 * healthMod) + (float)(2 * fuelMod);
        }
        AddHealth((int)(loaded[0] * healthMod * 25));
        AddFuel((int)(loaded[1] * fuelMod * 25));
        AddAmmo((int)(loaded[2] *ammoMod));
        System.Array.Clear(loaded, 0, 3);
    }

}
