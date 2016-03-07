﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	// TODO: please re-assess if these belong here
	public string userName = "";
	public string gameName = "";
	public string gamePass = "";

	public enum Team{Red, Blue, None};
	public enum Role{Pilot, Engineer, None};
	public Team teamSelection = Team.None;
	public Role roleSelection = Role.None;

	// Use this for initialization
	void Awake () {
		if (instance == null) { // instance not set?
			instance = this; // set instance to this
		} else if (instance != this) { // already exists?
			GameObject.Destroy(gameObject); // destroy this one to enforce singleton
		}
		GameObject.DontDestroyOnLoad(gameObject); // persist between scenes


	}
	// Update is called once per frame
	void Update () {
	
	}
}
