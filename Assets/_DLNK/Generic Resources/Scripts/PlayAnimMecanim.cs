using UnityEngine;
using System.Collections;

public class PlayAnimMecanim: MonoBehaviour 
{
	public Animator _animator;
	public GUIText AnimText;
	public int AnimCode;
	
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
		_animator.SetInteger("AnimNum", AnimCode);
		AnimText.fontStyle = FontStyle.Bold;
	}
	public void OnUpdate ()
	{
		int tmp;
		tmp=_animator.GetInteger ("AnimNum");
		if (tmp != AnimCode) {
			AnimText.fontStyle = FontStyle.Normal;
				}
	}
}
