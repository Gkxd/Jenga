using UnityEngine;
using System.Collections;

public class LockToPalm : MonoBehaviour {
    [Header("Reference Settings")]
    public new Rigidbody rigidbody;

    public Transform palm { get; set; }

    void FixedUpdate() {
        if (palm != null) {
            rigidbody.velocity = palm.position - rigidbody.position;
        }
    }
}
