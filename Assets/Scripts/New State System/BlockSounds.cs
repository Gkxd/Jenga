using UnityEngine;
using System.Collections;

public class BlockSounds : MonoBehaviour {
    [Header("Reference Settings")]
    public AudioSource audioSource;
    public AudioClip blockLeaveSound;

    public void playBlockLeaveSound() {
        Debug.Log("Playing sound");
        audioSource.PlayOneShot(blockLeaveSound);
    }

}
