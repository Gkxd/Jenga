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

    public float rotateSpeed;
    public float moveUpSpeed;

    private float mouseDragTime;

    private RaycastHit blockHit;

    private Vector3 mouseHitLocation;
    private Vector3 blockHitPosition;
    private Vector3 lastBlockPosition;
    private Vector3 blockHitEulerAngles;
    private float blockYaw;

    void Update() {
        if (!StateSystem.IsGameOver) {
            if (Input.GetMouseButtonDown(0)) {

                Ray r = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {

                    if (StateSystem.IsTopBlock(blockHit.collider.gameObject)) {
                        blockHit = new RaycastHit(); // Invalid block clicked, reset raycast info
                        StateSystem.FlashTopBlocks();
                    }
                    else if (!StateSystem.HasBlockBeenPlacedWell && StateSystem.LastSelectedBlock != null) {
                        if (blockHit.collider.gameObject != StateSystem.LastSelectedBlock) {
                            blockHit = new RaycastHit(); // Invalid block clicked, reset raycast info
                            StateSystem.LastSelectedBlock.GetComponent<ColorChange>().flashError();
                        }
                        else {
                            mouseHitLocation = blockHit.point;
                            lastBlockPosition = blockHitPosition = blockHit.collider.gameObject.transform.position;
                            blockHitEulerAngles = blockHit.collider.gameObject.transform.eulerAngles;
                            blockYaw = 0;
                        }
                    }
                    else {

                        mouseHitLocation = blockHit.point;
                        lastBlockPosition = blockHitPosition = blockHit.collider.gameObject.transform.position;
                        blockHitEulerAngles = blockHit.collider.gameObject.transform.eulerAngles;
                        blockYaw = 0;

                        StateSystem.LastInteractedBlock = blockHit.collider.gameObject;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (blockHit.collider != null) {
                    GameObject block = blockHit.collider.gameObject;

                    ColorChange colorChange = block.GetComponent<ColorChange>();
                    colorChange.selected = false;

                    Rigidbody blockRB = block.GetComponent<Rigidbody>();

                    if (mouseDragTime < tapThreshold) {
                        if (!StateSystem.HasBlockBeenPlacedWell && StateSystem.LastSelectedBlock != null && StateSystem.LastSelectedBlock != block) {
                            StateSystem.LastSelectedBlock.GetComponent<ColorChange>().flashError();
                        }
                        else {
							//Swetha
							(blockHit.collider.gameObject.GetComponent<AudioSource>()).Play();

                            Vector3 force = -blockHit.normal * tapForce * (mouseDragTime / 0.03f);
                            blockRB.AddForceAtPosition(force, blockHit.point, ForceMode.VelocityChange);
                            //StateSystem.LastSelectedBlock = block;
                        }
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

                        ColorChange colorChange = block.GetComponent<ColorChange>();
                        colorChange.selected = true;

                        Rigidbody blockRB = block.GetComponent<Rigidbody>();


                        if (Input.GetKey(KeyCode.C)) {
                            blockRB.constraints = RigidbodyConstraints.FreezeAll;
                        }
                        else if (Input.GetMouseButton(2)) {
                            blockRB.constraints = RigidbodyConstraints.FreezePosition;
                        }
                        else {
                            blockRB.constraints = RigidbodyConstraints.None;
                        }
                        blockRB.useGravity = false;

                        Vector3 mouseHitLocationScreen = camera.WorldToScreenPoint(mouseHitLocation);
                        Vector3 currentMouseScreenPoint = Input.mousePosition + Vector3.forward * mouseHitLocationScreen.z;
                        Vector3 currentMouseWorldPoint = camera.ScreenToWorldPoint(currentMouseScreenPoint);

                        Vector3 offset = currentMouseWorldPoint - ((mouseHitLocation - blockHitPosition) + lastBlockPosition);

                        Vector3 projectedOffset = Vector3.ProjectOnPlane(offset, Vector3.up);

                        Vector3 newBlockPosition = lastBlockPosition + projectedOffset;
                        newBlockPosition.y = Mathf.Max(0, newBlockPosition.y);
                        Vector3 moveDirection = newBlockPosition - blockRB.position;

                        if (moveDirection.magnitude > 2) {
                            moveDirection = moveDirection.normalized * 10;
                        }

                        blockRB.velocity = moveDirection + Vector3.up * Input.GetAxis("Mouse ScrollWheel") * moveUpSpeed;
                        lastBlockPosition = blockRB.position;

                        if (Input.GetMouseButton(2)) {
                            blockRB.angularVelocity = Vector3.down * Input.GetAxis("Mouse X") * rotateSpeed;
                        }
                        else {
                            blockRB.angularVelocity = Vector3.zero;
                        }

                        /*
                        if ((blockHitPosition - lastBlockPosition).sqrMagnitude > 4) {
                            StateSystem.LastSelectedBlock = blockHit.collider.gameObject;
                        }
                         */
                    }
                }
            }
            else {
                mouseDragTime = 0;
            }
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                Ray r = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {
                    Rigidbody blockRB = blockHit.collider.gameObject.GetComponent<Rigidbody>();

                    blockRB.AddExplosionForce(100, blockRB.position, 100, 100, ForceMode.VelocityChange);
                }
            }
        }
    }
}