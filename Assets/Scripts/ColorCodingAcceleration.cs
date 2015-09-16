using UnityEngine;
using System.Collections;

public class ColorCodingAcceleration : MonoBehaviour {

	// Use this for initialization
	public Rigidbody rb;
	public GameObject thisBlock;
	public bool selected =	false;
	
	//public bool selected = MouseGesture.selected;

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}




	// Update is called once per frame
	void Update () {

		if (selected == false) {
			if (rb.velocity.z != 0.0) {

				MeshRenderer blockRenderer = GetComponent<MeshRenderer> ();
				blockRenderer.material.SetColor ("_Color", new Color (1-rb.velocity.z, 1-rb.velocity.z, 1-rb.velocity.z));
		
			} else {
				changeColor ();

			}
		} else {
			MeshRenderer blockRenderer = GetComponent<MeshRenderer> ();

			blockRenderer.material.SetColor ("_Color", new Color (1,0,0));

		}
	
	}

	void changeColor(){

				
		MeshRenderer blockRenderer = GetComponent<MeshRenderer> ();
		blockRenderer.material.SetColor ("_Color", new Color(1,1,1));

	}

}
