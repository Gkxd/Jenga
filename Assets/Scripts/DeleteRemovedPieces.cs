﻿using UnityEngine;
using System.Collections;

public class DeleteRemovedPieces : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.collider.tag == "ground") {
				Destroy (gameObject);
		}
	}
}