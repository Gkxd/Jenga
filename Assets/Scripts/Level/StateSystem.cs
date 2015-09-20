using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateSystem : MonoBehaviour {
    public static int NumberOfLayers {
        get {
            return instance.numberOfLayers;
        }
    }

    public static bool IsGameOver {
        get {
            return instance.numberOfDangerBlocks > 1;
        }
    }

    public static bool CheckLastSelectedBlockPlacedWell(bool updateBlocks) {
        if (LastSelectedBlock == null) {
            return true;
        }
        else {

            int layer = NumberOfLayers + instance.moveCounter / 3;
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

            if (horizontalPositionDifference < 0.4f && verticalPositionDifference < 0.2f && angleDifference < 0.2f) {
                if (updateBlocks) {
                    instance.moveCounter++;
                    AddTopBlock(LastSelectedBlock);
                }
                return true;
            }
            else {
                return false;
            }
        }
    }

    public static GameObject LastSelectedBlock {
        get {
            return instance.lastSelectedBlock;
        }
        set {
            instance.lastSelectedBlock = value;
            blockWidth = LastSelectedBlock.transform.localScale.x;
            blockHeight = LastSelectedBlock.transform.localScale.y;
        }
    }

    private static StateSystem instance;
    private static float blockWidth;
    private static float blockHeight;

    [Header("Gameplay Settings")]
    public int numberOfLayers;

    private GameObject lastSelectedBlock;
    private int moveCounter;
    private int numberOfDangerBlocks;

    private Queue<GameObject> topBlocks;

    void Start() {
        instance = this;

        topBlocks = new Queue<GameObject>(3);
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
}
