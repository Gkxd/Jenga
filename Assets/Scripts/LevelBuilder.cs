using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {

    [Header("Reference Settings")]
    public GameObject blockPrefab;

    [Header("Gameplay Settings")]
    public int numberOfLayers;
    public float jitter;

    private static LevelBuilder instance;

    void Start() {
        instance = this;

        buildLevel();
    }

    public static void buildLevel() {
        foreach (Transform child in instance.transform) {
            DestroyImmediate(child.gameObject);
        }

        float blockHeight = instance.blockPrefab.transform.localScale.y;
        float blockWidth = instance.blockPrefab.transform.localScale.x;

        for (int i = 0; i < instance.numberOfLayers; i++) {
            float layerHeight = blockHeight * i;

            Vector3 position = new Vector3(0, layerHeight, 0);

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (i % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            for (int j = -1; j <= 1; j++) {
                Vector3 randomness = instance.jitter * (offset * Random.Range(-0.1f, 0.1f) + layerRotation * offset * Random.Range(-0.2f, 0.2f));

                GameObject block = (GameObject)Object.Instantiate(instance.blockPrefab, position + j * offset + randomness, layerRotation);
                block.name = "Block " + (3 * i + j + 1);
                block.transform.parent = instance.transform;
            }
        }
    }
}
