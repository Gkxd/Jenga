using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {
    private static LevelBuilder instance;

    [Header("Reference Settings")]
    public GameObject blockPrefab;

    [Header("Gameplay Settings")]
    public float jitter;

    private float blockHeight;
    private float blockWidth;

    private int additionalBlockCounter;

    void Start() {
        blockHeight = blockPrefab.transform.localScale.y;
        blockWidth = blockPrefab.transform.localScale.x;

        buildLevel();
        instance = this;
    }

    public static void BuildLevel() {
        instance.buildLevel();
    }

    private void buildLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        additionalBlockCounter = 0;
        
        for (int i = 0; i < StateSystem.NumberOfLayers; i++) {
            float layerHeight = blockHeight * i;

            Vector3 position = new Vector3(0, layerHeight, 0);

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (i % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            for (int j = -1; j <= 1; j++) {
                Vector3 randomness = jitter * (offset * Random.Range(-0.1f, 0.1f) + layerRotation * offset * Random.Range(-0.2f, 0.2f));

                GameObject block = (GameObject)Instantiate(blockPrefab, position + j * offset + randomness, layerRotation);
                block.name = "Block " + (3 * i + j + 1);
                block.transform.parent = transform;

                StateSystem.AddTopBlock(block);
            }
        }
    }

    public static void AddBlock() {
        instance.addBlock();
    }

    private void addBlock() {
        int layer = StateSystem.NumberOfLayers + additionalBlockCounter / 3;

        if (layer > 70) {
            layer = 39 + layer % 32;
        }

        float layerHeight = blockHeight * (layer + 0.5f);

        Vector3 position = new Vector3(0, layerHeight, 0);

        Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (layer % 2)));
        Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0) * (additionalBlockCounter % 3 - 1);

        GameObject block = (GameObject)Object.Instantiate(blockPrefab, position + offset, layerRotation);
        block.name = "Block " + (StateSystem.NumberOfLayers * 3 + additionalBlockCounter);
        block.transform.parent = transform;

        additionalBlockCounter++;
    }
}
