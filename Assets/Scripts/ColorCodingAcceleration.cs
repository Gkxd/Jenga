using UnityEngine;
using System.Collections;

public class ColorCodingAcceleration : MonoBehaviour {

    [Header("Reference Settings")]
    public Rigidbody rb;


    public bool selected {get; set;}

    void Update() {
        if (selected == false) {
            if (rb.velocity.z != 0.0) {
                MeshRenderer blockRenderer = GetComponent<MeshRenderer>();
                float c = Mathf.Clamp01(1 - rb.velocity.z);
                blockRenderer.material.SetColor("_Color", new Color(c, c, c, 1));
            }
            else {
                changeColor();
            }
        }
        else {
            MeshRenderer blockRenderer = GetComponent<MeshRenderer>();

            blockRenderer.material.SetColor("_Color", Color.red);
        }
    }

    void changeColor() {
        MeshRenderer blockRenderer = GetComponent<MeshRenderer>();
        blockRenderer.material.SetColor("_Color", Color.white);
    }
}