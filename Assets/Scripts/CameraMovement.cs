using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    [Header("Reference Settings")]
    public Transform cameraTransform;

    [Header("Gameplay Settings")]
    public float rotationSpeed;
    public float zoomSpeed;
    public float panSpeed;

    private float yaw = -45;
    private float pitch = 0;
    private float scale = 1;

    void Start() {
        cameraTransform.transform.LookAt(transform);
    }

    void Update() {


        if (!Input.GetMouseButton(0)) {
            if (Input.GetMouseButton(1)) {
                yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            }
            else if (Input.GetMouseButton(2)) {
                Vector3 movement = transform.up * Input.GetAxis("Mouse Y") + transform.right * Input.GetAxis("Mouse X");
                transform.position -= movement * panSpeed * Time.deltaTime;
            }
            scale -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;

            transform.eulerAngles = new Vector3(pitch, yaw, 0);
            transform.localScale = Vector3.one * scale;

            if (Input.GetKey(KeyCode.Space)) {
                resetView();
            }
        }
    }

    private void resetView() {
        transform.position = Vector3.zero;
        yaw = -45;
        pitch = 0;
        scale = 1;
    }
}
