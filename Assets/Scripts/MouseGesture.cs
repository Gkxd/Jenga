﻿using UnityEngine;
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
    private Vector3 mouseHitLocation;
    private Vector3 blockHitPosition;

	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Ray r = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(r.origin, r.direction, out blockHit, 100, blockMask)) {
                mouseHitLocation = blockHit.point;
                blockHitPosition = blockHit.collider.gameObject.transform.position;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (blockHit.collider != null) {
                GameObject block = blockHit.collider.gameObject;
                Rigidbody blockRB = block.GetComponent<Rigidbody>();

                if (mouseDragTime < tapThreshold) {
                    Vector3 force = -blockHit.normal * tapForce * (mouseDragTime / 0.1f);
                    blockRB.AddForce(force, ForceMode.VelocityChange);
                }
                else {
                    blockRB.velocity = Vector3.zero;
                    blockRB.constraints = RigidbodyConstraints.None;
                }
            }
        }

        if (Input.GetMouseButton(0)) {
            mouseDragTime += Time.deltaTime;

            if (blockHit.collider != null) {
                if (mouseDragTime >= tapThreshold) {
                    GameObject block = blockHit.collider.gameObject;
                    Rigidbody blockRB = block.GetComponent<Rigidbody>();

                    blockRB.constraints = RigidbodyConstraints.FreezeRotation;

                    Vector3 mouseHitLocationScreen = camera.WorldToScreenPoint(mouseHitLocation);
                    Vector3 currentMouseScreenPoint = Input.mousePosition + Vector3.forward * mouseHitLocationScreen.z;
                    Vector3 currentMouseWorldPoint = camera.ScreenToWorldPoint(currentMouseScreenPoint);

                    //Debug.DrawLine(mouseHitLocation, currentMouseWorldPoint, Color.red, 10);

                    Vector3 offset = currentMouseWorldPoint - mouseHitLocation;
                    Vector3 newBlockPosition = blockHitPosition + offset;
                    newBlockPosition.y = Mathf.Max(0, newBlockPosition.y);
                    blockRB.MovePosition(newBlockPosition);
                }
            }
        }
        else {
            mouseDragTime = 0;
        }
    }
}
