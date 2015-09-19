using UnityEngine;
using System.Collections;

public class MouseGesture : MonoBehaviour {


    [Header("Reference Settings")]
    public new Camera camera;
    public LayerMask blockMask;

    public Material newMaterial;

    [Header("Gameplay Settings")]
    public float tapThreshold;
    public float tapForce;

    private float mouseDragTime;

    private RaycastHit blockHit;

    private Vector3 mouseHitLocation;
    private Vector3 blockHitPosition;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {

            Ray r = camera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {
                mouseHitLocation = blockHit.point;
                blockHitPosition = blockHit.collider.gameObject.transform.position;

                GameObject block = blockHit.collider.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (blockHit.collider != null) {
                GameObject block = blockHit.collider.gameObject;

                ColorCodingAcceleration script = block.GetComponent<ColorCodingAcceleration>();
                script.selected = false;

                Rigidbody blockRB = block.GetComponent<Rigidbody>();

                if (mouseDragTime < tapThreshold) {
                    Vector3 force = -blockHit.normal * tapForce * (mouseDragTime / 0.03f);
                    blockRB.AddForceAtPosition(force, blockHit.point, ForceMode.VelocityChange);
                }
                else {
                    blockRB.velocity = Vector3.zero;
                    blockRB.constraints = RigidbodyConstraints.None;
                    blockRB.useGravity = true;
                }
            }
        }

        if (Input.GetMouseButton(0)) {

            mouseDragTime += Time.deltaTime;

            if (blockHit.collider != null) {
                if (mouseDragTime >= tapThreshold) {
                    GameObject block = blockHit.collider.gameObject;

                    ColorCodingAcceleration script = block.GetComponent<ColorCodingAcceleration>();
                    script.selected = true;

                    Rigidbody blockRB = block.GetComponent<Rigidbody>();

                    blockRB.constraints = RigidbodyConstraints.FreezeRotation;
                    blockRB.useGravity = false;

                    Vector3 mouseHitLocationScreen = camera.WorldToScreenPoint(mouseHitLocation);
                    Vector3 currentMouseScreenPoint = Input.mousePosition + Vector3.forward * mouseHitLocationScreen.z;
                    Vector3 currentMouseWorldPoint = camera.ScreenToWorldPoint(currentMouseScreenPoint);

                    Vector3 offset = currentMouseWorldPoint - mouseHitLocation;

                    Vector3 projectedOffset = Vector3.ProjectOnPlane(offset, Vector3.up);

                    Vector3 newBlockPosition = blockHitPosition + projectedOffset;
                    newBlockPosition.y = Mathf.Max(0, newBlockPosition.y);
                    Vector3 moveDirection = newBlockPosition - blockRB.position;

                    if (moveDirection.magnitude > 2) {
                        moveDirection = moveDirection.normalized * 10;
                    }

                    blockRB.velocity = moveDirection;
                }
            }
        }
        else {
            mouseDragTime = 0;
        }
    }
}