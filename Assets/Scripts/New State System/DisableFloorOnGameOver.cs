using UnityEngine;
using System.Collections;

public class DisableFloorOnGameOver : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (GameState.state == GameState.State.GAME_OVER)
			gameObject.SetActive (false);
	}
}
