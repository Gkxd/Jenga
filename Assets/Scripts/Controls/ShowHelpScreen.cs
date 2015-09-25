using UnityEngine;
using System.Collections;

public class ShowHelpScreen : MonoBehaviour {

	public GameObject helpScreen;

	// Use this for initialization
	void Start () {
		helpScreen.GetComponent<Canvas> ().enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("h")) {
			helpScreen.GetComponent<Canvas>().enabled = true;
		}
		if (Input.GetKeyUp("h")) {
			helpScreen.GetComponent<Canvas>().enabled = false;
		}
	
	}
}
