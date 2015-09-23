using UnityEngine;
using System.Collections;

public class TrackBlockDanger : MonoBehaviour {

    void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.tag == "Block") {
            StateSystem.DecreaseDangerBlocks();
        }
    }

    void OnTriggerExit(Collider collisionInfo) {
        if (collisionInfo.tag == "Block") {
            StateSystem.IncreaseDangerBlocks();
        }
    }
}
