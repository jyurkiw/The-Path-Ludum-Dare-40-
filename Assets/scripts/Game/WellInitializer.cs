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

        // Get list of valid start points, and pick one at random
        TerrainChunk chunk = chunkManager._chunks[Vector2Int.zero];
        List<Vector2Int> startPoints = chunk.GetValidStartPoints();
        Vector2Int wellPosition = startPoints[Mathf.RoundToInt(Mathf.Clamp(Random.value * startPoints.Count, 0, startPoints.Count - 1))];
        GameObject startingTile = chunk.GetTileAt(wellPosition);
        
        buildController.BuildStructure(startingTile.transform, GameGlobals.WELL_PREFAB_NAME, chunk.transform);
	}
}
