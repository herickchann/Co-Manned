using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

	public Texture ColorTexture;
	public Material ColorMaterial;
	public bool SetLightOn=true;
	public Light CharLight;
	public Texture TextureIlum;
	public Texture TextureNoilum;
	public Color LightColor=Color.white;
	
	// Use this for initialization
	void OnMouseUp () {

		ColorMaterial.mainTexture=ColorTexture;
		CharLight.GetComponent<Light>().enabled=true;
		ColorMaterial.SetTexture("_Illum",TextureIlum);
		CharLight.color = LightColor;

		if (!SetLightOn) { 
			CharLight.GetComponent<Light>().enabled=false;
			ColorMaterial.SetTexture("_Illum",TextureNoilum);
		}
	
	}
	
	// Update is called once per frame
//void Update () {
	
//	}

	//DLNK ASSETS
}
