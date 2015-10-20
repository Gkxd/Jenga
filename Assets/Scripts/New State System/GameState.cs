using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
    public enum State {
        TAKE_BLOCK, PLACE_BLOCK, GAME_OVER
    }

    #region Static Fields
    private static GameState instance;
    private static float blockWidth;
    private static float blockHeight;
    private static int numberOfDisconnectedBlocks;

    private static Queue<BlockState> invisibleBlocks;

    public static GameObject lastSelectedBlock;

    public static State state;
    #endregion

    #region Serialized Fields
    [Header("Reference Settings")]
    public GameObject blockPrefab;

    [Header("Gameplay Settings")]
    public float jitter;

    public int numberOfLayers;
    public int additionalLayers;
    #endregion

    void Awake() {
        instance = this;
        blockWidth = blockPrefab.transform.localScale.x;
        blockHeight = blockPrefab.transform.localScale.y;

        state = State.TAKE_BLOCK;

        invisibleBlocks = new Queue<BlockState>();

        buildLevel();
    }

    void Update() {
        if (state == State.PLACE_BLOCK) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                AddNewBlockOnTop();
                state = State.TAKE_BLOCK;
            }
        }
    }

    void buildLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfLayers + additionalLayers; i++) {
            float layerHeight = blockHeight * i;

            Vector3 position = new Vector3(0, layerHeight, 0);

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (i % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            for (int j = -1; j <= 1; j++) {
                Vector3 randomness = jitter * (offset * Random.Range(-0.1f, 0.1f) + layerRotation * offset * Random.Range(-0.2f, 0.2f));

                GameObject block = (GameObject)Instantiate(blockPrefab, position + j * offset + randomness, layerRotation);
                block.name = "Block " + (3 * i + j + 1);
                block.transform.parent = transform;

                BlockState blockState = block.GetComponent<BlockState>();
                blockState.setActive(i < numberOfLayers);

                if (j == 0) {
                    blockState.isMiddleBlock = true;
                }
                else {
                    blockState.isMiddleBlock = false;
                }

                if (i >= numberOfLayers) {
                    invisibleBlocks.Enqueue(blockState);
                }
            }
        }
    }

    public static void AddNewBlockOnTop() {
        if (invisibleBlocks.Count > 0) {
            invisibleBlocks.Dequeue().setActive(true);
        }
    }
}
