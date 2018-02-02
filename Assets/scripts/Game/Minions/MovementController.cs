using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class to control minion movement.
/// The only thing this class does is lerp between nodes over a set interval.
/// Destruction of minions and handling what happens when we reach the origin
/// are handled elsewhere.
/// Only corner case is not crashing the game when node.OutNode is null (we should just stop moving).
/// </summary>
public class MovementController : MonoBehaviour
{
    public const float LEFT_ROT = 0;
    public const float UP_ROT = 90f;
    public const float RIGHT_ROT = 180f;
    public const float DOWN_ROT = 270f;

    public Node Location { get; set; }
    private bool _active = false;
    public bool Active
    {
        get { return _active; }
        private set { _active = value; }
    }

    public float MoveInterval;
    public float TurnInterval;

    private float currentInterval;
    private NODE_DIRECTION facing;
    private bool turning = false;
    private Transform modelTransform;
	
	// Update is called once per frame
	public void Update ()
    {
        if (Active)
        {
            currentInterval += Time.deltaTime;

            // We're at or past the beginning of the next node.
            if (currentInterval > MoveInterval && !turning)
            {
                currentInterval -= MoveInterval;
                Location = Location.OutNode;

                // Detect turning
                if (Location.OutNode != null && Location.OutDirection != facing)
                    turning = true;
            }
            else if (currentInterval > TurnInterval && turning)
            {
                currentInterval -= TurnInterval;
                facing = Location.OutDirection;
                turning = false;
            }

            // Figure out the proper minion location based on the movement interval and handle arriving at the origin
            if (!turning && Location.OutNode != null)
            {
                Vector3 lerpedLocation = Vector3.Lerp(Location.VectorLocation.ToVector3(), Location.OutNode.VectorLocation.ToVector3(), currentInterval);
                Quaternion currentRotation = transform.rotation;

                transform.SetPositionAndRotation(lerpedLocation, currentRotation);
            }
            else if (turning)
            {
                Vector3 location = Location.VectorLocation.ToVector3();
                float rotation = Mathf.LerpAngle(GetDirectionRotation(facing), GetDirectionRotation(Location.OutDirection), currentInterval);

                modelTransform.localRotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                Active = false;
            }
        }
	}

    /// <summary>
    /// Activate this minion.
    /// All this does is begin moving down the path and animating the walk cycle.
    /// It's intended to be called right after instantiation, and is basically here to
    /// give us time to initialize the minion.
    /// Also does some light lifting, like moving the minion automatically to the passed node location.
    /// </summary>
    /// <param name="location"></param>
    public void Activate(Node location)
    {
        Location = location;
        float minionRot = GetDirectionRotation(location.OutDirection);

        transform.SetPositionAndRotation(location.VectorLocation.ToVector3(), Quaternion.Euler(0, 0, 0));
        modelTransform = transform.GetChild(0);
        modelTransform.localRotation = Quaternion.Euler(0, minionRot, 0);

        facing = location.OutDirection;
        
        Active = true;
    }

    public void Deactivate()
    {
        Active = false;
    }

    /// <summary>
    /// Get the rotation mapping to the passed direction.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetDirectionRotation(NODE_DIRECTION direction)
    {
        // Get Direction
        switch (direction)
        {
            case NODE_DIRECTION.LEFT:
                return LEFT_ROT;
            case NODE_DIRECTION.UP:
                return UP_ROT;
            case NODE_DIRECTION.RIGHT:
                return RIGHT_ROT;
            case NODE_DIRECTION.DOWN:
                return DOWN_ROT;
            default:
                throw new MovementControllerException(MOVEMENT_CONTROLLER_EXCEPTION_TYPES.BAD_ROTATION_DIRECTION);
        }
    }
}

public enum MOVEMENT_CONTROLLER_EXCEPTION_TYPES
{
    BAD_ROTATION_DIRECTION
}

public class MovementControllerException : Exception
{
    private static string GetMessage(MOVEMENT_CONTROLLER_EXCEPTION_TYPES type)
    {
        switch (type)
        {
            case MOVEMENT_CONTROLLER_EXCEPTION_TYPES.BAD_ROTATION_DIRECTION:
                return "Multi-direction passed for rotation. That shouldn't happen.";
            default:
                return string.Empty;
        }
    }

    public MovementControllerException(MOVEMENT_CONTROLLER_EXCEPTION_TYPES type) : base(GetMessage(type)) { }
}