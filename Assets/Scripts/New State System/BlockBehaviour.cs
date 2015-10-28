using UnityEngine;
using System.Collections;

using Leap;

public class BlockBehaviour : MonoBehaviour {
    #region Serialized Fields
	[Header("Audio Settings")]
	public AudioClip MovingBlock;
	public AudioSource audio;
	public bool PlayedAudioMovingBlock = false;

	[Header("Reference Settings")]
    public new Rigidbody rigidbody;
    public MeshRenderer renderer;
    public BoxCollider collider;
    public BlockState blockState;

    [Header("Gameplay Settings")]
    [Header("Color")]
    public Gradient velocityColor;
    public Gradient leapMotionHandColor;
    public Color leapMotionHandSelectColor;
    public Color selectedColor;
	public Color EndTurnColor;

    [Header("Damping")]
    public float maxDamp;
    public float dampIncrease;

    [Header("LeapMotion")]
    public float takeThreshold;
    public float dropThreshold;
    #endregion

    private Color targetColor = Color.white;
    private Color currentColor;
    private Material blockMaterial;

    private float dampAmount;

    private HandController controller;

    void Start() {
        currentColor = targetColor;
        blockMaterial = renderer.material;
        controller = GameObject.FindGameObjectWithTag("LeapmotionController").GetComponent<HandController>();

		//audio = GetComponents<AudioSource>() [1];
    }

    void FixedUpdate() {
        if (blockState.selected) {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            HandList hands = controller.GetFrame().Hands;
            Hand leapMotionHand = hands.Frontmost;

            GameObject hand = GameObject.FindGameObjectWithTag("Hand");

            if (hand != null) {
                Vector3 grabPosition = hand.transform.Find("palm/block detector").position;

                Vector3 velocity = grabPosition - rigidbody.position;

                velocity = Vector3.ProjectOnPlane(velocity, Vector3.up); // Remove vertical movement
                Vector3 positionXZ = Vector3.ProjectOnPlane(rigidbody.position, Vector3.up);

                if (blockState.isMiddleBlock) {
                    rigidbody.velocity = velocity;
                }
                else { // Don't let people move side blocks towards center of tower
                    if (Vector3.Dot(velocity, positionXZ) >= 0) {
                        rigidbody.velocity = velocity;
                    }
                    else {
                        rigidbody.velocity = velocity - Vector3.Project(velocity, positionXZ);
                    }
                }
            }
        }
        else {
            rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    void Update() {
		PlayedAudioMovingBlock = false;

        HandList hands = controller.GetFrame().Hands;
        Hand leapMotionHand = hands.Frontmost;

        GameObject hand = GameObject.FindGameObjectWithTag("Hand");

		if (GameObject.Find("StateBlockDisappear(Clone)") != null) {
			currentColor = EndTurnColor;
			print ("END TURN");
		}


        if (!blockState.selected) {
            float t = Mathf.Clamp01(rigidbody.velocity.magnitude / 10);
            targetColor = velocityColor.Evaluate(t);

            if (GameState.state == GameState.State.TAKE_BLOCK) {
                if (hand != null) {
                    Vector3 grabPosition = hand.transform.Find("palm/block detector").position;

                    if (collider.bounds.Contains(grabPosition) && GameState.lastSelectedBlock == null) {
                        targetColor = leapMotionHandSelectColor;

                        if (leapMotionHand.GrabStrength > takeThreshold) {
                            blockState.selected = true;
                            GameState.lastSelectedBlock = gameObject;
                        }
                    }
                    else if ((grabPosition - transform.position).sqrMagnitude < 100 && GameState.lastSelectedBlock == null) {
                        float distance = (grabPosition - transform.position).magnitude;
                        targetColor = leapMotionHandColor.Evaluate(1 - distance / 10);
                    }
                }
            }
        }
        else {
            targetColor = selectedColor;

			if( PlayedAudioMovingBlock == false)
			{
				audio.PlayOneShot(MovingBlock, 0.07F);
				PlayedAudioMovingBlock = true;
			}

			if (hand == null || (leapMotionHand.GrabStrength < dropThreshold && GameState.lastSelectedBlock == gameObject)) {
                blockState.selected = false;
                GameState.lastSelectedBlock = null;
            }
        }

        if (GameState.lastSelectedBlock != null) {
            dampAmount = 0;
        }
        else {
            dampAmount = Mathf.Min(maxDamp, dampAmount + dampIncrease * Time.deltaTime);
        }


        rigidbody.drag = dampAmount;
        rigidbody.angularDrag = dampAmount;

        currentColor = Color.Lerp(currentColor, targetColor, 10 * Time.deltaTime);
        blockMaterial.SetColor("_Color", currentColor);
        blockMaterial.SetColor("_EmissionColor", currentColor);
    }
}
