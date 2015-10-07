using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class StateSystem : MonoBehaviour {
    public static int NumberOfLayers {
        get {
            return instance.numberOfLayers;
        }
    }

    public static bool IsGameOver {
        get {
            return instance.isGameOver;//instance.numberOfDangerBlocks > 1;
        }
    }

    public static bool HasBlockBeenPlacedWell { get; private set; }

    public static bool HasSelectedBlockColor { get; private set; }

    public static float LeapmotionGrabYaw { get; private set; }

    public static float LastBlockGrabYaw { get; private set; }

    public static float LayerHeight {
        get {
            return blockHeight * (NumberOfLayers + instance.moveCounter / 3);
        }
    }

    public static GameObject LastSelectedBlock {
        get {
            return instance.lastSelectedBlock;
        }
        set {
            instance.lastSelectedBlock = value;
            HasBlockBeenPlacedWell = false;
        }
    }

    public static GameObject LastInteractedBlock {
        get {
            return instance.lastInteractedBlock;
        }
        set {
            instance.lastInteractedBlock = value;
        }
    }

    //public static Controller controller;

    private static StateSystem instance;
    private static float blockWidth;
    private static float blockHeight;

    [Header("Reference Settings")]
    public HandController controller;

    [Header("Gameplay Settings")]
    public int numberOfLayers;

    private GameObject lastSelectedBlock;
    private GameObject lastInteractedBlock;
    private int moveCounter;
    private int numberOfDangerBlocks;
    private bool isGameOver;

    private Queue<GameObject> topBlocks;

    void Start() {
        instance = this;

        topBlocks = new Queue<GameObject>(3);

        HasBlockBeenPlacedWell = true;

        controller.GetLeapController().EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!HasBlockBeenPlacedWell && LastSelectedBlock != null) {
                if (isLastSelectedBlockPlacedWell()) {
                    placeLastSelectedBlockWell();
                    moveCounter++;
                    AddTopBlock(LastSelectedBlock);
                    HasBlockBeenPlacedWell = true;
                    LastSelectedBlock.GetComponent<ColorChange>().flashGood();
                    LastSelectedBlock = null;
                    LastInteractedBlock = null;
                }
                else {
                    LastSelectedBlock.GetComponent<ColorChange>().flashError();
                }
            }
        }

        GameObject hand;
        if (hand = GameObject.FindGameObjectWithTag("Hand")) {
            HandList hands = controller.GetFrame().Hands;

            Hand leapMotionHand = hands.Frontmost;

            Debug.Log(leapMotionHand.GrabStrength + " " + leapMotionHand.PinchStrength);
            if (leapMotionHand.PinchStrength > 0.6f) {
                if (lastInteractedBlock == null) {
                    foreach (Transform child in transform) {
                        if (lastSelectedBlock == null || child.gameObject == lastSelectedBlock) {
                            Transform palm = hand.transform.Find("palm");
                            Transform index = hand.transform.Find("index/bone3");
                            Vector3 palmPosition = palm.position;
                            Vector3 indexPosition = index.position;
                            BoxCollider blockCollider = child.GetComponent<BoxCollider>();
                            if (blockCollider.bounds.Contains(indexPosition)) {

                                if (IsTopBlock(child.gameObject)) {
                                    FlashTopBlocks();
                                    break;
                                }

                                lastInteractedBlock = child.gameObject;
                                lastInteractedBlock.GetComponent<LeapmotionBlockBehaviour>().palm = palm;
                                lastInteractedBlock.GetComponent<ColorChange>().selected = true;

                                HasSelectedBlockColor = true;
                                LeapmotionGrabYaw = palm.localEulerAngles.y;
                                LastBlockGrabYaw = child.localEulerAngles.y;

                                break;
                            }
                        }
                    }
                }
            }
            else {
                if (lastInteractedBlock != null) {
                    lastInteractedBlock.GetComponent<LeapmotionBlockBehaviour>().palm = null;
                    lastInteractedBlock.GetComponent<ColorChange>().selected = false;
                    HasSelectedBlockColor = false;
                    lastInteractedBlock = null;
                }

                GestureList gestures = controller.GetFrame().Gestures();
                foreach (Gesture gesture in gestures) {
                    if (gesture.Type == Gesture.GestureType.TYPE_CIRCLE) {
                        if (!HasBlockBeenPlacedWell && LastSelectedBlock != null) {
                            if (isLastSelectedBlockPlacedWell()) {
                                placeLastSelectedBlockWell();
                                moveCounter++;
                                AddTopBlock(LastSelectedBlock);
                                HasBlockBeenPlacedWell = true;
                                LastSelectedBlock.GetComponent<ColorChange>().flashGood();
                                LastSelectedBlock = null;
                                LastInteractedBlock = null;
                            }
                            else {
                                LastSelectedBlock.GetComponent<ColorChange>().flashError();
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    private bool isLastSelectedBlockPlacedWell() {
        if (LastSelectedBlock == null) {
            return false;
        }
        else {
            int layer = NumberOfLayers + moveCounter / 3;
            float layerHeight = blockHeight * layer;

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (layer % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            Vector3 positionXZ = Vector3.Scale(LastSelectedBlock.transform.position, Vector3.one - Vector3.up);

            float horizontalPositionDifference = Mathf.Min(
                (positionXZ).sqrMagnitude,
                (positionXZ - offset).sqrMagnitude,
                (positionXZ + offset).sqrMagnitude
                );

            float verticalPositionDifference = Mathf.Abs(LastSelectedBlock.transform.position.y - layerHeight);

            float angleDifference = Mathf.Abs(Vector3.Dot(offset, LastSelectedBlock.transform.forward));
            Debug.Log(horizontalPositionDifference + " " + verticalPositionDifference + " " + angleDifference);

            //return horizontalPositionDifference < 0.4f && verticalPositionDifference < 0.2f && angleDifference < 0.2f;
            return verticalPositionDifference < 0.2f;
        }
    }

    private void placeLastSelectedBlockWell() {
        if (LastSelectedBlock == null) {
            return;
        }
        int layer = NumberOfLayers + moveCounter / 3;
        float layerHeight = blockHeight * layer;

        Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (layer % 2)));
        Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

        Vector3 positionXZ = Vector3.Scale(LastSelectedBlock.transform.position, Vector3.one - Vector3.up);

        float horizontalPositionDifference = positionXZ.sqrMagnitude;
        float offsetAmount = 0;

        if ((positionXZ - offset).sqrMagnitude < horizontalPositionDifference) {
            horizontalPositionDifference = (positionXZ - offset).sqrMagnitude;
            offsetAmount = 1;
        }

        if ((positionXZ + offset).sqrMagnitude < horizontalPositionDifference) {
            horizontalPositionDifference = (positionXZ + offset).sqrMagnitude;
            offsetAmount = -1;
        }

        LastSelectedBlock.transform.position = Vector3.up * layerHeight + offset * offsetAmount;
        LastSelectedBlock.transform.rotation = layerRotation;
    }

    public static void SetBlockDimensions(float width, float height) {
        blockWidth = width;
        blockHeight = height;
    }

    public static void IncreaseDangerBlocks() {
        instance.numberOfDangerBlocks++;
    }

    public static void DecreaseDangerBlocks() {
        instance.numberOfDangerBlocks--;
        if (instance.numberOfDangerBlocks < 0) {
            instance.numberOfDangerBlocks = 0;
        }
    }

    public static void AddTopBlock(GameObject block) {
        instance.addTopBlock(block);
    }

    private void addTopBlock(GameObject block) {
        topBlocks.Enqueue(block);
        if (topBlocks.Count > 6) {
            topBlocks.Dequeue();
        }
    }

    public static bool IsTopBlock(GameObject block) {
        return instance.topBlocks.Contains(block);
    }

    public static void FlashTopBlocks() {
        instance.flashTopBlocks();
    }

    private void flashTopBlocks() {
        foreach (GameObject block in topBlocks) {
            block.GetComponent<ColorChange>().flashError();
        }
    }

    public void setGameOver() {
        isGameOver = true;
    }

    public static void SetGameOver() {
        instance.setGameOver();
    }
}
