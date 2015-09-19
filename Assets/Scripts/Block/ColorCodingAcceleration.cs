using UnityEngine;
using System.Collections;

public class ColorCodingAcceleration : MonoBehaviour {

    [Header("Reference Settings")]
    public Rigidbody blockRB;
    public MeshRenderer blockMeshRenderer;


    public bool selected {get; set;}

    void Update() {
        if (selected == false) {
            MeshRenderer blockRenderer = GetComponent<MeshRenderer>();
            float c = Mathf.Clamp01(1 - blockRB.velocity.magnitude / 10);
            blockRenderer.material.SetColor("_Color", new Color(c, c, c, 1));
        }
        else {
            MeshRenderer blockRenderer = GetComponent<MeshRenderer>();
            blockRenderer.material.SetColor("_Color", Color.blue);
        }
    }
}