using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLobbyScreenScript : MonoBehaviour {

	public Text UserName;
	// popup components
	public CanvasGroup GamePopupMaskPanel;
	public Text GamePopupTitleText;
	public InputField GamePopupGameName;
	public InputField GamePopupGamePassword;
	// Network Manager
	public NetworkManagerScript netManager;

	private bool popupClientMode = true; // false if creating a game
	private string gameName;
	private string gamePass;

	void Start () {
		UserName.text = "Username: " + GameManager.instance.userName;
		// ensure the Join Game popup is out of the way
		GamePopupMaskPanel.alpha = 0;
		GamePopupMaskPanel.interactable = false;
		GamePopupMaskPanel.blocksRaycasts = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void selectGame () {

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

	public void setGameName (string input) {
		this.gameName = input;
	}

	public void setGamePass (string input) {
		this.gamePass = input;
	}

	public void popupOkButtonPressed() {
		// re-using the popup for host / client so we need to differentiate between them
		if (this.popupClientMode) {
			// start as client
			Debug.Log("UI is invoking client startup");
			netManager.StartClient();
		} else {
			// start as server
			Debug.Log("UI is invoking server startup");
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
