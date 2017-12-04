using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class TerrainChunk : MonoBehaviour {
    private GameObject[,] tiles;
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
        builtChunk = false;
        _chunkMap = null;
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

        builtChunk = true;
        tiles = new GameObject[GameGlobals.CHUNK_SIZE, GameGlobals.CHUNK_SIZE];

        for (int row = 0; row < GameGlobals.CHUNK_SIZE; row++)
        {
            for (int col = 0; col < GameGlobals.CHUNK_SIZE; col++)
            {
                char idxChar = _chunkMap[row].ToUpper()[col];
                int tileIndex;

                if (idxChar >= '0' && idxChar <= '9')
                {
                    tileIndex = int.Parse(idxChar.ToString());
                }
                else
                {
                    tileIndex = 10 + (int)(idxChar - 'A');
                }

                Assert.IsTrue(tileIndex <= _tileMaterials.Length, "You forgot to add a prefab to the tile material list. Found " + tileIndex + " with max of " + (_tileMaterials.Length - 1));
                
                GameObject tile = Instantiate<GameObject>(_tilePrefab);
                tile.transform.parent = transform;
                tile.transform.localPosition = GameUtils.GetTilePosAt(col, row);
                tile.transform.rotation = Quaternion.identity;
                tiles[row, col] = tile;


                if (idxChar > 0)
                {
                    tiles[row, col].GetComponent<Renderer>().material = _tileMaterials[tileIndex];
                }
            }
        }
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
        return tiles[position.y, position.x];
    }
}
