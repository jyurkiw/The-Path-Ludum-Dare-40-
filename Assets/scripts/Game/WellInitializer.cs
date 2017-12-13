using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initialize the Player's Well so that they can build more towers.
/// Initialization means figuring out where to place the well on the path in the central,
/// initial chunk.
/// </summary>
public class WellInitializer : MonoBehaviour {
	// Use this for initialization
	void Start () {
        BuilderController buildController = GetComponentInParent<BuilderController>();
        //CameraController gimble = FindObjectOfType<CameraController>();

        GameObject well = buildController.BuildStructure(new Vector2Int((int)(GameGlobals.CHUNK_SIZE / 2), (int)(GameGlobals.CHUNK_SIZE / 2)), GameGlobals.WELL_PREFAB_NAME);
    }
}
