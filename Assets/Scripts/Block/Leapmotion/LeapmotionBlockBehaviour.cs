using UnityEngine;
using System.Collections;

public class LeapmotionBlockBehaviour : MonoBehaviour {
    [Header("Reference Settings")]
    public new Rigidbody rigidbody;

    public Transform palm { get; set; }

    void FixedUpdate() {
        if (palm != null) {
            rigidbody.velocity = palm.position - rigidbody.position;
            float yaw = StateSystem.LastBlockGrabYaw + palm.localEulerAngles.y - StateSystem.LeapmotionGrabYaw;
            rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.Euler(0, yaw, 0), Time.deltaTime);
        }
    }

    void Update() {
        if (StateSystem.LastSelectedBlock == gameObject) {
            if (palm == null) {
                rigidbody.freezeRotation = false;
            }
            else {
                rigidbody.freezeRotation = true;
            }
        }
        else {
            rigidbody.freezeRotation = false;
        }
    }
}
