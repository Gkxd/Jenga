using UnityEngine;
using System.Collections;

public class DeleteRemovedPieces : MonoBehaviour {
    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "ground") {
            Destroy(gameObject);
        }
    }
}