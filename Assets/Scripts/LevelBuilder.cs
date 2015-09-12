using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {

    public GameObject blockPrefab;
    public int numberOfLayers;
    public float jitter;

    void Start() {
        float blockHeight = blockPrefab.transform.localScale.y;
        float blockWidth = blockPrefab.transform.localScale.x;

        for (int i = 0; i < numberOfLayers; i++) {
            float layerHeight = blockHeight * i;

            Vector3 position = new Vector3(0, layerHeight, 0);

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (i % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            for (int j = -1; j <= 1; j++) {
                Vector3 randomness = jitter * (offset * Random.Range(-0.1f, 0.1f) + layerRotation * offset * Random.Range(-0.2f, 0.2f));

                GameObject block = (GameObject)Object.Instantiate(blockPrefab, position + j * offset + randomness, layerRotation);
                block.transform.parent = transform;
            }
        }
    }
}
