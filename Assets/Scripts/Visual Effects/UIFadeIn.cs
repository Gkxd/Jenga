using UnityEngine;
using System.Collections;

public class UIFadeIn : MonoBehaviour {

    // Use this for initialization

    public GameObject blocksOnTop;

    public void fadeout() {
        StartCoroutine(DoFadeOut());

    }

    void Start() {
        StartCoroutine(DoFadeOut());
    }

    void Update() {
        if (StateSystem.LastSelectedBlock != null) {
            StartCoroutine(DoFadeIn());
        }
        else if (GetComponent<CanvasGroup>().alpha > 0) {
            StartCoroutine(DoFadeOut());
        }
    }

    IEnumerator DoFadeOut() {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        canvasGroup.interactable = false;
        yield return null;
    }

    IEnumerator DoFadeIn() {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1) {
            canvasGroup.alpha += Time.deltaTime / 2;
            DoFadeOut();
            yield return null;
        }
        canvasGroup.interactable = true;
        yield return null;
    }
}
