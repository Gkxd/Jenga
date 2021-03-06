﻿using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {
	[Header("Audio Settings")]
	public AudioClip MovingBlock;
	AudioSource audio;
	public bool PlayedAudioMovingBlock = false;

    [Header("Reference Settings")]
    public Rigidbody blockRB;
    public MeshRenderer blockMeshRenderer;
    public BoxCollider blockCollider;

    [Header("Gameplay Settings")]
    public Gradient velocityColor;
    public Gradient leapMotionHandColor;
    public Color leapMotionHandSelectColor;
    public Color selectedColor;
    public Color errorColor;
    public Color confirmColor;

    public bool selected {get; set;}

    private Color targetColor = Color.white;
    private Color currentColor;

    private Material blockMaterial;

    void Start() {
        currentColor = targetColor;
        blockMaterial = blockMeshRenderer.material;
		audio = GetComponent<AudioSource>();
    }

    void Update() {
        if (selected == false) {
			PlayedAudioMovingBlock = false;
            float t = Mathf.Clamp01(blockRB.velocity.magnitude / 10);
            targetColor = velocityColor.Evaluate(t);

            if ((StateSystem.LastSelectedBlock == null || StateSystem.LastSelectedBlock == gameObject) && !StateSystem.HasSelectedBlockColor && !StateSystem.IsTopBlock(gameObject)) {
                GameObject hand;
                if (hand = GameObject.FindGameObjectWithTag("Hand")) {
                    Vector3 palmPosition = hand.transform.Find("palm/block detector").position;

                    if (blockCollider.bounds.Contains(palmPosition)) {
                        targetColor = leapMotionHandSelectColor;
                    }
                    else if ((palmPosition - transform.position).sqrMagnitude < 100) {
                        float distance = (palmPosition - transform.position).magnitude;
                        targetColor = leapMotionHandColor.Evaluate(1 - distance / 10);
                    }
                }
            }
        }
        else {
            targetColor = selectedColor;
			if( PlayedAudioMovingBlock == false)
			{
				audio.PlayOneShot(MovingBlock, 0.07F);
				PlayedAudioMovingBlock = true;
			}
        }

        currentColor = Color.Lerp(currentColor, targetColor, 10 * Time.deltaTime);
        blockMaterial.SetColor("_Color", currentColor);
        blockMaterial.SetColor("_EmissionColor", currentColor);
    }

    public void flashError() {
        currentColor = errorColor;
    }

    public void flashGood() {
        currentColor = confirmColor;
    }
}