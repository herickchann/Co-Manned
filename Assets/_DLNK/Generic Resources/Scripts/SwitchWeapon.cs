using UnityEngine;
using System.Collections;

public class SwitchWeapon : MonoBehaviour {

	public GameObject weapon1;
	public GameObject weapon2;
	public GUIElement weaponButton;
	public Texture button1;
	public Texture button2;

	// Use this for initialization
	void OnStart () {
		weapon1.SetActive(true);
		weapon2.SetActive(false);
		weaponButton.GetComponent<GUITexture>().texture=button1;
		}

	void OnMouseUp () {
	
		if (weapon1.activeInHierarchy) {
		weapon2.SetActive(true);
		weapon1.SetActive(false);
		weaponButton.GetComponent<GUITexture>().texture = button2;
				}
		else{
		weapon1.SetActive(true);
		weapon2.SetActive(false);
		weaponButton.GetComponent<GUITexture>().texture = button1;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
