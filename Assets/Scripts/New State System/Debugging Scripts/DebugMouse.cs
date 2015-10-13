using UnityEngine;
using System.Collections;

public class DebugMouse : MonoBehaviour {
    public new Camera camera;
    public LayerMask blockMask;

    private RaycastHit blockHit;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray r = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {
                BlockState blockState = blockHit.collider.gameObject.GetComponent<BlockState>();
                Debug.Log(blockState.name + ", Colliding: " + blockState.getNumberOfCollidingBlocks());
            }
        }
    }
}
