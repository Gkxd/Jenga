using UnityEngine;
using System.Collections;

public class AddJengaBlocksOnTop : MonoBehaviour {

    [Header("Reference Settings")]
    public GameObject blockPrefab;

    [Header("Gameplay Settings")]
    public float jitter;

    public float currentLayer = 18;
    public float currentLayerBlock = -1;

    void OnCollisionEnter(Collision collisionInfo) {
        print("Collide!!");
        if (collisionInfo.collider.tag == "block") {
            makeNewBlock(collisionInfo.gameObject);
            if (currentLayerBlock == -1)
                currentLayerBlock = 0;
            else if (currentLayerBlock == 0)
                currentLayerBlock = 1;
            else if (currentLayerBlock == 1) {
                currentLayer += 1;
                currentLayerBlock = -1;
            }
        }
    }

    void makeNewBlock(GameObject blockFalling) {
        print(currentLayer);
        print(currentLayerBlock);


        float blockHeight = blockPrefab.transform.localScale.y;
        float blockWidth = blockPrefab.transform.localScale.x;
        print(blockHeight);
        print(blockWidth);

        float layerHeight = blockHeight * currentLayer;

        Vector3 position = new Vector3(0, layerHeight, 0);

        Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (currentLayer % 2)));
        Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

        Vector3 randomness = jitter * (offset * Random.Range(-0.1f, 0.1f) + layerRotation * offset * Random.Range(-0.2f, 0.2f));

        GameObject block = (GameObject)Object.Instantiate(blockPrefab, position + currentLayerBlock * offset + randomness, layerRotation);
        block.name = "Block " + (3 * currentLayer + currentLayerBlock + 1);
        block.transform.parent = blockFalling.transform.parent;
    }
}
