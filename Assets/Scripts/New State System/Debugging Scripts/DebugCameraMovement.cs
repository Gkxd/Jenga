using UnityEngine;
using System.Collections;

public class DebugCameraMovement : MonoBehaviour {
    public float panSpeed, yawSpeed, pitchSpeed;
    private float height, yaw, pitch;
	void Start () {
        height = transform.position.y;
	    yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
	}

	void Update () {
        if (Input.GetMouseButton(1)) {
            yaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -10, 60);
        }
        if (Input.GetMouseButton(2)) {
            height -= Input.GetAxis("Mouse Y");

            height = Mathf.Max(5, height);
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        transform.position = Vector3.up * height;
	}
}
