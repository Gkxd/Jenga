using UnityEngine;
using System.Collections;

public class FallSound : MonoBehaviour {
	
	public AudioClip impact;
	AudioSource audio;
	
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (GameState.state == GameState.State.PLACE_BLOCK) {
			audio.PlayOneShot(impact, 0.02F);
		}
		
	}
	
}

