using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

    [Header("Reference Settings")]
    public Rigidbody blockRB;
    public MeshRenderer blockMeshRenderer;

    [Header("Gameplay Settings")]
    public Gradient velocityColor;
    public Color selectedColor;
    public Color errorColor;
    public Color confirmColor;

    public bool selected {get; set;}

    private Color targetColor = Color.white;
    private Color currentColor;

    private Material blockMaterial;

    void Start() {
        currentColor = targetColor;
        blockMaterial = blockMeshRenderer.material;
    }

    void Update() {
        if (selected == false) {
            float t = Mathf.Clamp01(blockRB.velocity.magnitude / 10);
            targetColor = velocityColor.Evaluate(t);
        }
        else {
            targetColor = selectedColor;
        }

        currentColor = Color.Lerp(currentColor, targetColor, 2 * Time.deltaTime);
        blockMaterial.SetColor("_Color", currentColor);
    }

    public void flashError() {
        currentColor = errorColor;
    }

    public void flashGood() {
        currentColor = confirmColor;
    }
}