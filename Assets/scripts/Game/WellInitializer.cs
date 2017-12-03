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
        ChunkManager chunkManager = GetComponentInParent<ChunkManager>();
        BuilderController buildController = GetComponentInParent<BuilderController>();

        buildController.BuildStructure(Vector3.zero, GameGlobals.WELL_PREFAB_NAME);
	}
}
