﻿using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
    void Update() {
        if (Input.GetKey(KeyCode.R)) {
			Application.LoadLevel("Jenga Leapmotion 3");
        }
    }
}
