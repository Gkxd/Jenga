using UnityEngine;
using System.Collections;

public class BlockDisappear : MonoBehaviour {
    [Header("Reference Settings")]
    public new Rigidbody rigidbody;
    public MeshRenderer renderer;

    [Header("Gameplay Settings")]
    public Gradient colorRamp;
    public AnimationCurve gradientRamp;
    public float fadeTime;

    private float totalTime;

    void FixedUpdate() {
        rigidbody.AddForce(-Physics.gravity);
    }

    void Update() {

        totalTime += Time.deltaTime;

        if (totalTime > fadeTime) {
            Destroy(gameObject);
        }

        renderer.material.SetColor("_Color", colorRamp.Evaluate(gradientRamp.Evaluate(totalTime / fadeTime)));
    }
}
