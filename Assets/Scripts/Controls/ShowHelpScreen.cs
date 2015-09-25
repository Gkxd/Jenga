using UnityEngine;
using System.Collections;

public class ShowHelpScreen : MonoBehaviour {

    [Header("Reference Settings")]
    public Canvas helpScreenCanvas;
	
	void Update () {
        helpScreenCanvas.enabled = Input.GetKey(KeyCode.H);
	}
}
