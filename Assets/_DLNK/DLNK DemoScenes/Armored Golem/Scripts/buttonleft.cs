using UnityEngine;
using System.Collections;

public class buttonleft : MonoBehaviour 
{
	public Animator _animator;
	public GUIText AnimText;

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void OnMouseUp()
	{
		int tmp;
		tmp = _animator.GetInteger("AnimNum");
		tmp--;

		if (tmp < 1)
		{
			tmp = 11;
		}

		_animator.SetInteger("AnimNum", tmp);

		
		if (tmp == 1){
			AnimText.text = "Stand Idle";
		}
		if (tmp == 2){
			AnimText.text = "Stand Lookaround";
		}
		if (tmp == 3){
			AnimText.text = "Get Damage 1";
		}
		if (tmp == 4){
			AnimText.text = "Get Damage 2";
		}
		if (tmp == 5){
			AnimText.text = "Get Damage 3";
		}
		if (tmp == 6){
			AnimText.text = "Combat Attack 1";
		}
		if (tmp == 7){
			AnimText.text = "Combat Attack 2";
		}
		if (tmp == 8){
			AnimText.text = "Combat Attack 3";
		}
		if (tmp == 9){
			AnimText.text = "Walk in Position";
		}
		if (tmp == 10){
			AnimText.text = "Simple Jump";
		}
		if (tmp == 11){
			AnimText.text = "Fall Down / Die";
		}

		//DLNK ASSETS
	}
}
