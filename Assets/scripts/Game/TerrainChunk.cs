using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class TerrainChunk : MonoBehaviour {
    //private GameObject[,] tiles;
    public GameObject _tilePrefab;
    public Material[] _tileMaterials;

    private List<string> _chunkMap;

    public Vector2Int _id;

    /// <summary>
    /// Signifies a chunk that has executed InitChunk.
    /// </summary>
    public bool builtChunk { get; private set; }

	// Use this for initialization
	void Start () {
        //builtChunk = false;
        //_chunkMap = null;
    }

    /// <summary>
    /// Set the chunk map data. Must be called before InitChunk.
    /// </summary>
    /// <param name="chunkData"></param>
    public void SetChunkMap(string chunkData)
    {
        if (_chunkMap == null)
        {
            _chunkMap = new List<string>(chunkData.Split('\n'));
            _chunkMap.Reverse();

            Assert.AreEqual(GameGlobals.CHUNK_SIZE, _chunkMap.Count);
        }
    }

    /// <summary>
    /// Initialize a chunk.
    /// Process the passed chunk data block and instantiate the necessary tiles.
    /// </summary>
    /// <param name="_chunkData">A chunk data block.</param>
    public void InitChunk()
    {
        Assert.IsNotNull(_chunkMap);
    }

    /// <summary>
    /// Get the position of all valid start points in a chunk.
    /// Valid start points are bounded by the WELL_START_AREA global which cuts off the outer quarter of the tiles.
    /// We don't want wells to be placed on the chunk border.
    /// </summary>
    /// <returns>A list of valid start points.</returns>
    public List<Vector2Int> GetValidStartPoints()
    {
        List<Vector2Int> startPoints = new List<Vector2Int>();

        for (int row = GameGlobals.WELL_START_AREA.position.y; row < GameGlobals.WELL_START_AREA.width; row++)
        {
            for (int col = GameGlobals.WELL_START_AREA.position.x; col < GameGlobals.WELL_START_AREA.height; col++)
            {
                if (GameGlobals.VALID_WELL_TILES.Contains(_chunkMap[row][col].ToString()))
                {
                    startPoints.Add(new Vector2Int(col, row));
                }
            }
        }

        return startPoints;
    }

    /// <summary>
    /// Get the tile at the given position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GetTileAt(Vector2Int position)
    {
        //return tiles[position.y, position.x];
        return new GameObject();
    }
}
