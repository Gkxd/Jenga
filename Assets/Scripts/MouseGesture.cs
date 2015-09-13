using UnityEngine;
using System.Collections;

public class MouseGesture : MonoBehaviour {

    [Header("Reference Settings")]
    public new Camera camera;
    public LayerMask blockMask;

    [Header("Gameplay Settings")]
    public float tapThreshold;
    public float tapForce;

    private float mouseDragTime;

    private RaycastHit blockHit;

	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Ray r = camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {

            }
        }

        if (Input.GetMouseButtonUp(0)) {
            
            if (blockHit.collider != null) {
                if (mouseDragTime < tapThreshold) {
                    GameObject block = blockHit.collider.gameObject;
                    Vector3 force = -blockHit.normal * tapForce * (mouseDragTime / 0.1f);
                    block.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                }
            }
        }

        if (Input.GetMouseButton(0)) {
            mouseDragTime += Time.deltaTime;
        }
        else {
            mouseDragTime = 0;
        }
    }
}
