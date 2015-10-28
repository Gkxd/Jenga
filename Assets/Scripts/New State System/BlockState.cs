using UnityEngine;
using System.Collections;

public class BlockState : MonoBehaviour {
    #region Serialized Fields
	//Audio


    [Header("Reference Settings")]
    public new Rigidbody rigidbody;
    public MeshRenderer renderer;
    public BlockBehaviour blockBehaviour;

    public GameObject disappearEffect;
    #endregion

	private static float timeOfLastBlockDeleted;

    public bool selected { get; set; }
    public bool isMiddleBlock { get; set; }
    private int numberOfCollidingBlocks;

    void Start() {
		numberOfCollidingBlocks = 0;

    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Block") {
            numberOfCollidingBlocks++;
        }
    }

    void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "RemoveBlock") {
			
			if (Time.time - timeOfLastBlockDeleted < 2) {
				GameState.state = GameState.State.GAME_OVER;
			}

			timeOfLastBlockDeleted = Time.time;

            if (GameState.state == GameState.State.TAKE_BLOCK) {
				GameState.AddNewBlockOnTop(0);
                //GameState.state = GameState.State.PLACE_BLOCK;
            }

			onBlockDestroyed();

            Destroy(gameObject);
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Block") {
            numberOfCollidingBlocks--;
        }
        if (numberOfCollidingBlocks == 0) {
            onExitTower();
        }
    }

    private void onExitTower() {
        // This method will be called when the block exits the tower.

        // A block is "in" the tower if it is touching at least one other block.

        Debug.Log(name + " has exited the tower.");


        // Make player drop the block when it exits the tower
        if (selected) {
            selected = false;
            GameState.lastSelectedBlock = null;


            renderer.enabled = false;

            Instantiate(disappearEffect, transform.position, transform.rotation);
        }
    }

    private void onBlockDestroyed() {
        // This method will be called when the block falls off the platform.

        Debug.Log(name + " has been removed.");
	
    }

    public bool isInTower() {
        return numberOfCollidingBlocks > 0;
    }

    public int getNumberOfCollidingBlocks() {
        return numberOfCollidingBlocks;
    }

    public void setActive(bool active) {
        if (active) {
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.mass = 10;
            blockBehaviour.enabled = renderer.enabled = true;
        }
        else {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.mass = 0;
            blockBehaviour.enabled = renderer.enabled = false;
        }
    }
}
