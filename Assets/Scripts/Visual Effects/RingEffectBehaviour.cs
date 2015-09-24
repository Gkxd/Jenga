using UnityEngine;
using System.Collections;

public class RingEffectBehaviour : MonoBehaviour {

    [Header("Reference Settings")]
    public MeshRenderer renderer;

    [Header("Gameplay Settings")]
    public Gradient colorChange;
    public float maxSize;

    private float currentSize;

    void Start() {
        transform.localScale = new Vector3(currentSize, currentSize, 1);
	}
	
	void Update () {
        currentSize = Mathf.Lerp(currentSize, maxSize, 2*Time.deltaTime);
        
        transform.localScale = new Vector3(currentSize, currentSize, 1);
        renderer.material.SetColor("_Color", colorChange.Evaluate(currentSize/maxSize));

        if (maxSize - currentSize < 0.2f) {
            Destroy(gameObject);
        }
	}
}
