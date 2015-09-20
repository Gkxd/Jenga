using UnityEngine;
using System.Collections;

public class GameOverFloor : MonoBehaviour {

    [Header("Reference Settings")]
    public Collider collider;

    [Header("Gameplay Settings")]
    public PhysicMaterial physicMaterial;

    private Vector3 targetScale = new Vector3(7.5f, 1, 7.5f);

	void Update () {
        if (StateSystem.IsGameOver) {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.05f);
            collider.material = physicMaterial;
        }
	}
}
