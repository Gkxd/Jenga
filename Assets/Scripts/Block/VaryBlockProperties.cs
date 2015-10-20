using UnityEngine;
using System.Collections;

public class VaryBlockProperties : MonoBehaviour {

    [Header("Reference Settings")]
    public Rigidbody blockRB;

    [Header("Gameplay Settings")]
    public float mass;
    public float massJitter;

	// Use this for initialization
	void Start () {
        blockRB.mass = mass + Random.Range(-massJitter, massJitter);
	}
}
