using UnityEngine;
using System.Collections;

public class DisableOnGameOver : MonoBehaviour {
    void Update() {
        if (StateSystem.IsGameOver) {
            gameObject.SetActive(false);
        }
    }
}
