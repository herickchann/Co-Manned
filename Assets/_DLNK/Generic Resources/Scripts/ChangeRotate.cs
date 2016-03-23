using UnityEngine;
using System.Collections;

public class ChangeRotate : MonoBehaviour {

	public Rotate RotateObject;
	public float RotationAdditive;
	public bool Substract=false;

	// Use this for initialization
	void OnMouseUp () {

		if (!Substract) {
			RotateObject.RotationFactor=(RotateObject.RotationFactor)+RotationAdditive;
		}
		else
			RotateObject.RotationFactor=(RotateObject.RotationFactor)-RotationAdditive;
	}
	
	// Update is called once per frame
	//DLNK ASSETS
	void Update () {
	
	}
}
