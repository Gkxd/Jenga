using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public Transform cameraTransform;

    public float rotationSpeed;
    public float zoomSpeed;

    private float yaw = -45;
    private float pitch = 0;

    private float scale = 1;

    void Start() {
        cameraTransform.transform.LookAt(transform);
    }

    void Update() {
        
        if (Input.GetMouseButton(1)) {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }

        scale -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        scale = Mathf.Max(scale, 0.1f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        transform.localScale = Vector3.one * scale;
    }
}
