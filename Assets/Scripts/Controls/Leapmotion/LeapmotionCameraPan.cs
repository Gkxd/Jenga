using UnityEngine;
using System.Collections;

public class LeapmotionCameraPan : MonoBehaviour {

    [Header("Gameplay Settings")]
    public float panSpeed;
    public float rotateSpeed;

    private float yaw = -45;

	void Update () {
        GameObject hand;
        if (hand = GameObject.FindGameObjectWithTag("Hand")) {
            Transform palm = hand.transform.Find("palm");

            if (palm.localPosition.y > 0.35f) {
                transform.Translate(Vector3.up * panSpeed * Time.deltaTime);
            }
            else if (palm.localPosition.y < 0.1f) {
                transform.Translate(-Vector3.up * panSpeed * Time.deltaTime);
            }

            if (transform.position.y < -0.5f) {
                transform.position = -0.5f * Vector3.up;
            }
            else if (transform.position.y > StateSystem.LayerHeight + 5) {
                transform.position = (StateSystem.LayerHeight + 5) * Vector3.up;
            }

            if (palm.localPosition.x < -0.25f) {
                yaw += rotateSpeed * Time.deltaTime;
            }
            if (palm.localPosition.x > 0.25f) {
                yaw -= rotateSpeed * Time.deltaTime;
            }
        }

        transform.eulerAngles = new Vector3(0, yaw, 0);
	}
}
