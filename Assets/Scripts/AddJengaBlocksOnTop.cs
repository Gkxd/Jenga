using UnityEngine;
using System.Collections;

public class AddJengaBlocksOnTop : MonoBehaviour {
    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.collider.tag == "block") {
            LevelBuilder.AddBlock();
        }
    }
}
