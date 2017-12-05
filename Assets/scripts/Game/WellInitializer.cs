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
        //ChunkManager chunkManager = GetComponentInParent<ChunkManager>();
        BuilderController buildController = GetComponentInParent<BuilderController>();
        CameraController gimble = FindObjectOfType<CameraController>();

        // Get list of valid start points, and pick one at random
        //TerrainChunk chunk = chunkManager._chunks[Vector2Int.zero];
        //List<Vector2Int> startPoints = chunk.GetValidStartPoints();
        //Vector2Int wellPosition = startPoints[Mathf.RoundToInt(Mathf.Clamp(Random.value * startPoints.Count, 0, startPoints.Count - 1))];
        //GameObject startingTile = chunk.GetTileAt(wellPosition);
        
        //GameObject well = buildController.BuildStructure(startingTile.transform, GameGlobals.WELL_PREFAB_NAME, chunk.transform);

        // Move the camera to look at the well
        //Vector3 gimblePosition = new Vector3(well.transform.position.x, gimble.transform.position.y, well.transform.position.z + GameGlobals.CAMERA_INIT_POSITION_Z_OFFSET);
        //gimble.transform.SetPositionAndRotation(gimblePosition, Quaternion.identity);
	}
}
