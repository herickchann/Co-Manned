using UnityEngine;
using System.Collections;

public class PlayAnimLegacy : MonoBehaviour {

	public Animation Char;
	public string CharAnim;
	// Use this for initialization
	void OnMouseUp () {

		Char.GetComponent<Animation>().Play (CharAnim);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
