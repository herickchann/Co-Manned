using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;

public class GameDiscoveryScreen : MonoBehaviour {

	public Text UserName;
	public CanvasGroup GameDiscoveryCanvas;
	// popup components
	public CanvasGroup GamePopupCanvas;
	public Text GamePopupTitleText;
	public InputField GamePopupGameName;
	public InputField GamePopupGamePassword;
	// lobby listing
	public GridLayoutGroup gameListing;
	// Network Manager
	public LobbyManager lobbyManager;
	public int gameInfoExpirationMs = 5000; // milleseconds to expire a gameInfo

	private bool popupClientMode = true; // false if creating a game
	private string inputGameName = "";
	private string inputGamePass = "";

	private string selectedHostKey = "";

	// key is hostAddr:portNum
	// using a dictionary allows us to quickly lookup membership
	// although, we don't expect that many games so List works too
	private Dictionary<string, DiscoveredGameInfo> gameInfoDict;
	private Dictionary<string, Button> selectionButtonsDict;

	public Button buttonPrefab;
	public bool deleteDemoGameList = true;

	void Start () {
		UserName.text = "Username: " + GameManager.instance.userName;
		hideGameInfoPopup(); // ensure the Join Game popup is out of the way
		showGameDiscoveryScreen();
		this.gameInfoDict = new Dictionary<string, DiscoveredGameInfo>();
		this.selectionButtonsDict = new Dictionary<string, Button> ();
		if (this.deleteDemoGameList) { // clear demo listing if true
			foreach (Transform child in this.gameListing.transform) {
				Destroy(child.gameObject);
			}
		}
		lobbyManager.startDiscovery();
	}

	// Update is called once per frame
	void Update () {
		// not allowed to modify dictionary during iteration, must use key list
		List<string> gameKeys = new List<string>(this.gameInfoDict.Keys);
		foreach(string gameKey in gameKeys) {
			DiscoveredGameInfo gameInfo = this.gameInfoDict[gameKey];
			if (isGameInfoExpired(gameInfo)) {
				this.gameInfoDict.Remove(gameKey); // remove outdated info records
				Button buttonToRemove = this.selectionButtonsDict[gameKey];
				Destroy(buttonToRemove.gameObject); // delete button
				this.selectionButtonsDict.Remove(gameKey); // remove button reference
				Debug.Log("Removed game key: " + gameKey);
			}
		}
	}

	public void hideGameDiscoveryScreen() {
		GameDiscoveryCanvas.alpha = 0;
		GameDiscoveryCanvas.interactable = false;
		GameDiscoveryCanvas.blocksRaycasts = false;
	}

	public void showGameDiscoveryScreen() {
		GameDiscoveryCanvas.alpha = 1;
		GameDiscoveryCanvas.interactable = true;
		GameDiscoveryCanvas.blocksRaycasts = true;
	}

	public void hideGameInfoPopup() {
		GamePopupCanvas.alpha = 0;
		GamePopupCanvas.interactable = false;
		GamePopupCanvas.blocksRaycasts = false;
	}

	public void showGameInfoPopup() {
		GamePopupCanvas.alpha = 1;
		GamePopupCanvas.interactable = true;
		GamePopupCanvas.blocksRaycasts = true;
	}

	// checks if game info has expired
	private bool isGameInfoExpired(DiscoveredGameInfo gameInfo) {
		int curTime = (int)(System.DateTime.Now.Ticks / 10000);
		return (curTime - gameInfo.timeStamp > this.gameInfoExpirationMs) ? true : false;
	}

	private string formatGameInfo(DiscoveredGameInfo gameInfo) {
		string yesOrNo = (gameInfo.passwordProtected) ? "Yes" : "No ";
		string formattedInfo = string.Format("{0,-32} | {1,-32} | {2}/{3} ",
			"  " + gameInfo.gameName, yesOrNo, gameInfo.numPlayers, gameInfo.playerLimit);
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
		//this.selectionButtonsDict[hostKey].enabled = false; // do not disable to allow double-click
		this.selectionButtonsDict[hostKey].image.color = Color.gray;
		if (this.selectedHostKey == hostKey) { // if selection clicked a second time
			joinGame();
		} else { // else update hostKey selection
			this.selectedHostKey = hostKey; 
		}
	}

	public void createGame () {
		Debug.Log("Create Game Button pressed.");
		// open game popup in host mode
		this.popupClientMode = false;
		GamePopupGameName.interactable = true;
		showGameInfoPopup();
	}

	public void joinGame() {
		Debug.Log("Join Game Button pressed.");
		if (this.selectedHostKey != "") { // do nothing if no game is selected
			// check that selection is still valid
			if (this.gameInfoDict.ContainsKey(this.selectedHostKey)) { // only act on valid keys
				DiscoveredGameInfo myGameInfo = this.gameInfoDict[this.selectedHostKey];
				this.popupClientMode = true; // open game popup in client mode
				if (myGameInfo.passwordProtected) { // ask user for password if required
					GamePopupGameName.text = myGameInfo.gameName;
					GamePopupGameName.interactable = false; // cannot change selected game name
					showGameInfoPopup();
					// wait for password input and carry on at OK button
				} else { // else join directly
					popupOkButtonPressed();
				}
			} else { // pretend nothing happened if invalid
				this.selectedHostKey = ""; // erase invalid key
			}
		}
	}

	// semi-colons not allowed b/c we use those as delimiters
	public void setGameName (string input) { this.inputGameName = input.Replace(":", ""); }

	public void setGamePass (string input) { this.inputGamePass = input; }

	public void popupOkButtonPressed() {
		// re-using the popup for host / client so we need to differentiate between them
		if (this.popupClientMode) {
			DiscoveredGameInfo myGameInfo = this.gameInfoDict[this.selectedHostKey];
			Debug.Assert(myGameInfo.gameName != "");
			lobbyManager.setGameNameAndPass(myGameInfo.gameName, this.inputGamePass); // commit data to networkManager
			GameManager.instance.gameName = myGameInfo.gameName;
			// start as client
			Debug.Log("UI is invoking client startup");
			lobbyManager.setConnectionInfo(myGameInfo.hostAddress, myGameInfo.hostPort);
			lobbyManager.Join();
			hideGameInfoPopup();
			hideGameDiscoveryScreen();
		} else {
			if (this.inputGameName == "") { // gen random gameName if none is given
				Random.seed = (int)System.DateTime.Now.Ticks;
				int randNum = Random.Range(0, 9999);
				this.inputGameName = "game" + randNum.ToString("0000");
			}
			lobbyManager.setGameNameAndPass(this.inputGameName, this.inputGamePass); // commit data to networkManager
			GameManager.instance.gameName = this.inputGameName;
			// start as server
			Debug.Log("UI is invoking server startup");
			// commit game information to the network manager
			lobbyManager.Host();
			hideGameInfoPopup();
			hideGameDiscoveryScreen();
		}
	}

	public void popupCancelButtonPressed() {
		hideGameInfoPopup();
		GamePopupGameName.textComponent.text = "";
		GamePopupGamePassword.textComponent.text = "";
	}
}
