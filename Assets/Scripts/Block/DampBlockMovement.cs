using UnityEngine;
using System.Collections;

public class DampBlockMovement : MonoBehaviour {

    [Header("Reference Settings")]
    public Rigidbody blockRB;

    [Header("Gameplay Settings")]
    public float maxDamp;
    public float dampIncrease;

    private float dampAmount;

    void Start() {
        dampAmount = maxDamp;
    }

    void Update() {
        if (StateSystem.IsGameOver || Input.GetMouseButton(0)) {
            dampAmount = 0;
        }
        else {
            dampAmount = Mathf.Min(maxDamp, dampAmount + dampIncrease * Time.deltaTime);
        }

        blockRB.drag = dampAmount;
        blockRB.angularDrag = dampAmount;
    }
}
