using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateSystem : MonoBehaviour {

    public static int DangerBlocks {
        private get {
            return instance.numberOfDangerBlocks;
        }
        set {
            instance.numberOfDangerBlocks = value;
        }
    }

    public static int NumberOfLayers {
        private get {
            return instance.numberOfLayers;
        }
        set {
            instance.numberOfLayers = value;
        }
    }

    public static bool IsGameOver {
        get {
            return DangerBlocks > 1;
        }
    }

    public static bool IsLastSelectedBlockPlacedWell {
        get {
            if (LastSelectedBlock == null) {
                return true;
            }
            else {

                int layer = NumberOfLayers + instance.moveCounter / 3 + 1;
                float layerHeight = blockHeight * layer - 1.5f;

                Vector3 targetBlockPosition = new Vector3(0, layerHeight, 0);

                Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (layer % 2)));
                Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

                float positionDifference = Mathf.Min(
                    (targetBlockPosition - LastSelectedBlock.transform.position).sqrMagnitude,
                    (targetBlockPosition - offset - LastSelectedBlock.transform.position).sqrMagnitude,
                    (targetBlockPosition + offset - LastSelectedBlock.transform.position).sqrMagnitude
                    );

                float angleDifference = Mathf.Abs(Vector3.Dot(offset, LastSelectedBlock.transform.forward));

                if (positionDifference < 8 && angleDifference < 3) {
                    instance.moveCounter++;
                    AddTopBlock(LastSelectedBlock);
                    return true;
                }
                else {
                    return false;
                }
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

    private GameObject lastSelectedBlock;
    private int moveCounter;
    private int numberOfDangerBlocks;
    private int numberOfLayers;

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
    }

    public static void AddTopBlock(GameObject block) {
        instance.addTopBlock(block);
    }

    public static bool IsTopBlock(GameObject block) {
        return instance.topBlocks.Contains(block);
    }

    private void addTopBlock(GameObject block) {
        topBlocks.Enqueue(block);
        if (topBlocks.Count > 6) {
            topBlocks.Dequeue();
        }
    }
}
