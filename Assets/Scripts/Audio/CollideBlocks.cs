using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class CollideBlocks : MonoBehaviour {
	
	public BlockState blockState;
	
	public AudioClip impact;
	AudioSource audio;
	
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter (Collision col)
	{
		if (blockState.selected && col.gameObject.tag == "Block") 
		{
			
			audio.PlayOneShot(impact, 0.7F);
			
		}
	}
}