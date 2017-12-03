using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the main camera.
/// The camera is tilted at 75 degrees, and so the controller goes onto a gimble game object that is not rotated. That way,
/// moving along the Z axis doesn't raise the camera up into the air.
/// </summary>
public class CameraController : MonoBehaviour {
    private int widthActiveDistance, heightActiveDistance, screenMLEdge, screenMREdge, screenMTEdge, screenMBEdge;

    private void Start()
    {
        widthActiveDistance = (int)(Screen.width * 0.05f);
        heightActiveDistance = (int)(Screen.height * 0.05f);

        screenMLEdge = widthActiveDistance;
        screenMREdge = Screen.width - widthActiveDistance;
        screenMTEdge = Screen.height - heightActiveDistance;
        screenMBEdge = heightActiveDistance;
    }

    /// <summary>
    /// Handle camera controls.
    /// Implemented: WASD
    /// TODO: Mouse movement by screen edge proximity
    /// </summary>
    private void Update () {
        Vector3 cameraMovementThisUpdate = Vector3.zero;

        // WASD Movement
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

        // Mouse Movement
        if (Input.mousePosition.x <= screenMLEdge)
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(-Mathf.Lerp(GameGlobals.CAMERA_MOUSE_MIN_MOVE_SPEED, GameGlobals.CAMERA_MOVE_SPEED, Input.mousePosition.x), 0, 0);
        }

        if (Input.mousePosition.x >= screenMREdge)
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(Mathf.Lerp(GameGlobals.CAMERA_MOUSE_MIN_MOVE_SPEED, GameGlobals.CAMERA_MOVE_SPEED, Screen.width - Input.mousePosition.x), 0, 0);
        }

        if (Input.mousePosition.y <= screenMBEdge)
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(0, 0, -Mathf.Lerp(GameGlobals.CAMERA_MOUSE_MIN_MOVE_SPEED, GameGlobals.CAMERA_MOVE_SPEED, Input.mousePosition.y));
        }

        if (Input.mousePosition.y >= screenMTEdge)
        {
            cameraMovementThisUpdate = cameraMovementThisUpdate + new Vector3(0, 0, Mathf.Lerp(GameGlobals.CAMERA_MOUSE_MIN_MOVE_SPEED, GameGlobals.CAMERA_MOVE_SPEED, Screen.height - Input.mousePosition.y));
        }

        transform.Translate(cameraMovementThisUpdate);
    }
}
