using UnityEngine;
using System.Collections;

public class RotateSkybox : MonoBehaviour {

    public float rotationSpeed;

    private float yaw = 0;

	void Update () {
        yaw += rotationSpeed * Time.deltaTime;
        yaw %= 360;

        transform.localEulerAngles = new Vector3(0, yaw, 0);
	}
}
