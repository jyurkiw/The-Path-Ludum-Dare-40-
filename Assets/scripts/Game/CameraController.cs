using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the main camera.
/// The camera is tilted at 75 degrees, and so the controller goes onto a gimble game object that is not rotated. That way,
/// moving along the Z axis doesn't raise the camera up into the air.
/// </summary>
public class CameraController : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        Vector3 cameraMovementThisUpdate = Vector3.zero;

		if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(0, 0, GameGlobals.CAMERA_MOVE_SPEED);
        }

        if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(0, 0, -GameGlobals.CAMERA_MOVE_SPEED);
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(-GameGlobals.CAMERA_MOVE_SPEED, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(GameGlobals.CAMERA_MOVE_SPEED, 0, 0);
        }

        transform.Translate(cameraMovementThisUpdate);
    }
}
