using UnityEngine;
using System.Collections;

public class DeleteRemovedPieces : MonoBehaviour {
    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "RemoveBlock") {
            Destroy(gameObject);
            LevelBuilder.AddBlock();
        }
    }
}