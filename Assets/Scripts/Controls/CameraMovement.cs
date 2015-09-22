using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    [Header("Reference Settings")]
    public Transform cameraTransform;

    [Header("Gameplay Settings")]
    public float rotationSpeed;
    public float zoomSpeed;
    public float panSpeed;

    private float yaw;
    private float pitch;
    private float scale;

    private Vector3 resetPosition;
    private float resetYaw;
    private float resetPitch;
    private float resetScale;

    void Start() {
        resetPosition = transform.position;
        resetYaw = transform.eulerAngles.y;
        resetPitch = transform.eulerAngles.x;
        resetScale = transform.localScale.x;

        cameraTransform.transform.LookAt(transform);

        resetView();
    }

    void Update() {
        if (!Input.GetMouseButton(0)) {
            //scale -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.R)) {
                resetView();
            }
        }

        if (Input.GetMouseButton(1)) {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        }

        pitch = Mathf.Clamp(pitch, -40, 40);

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
        transform.localScale = Vector3.one * scale;
    }

    private void resetView() {
        transform.position = resetPosition;
        yaw = resetYaw;
        pitch = resetPitch;
        scale = resetScale;
    }
}
