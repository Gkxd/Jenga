using UnityEngine;
using System.Collections;

public class SpawnRingEffect : MonoBehaviour {

    [Header("Reference Settings")]
    public GameObject ringEffect;

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Block") {
            foreach (ContactPoint contactPoint in other.contacts) {
                Vector3 point = contactPoint.point;
                Vector3 spawnLocation = new Vector3(point.x, -0.74f, point.z);
                GameObject ring = (GameObject)Instantiate(ringEffect, spawnLocation, ringEffect.transform.rotation);
            }
        }
    }
}
