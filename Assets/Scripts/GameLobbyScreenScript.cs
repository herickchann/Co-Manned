using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AssemblyCSharp;

public class GameLobbyScreenScript : MonoBehaviour {

	public Text UserName;
	// popup components
	public CanvasGroup GamePopupMaskPanel;
	public Text GamePopupTitleText;
	public InputField GamePopupGameName;
	public InputField GamePopupGamePassword;
	// lobby listing
	public GridLayoutGroup gameListing;
	// Network Manager
	public NetworkManagerScript netManager;
	public int gameInfoExpirationMs = 5000; // milleseconds to expire a gameInfo

	private bool popupClientMode = true; // false if creating a game
	private string gameName = "";
	private string gamePass = "";

	private DiscoveredGameInfo selectedGameInfo;

	// key is hostAddr:portNum
	// using a dictionary allows us to quickly lookup membership
	// although, we don't expect that many games so List works too
	private Dictionary<string, DiscoveredGameInfo> gameInfoDict;
	private Dictionary<string, Button> selectionButtonsDict;

	public Button buttonPrefab;

	void Start () {
		UserName.text = "Username: " + GameManager.instance.userName;
		// ensure the Join Game popup is out of the way
		GamePopupMaskPanel.alpha = 0;
		GamePopupMaskPanel.interactable = false;
		GamePopupMaskPanel.blocksRaycasts = false;
		this.gameInfoDict = new Dictionary<string, DiscoveredGameInfo>();
		this.selectionButtonsDict = new Dictionary<string, Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		int curTime = (int)(System.DateTime.Now.Ticks / 10000);
		// not allowed to modify dictionary during iteration, must use key list
		List<string> gameKeys = new List<string>(this.gameInfoDict.Keys);
		foreach(string gameKey in gameKeys) {
			DiscoveredGameInfo gameInfo = this.gameInfoDict[gameKey];
			if (curTime - gameInfo.timeStamp > this.gameInfoExpirationMs) {
				this.gameInfoDict.Remove(gameKey); // remove outdated info records
				Button buttonToRemove = this.selectionButtonsDict[gameKey];
				Destroy(buttonToRemove.gameObject); // delete button
				this.selectionButtonsDict.Remove(gameKey); // remove button reference
				Debug.Log("Removed game key: " + gameKey);
			}
		}
	}

	private string formatGameInfo(DiscoveredGameInfo gameInfo) {
		string yesOrNo = (gameInfo.passwordProtected) ? "Yes" : "No ";
		string formattedInfo = string.Format("{0,-32} | {1} | {2}/{3} ",
			gameInfo.gameName, yesOrNo, gameInfo.numPlayers, gameInfo.playerLimit);
		return formattedInfo;
	}

	private Button addInfoButton(string hostKey, DiscoveredGameInfo gameInfo) {
		Button gameInfoButton = Instantiate(this.buttonPrefab); // instantiate prefab
		Text btex = gameInfoButton.GetComponentInChildren<Text>();
		string formattedInfo = formatGameInfo(gameInfo);
		btex.text = formattedInfo;
		gameInfoButton.onClick.AddListener(() => selectGame(hostKey)); // attach listener to button
		gameInfoButton.transform.SetParent(this.gameListing.transform); // attach button to game listing
		gameInfoButton.GetComponent<RectTransform>().localScale = Vector3.one; // scale properly
		Debug.Log("Added button with info: " + formattedInfo);
		return gameInfoButton; // return reference for tracking
	}

	public void addGameInfo(DiscoveredGameInfo gameInfo) {
		string hostKey = gameInfo.hostAddress + ":" + gameInfo.hostPort;
		if (this.gameInfoDict.ContainsKey(hostKey)) { // update last record
			this.gameInfoDict[hostKey] = gameInfo;
			// update button
			Button buttonToUpdate = this.selectionButtonsDict[hostKey]; 
			buttonToUpdate.GetComponentInChildren<Text>().text = formatGameInfo(gameInfo);
			Debug.Log("Updated game key: " + hostKey);
		} else { // else insert
			this.gameInfoDict.Add(hostKey, gameInfo);
			// keep track of this button
			this.selectionButtonsDict.Add(hostKey, addInfoButton(hostKey, gameInfo));
			Debug.Log("Added game key: " + hostKey);
		}
	}

	public void selectGame (string hostKey) {
		// called button listing
		Debug.Log("hostKey: " + hostKey + " was selected.");
		//this.selectionButtonsDict[hostKey].enabled = false;
		this.selectionButtonsDict[hostKey].image.color = Color.gray;
	}

	public void createGame () {
		Debug.Log("Create Game Button pressed.");
		// open game popup in host mode
		this.popupClientMode = false;
		GamePopupGameName.interactable = true;
		GamePopupMaskPanel.alpha = 1;
		GamePopupMaskPanel.interactable = true;
		GamePopupMaskPanel.blocksRaycasts = true;
	}
		
	public void joinGame() {
		Debug.Log("Join Game Button pressed.");
		// open game popup in client mode
		this.popupClientMode = true;
		GamePopupGameName.textComponent.text = "Selected Game";
		GamePopupGameName.interactable = false; // cannot change selected game name
		GamePopupMaskPanel.alpha = 1;
		GamePopupMaskPanel.interactable = true;
		GamePopupMaskPanel.blocksRaycasts = true;
	}

	// semi-colons not allowed b/c we use those as delimiters
	public void setGameName (string input) { this.gameName = input.Replace(":", ""); }

	public void setGamePass (string input) { this.gamePass = input; }

	public void popupOkButtonPressed() {
		// re-using the popup for host / client so we need to differentiate between them
		// TODO: pass network information to netManager here
		if (this.popupClientMode) {
			// start as client
			Debug.Log("UI is invoking client startup");
			netManager.StartClient();
		} else {
			// start as server
			Debug.Log("UI is invoking server startup");
			// commit game information to the network manager
			netManager.startBroadcast(this.gameName, this.gamePass);
			netManager.StartHost();
		}
		//SceneManager.LoadScene("GameCreationScreen");
	}

	public void popupCancelButtonPressed() {
		GamePopupMaskPanel.alpha = 0;
		GamePopupMaskPanel.interactable = false;
		GamePopupMaskPanel.blocksRaycasts = false;
		GamePopupGameName.textComponent.text = "";
		GamePopupGamePassword.textComponent.text = "";
	}
}
