using UnityEngine;
using System.Collections;

public class ShrinkOnGameOver : MonoBehaviour {

    Vector3 targetScale = Vector3.up;

	void Update () {
        if (StateSystem.IsGameOver) {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.05f);
        }
        if (transform.localScale.x < 0.5f) {
            gameObject.SetActive(false);
        }
	}
}
