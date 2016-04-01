using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResultsScreenScript : MonoBehaviour {

	public Text WinnerText;
	public GameObject LogoPanel;
	public Text RedDmgText;
	public Text BlueDmgText;
	public Text RedShotsText;
	public Text BlueShotsText;
	public Text RedAccuracyText;
	public Text BlueAccuracyText;
	public Text RedFuelBurnedText;
	public Text BlueFuelBurnedText;
	public Text RedPickupsText;
	public Text BluePickupsText;

	Color redColour = new Color(1f, 0.125f, 0f, 1f);
	Color blueColour = new Color(0f, 0.5f, 1f, 1f);
	Color greyColour = new Color(0.75f, 0.75f, 0.75f, 1f);

	// Use this for initialization
	void Start () {
		// get results from global data
		GlobalDataController globalData = GameObject.Find("GlobalData").GetComponent<GlobalDataController>();
		Debug.Assert(globalData, "GlobalDataController object not found");
		Dictionary<string, Dictionary<string, int>> resultsDict = globalData.getResultsDict();

		// pick winner
		int redHealth = resultsDict["red"]["endHealth"];
		int blueHealth = resultsDict["blue"]["endHealth"];
		string winnerString = "It's a Draw!";
		Color winnerColour = greyColour;
		if (redHealth > blueHealth) {
			winnerString = "Red Team Wins!";
			winnerColour = redColour;
		} else if (redHealth < blueHealth) {
			winnerString = "Blue Team Wins!";
			winnerColour = blueColour;
		}

		// render winner
		WinnerText.text = winnerString;
		foreach (Image img in LogoPanel.GetComponentsInChildren<Image>()) {
			img.color = winnerColour;
		}

		// update stats
		// damage dealt = enemy team damage taken
		RedDmgText.text = resultsDict["blue"]["dmgTaken"].ToString() + " HP";
		BlueDmgText.text = resultsDict["red"]["dmgTaken"].ToString() + " HP";
		int redShotsFired = resultsDict["red"]["shotsFired"];
		int blueShotsFired = resultsDict["blue"]["shotsFired"];
		RedShotsText.text = redShotsFired.ToString();
		BlueShotsText.text = blueShotsFired.ToString();
		int redHitsTaken = resultsDict["red"]["timesHit"];
		int blueHitsTaken = resultsDict["blue"]["timesHit"];
		// protect against division by zero
		float redAccuracy = 0f;
		if (redShotsFired > 0) {
			redAccuracy = (float) blueHitsTaken / (float) redShotsFired;
		}
		float blueAccuracy = 0f;
		if (blueShotsFired > 0) {
			blueAccuracy = (float) redHitsTaken / (float) blueShotsFired;
		}
		RedAccuracyText.text = string.Format("{0:0.0} %", redAccuracy * 100);
		BlueAccuracyText.text = string.Format("{0:0.0} %", blueAccuracy * 100);
		RedFuelBurnedText.text = string.Format("{0:0.00} kWh", resultsDict["red"]["fuelBurned"] / 100f);
		BlueFuelBurnedText.text = string.Format("{0:0.00} kWh", resultsDict["blue"]["fuelBurned"] / 100f);
		RedPickupsText.text = resultsDict["red"]["pickups"].ToString();
		BluePickupsText.text = resultsDict["blue"]["pickups"].ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
