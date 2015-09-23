using UnityEngine;
using System.Collections;

public class LockSelectedBlock : MonoBehaviour {

    void OnTriggerEnter(Collider collisionInfo) {
        if (collisionInfo.gameObject == StateSystem.LastInteractedBlock) {
            StateSystem.LastSelectedBlock = collisionInfo.gameObject;
        }
    }
}
