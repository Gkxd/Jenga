using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
    public enum State {
        TAKE_BLOCK, PLACE_BLOCK, GAME_OVER
    }
	//Audio
	[Header("Audio Settings")]
	public AudioClip impact;
	AudioSource audio;
	public bool playedAudioWhenFalling = false;

    #region Static Fields
    private static GameState instance;
    private static float blockWidth;
    private static float blockHeight;
    private static int numberOfDisconnectedBlocks;
    private static bool hasBlockBeenPlaced;

    private static Queue<JengaLayer> invisibleLayers;
    private static JengaLayer topLayer;

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
		Application.targetFrameRate = 14;
        instance = this;
        blockWidth = blockPrefab.transform.localScale.x;
        blockHeight = blockPrefab.transform.localScale.y;

        state = State.TAKE_BLOCK;

        invisibleLayers = new Queue<JengaLayer>();

        buildLevel();
		audio = GetComponent<AudioSource>();
    }

    void Update() {
        if (state == State.PLACE_BLOCK) {
			if( playedAudioWhenFalling == false)
			{
				print ("PLAYING AUDIO");
				audio.PlayOneShot(impact, 0.07F);
				playedAudioWhenFalling = true;
			}

            if (Input.GetKeyDown(KeyCode.Space)) {
                AddNewBlockOnTop(0);
				playedAudioWhenFalling = false;
                state = State.TAKE_BLOCK;
            }
        }
    }

    void buildLevel() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        topLayer = new JengaLayer();

        for (int i = 0; i < numberOfLayers + additionalLayers; i++) {
            float layerHeight = blockHeight * i;

            Vector3 position = new Vector3(0, layerHeight, 0);

            Quaternion layerRotation = Quaternion.Euler(Vector3.up * (90 * (i % 2)));
            Vector3 offset = layerRotation * new Vector3(blockWidth, 0, 0);

            JengaLayer blockLayer = new JengaLayer();
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

                    switch (j) {
                    case -1:
                        blockLayer.a = blockState;
                        break;
                    case 0:
                        blockLayer.b = blockState;
                        break;
                    case 1:
                        blockLayer.c = blockState;
                        break;
                    }
                    invisibleLayers.Enqueue(blockLayer);
                }
                else if (i == numberOfLayers - 1) {
                    switch (j) {
                        case -1:
                            topLayer.a = blockState;
                            break;
                        case 0:
                            topLayer.b = blockState;
                            break;
                        case 1:
                            topLayer.c = blockState;
                            break;
                    }
                }
            }
        }
    }

    public static void AddNewBlockOnTop(int i) {
        if (invisibleLayers.Count > 0) {
            JengaLayer currentLayer = invisibleLayers.Peek();

            currentLayer.setActive(i);

            if (currentLayer.a == null && currentLayer.b == null && currentLayer.c == null) {
                invisibleLayers.Dequeue();
            }

            hasBlockBeenPlaced = true;
        }
    }

    private class JengaLayer {
        public BlockState a { get; set; }
        public BlockState b { get; set; }
        public BlockState c { get; set; }

        public void setActive(int i) {
            switch (i) {
            case 0:
                if (a != null) {
                    a.setActive(true);
                    a = null;
                }
                else if (b != null) {
                    b.setActive(true);
                    b = null;
                }
                else if (c != null) {
                    c.setActive(true);
                    c = null;
                }
                break;
            case 1:
                if (b != null) {
                    b.setActive(true);
                    b = null;
                }
                else if (c != null) {
                    c.setActive(true);
                    c = null;
                }
                else if (a != null) {
                    a.setActive(true);
                    a = null;
                }
                break;
            case 2:
                if (c != null) {
                    c.setActive(true);
                    c = null;
                }
                else if (b != null) {
                    b.setActive(true);
                    b = null;
                }
                else if (a != null) {
                    a.setActive(true);
                    a = null;
                }
                break;
            }
        }

        public bool contains(BlockState block) {
            return a == block || b == block || c == block;
        }
    }

    public static bool isTopBlock(BlockState block) {
        if (!hasBlockBeenPlaced) {
            return topLayer.contains(block);
        }
        else {
            return invisibleLayers.Peek().contains(block);
        }
    }
}
