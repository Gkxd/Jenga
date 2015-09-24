using UnityEngine;
using System.Collections;

public class PressMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void startPressed(){
		Application.LoadLevel("Test Scene");
	}

	public void instructionsPressed(){
		Application.LoadLevel("Instructions");
	}

	public void OKPressed(){
		Application.LoadLevel("Intro");
	}
}
