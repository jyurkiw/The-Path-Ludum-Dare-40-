using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the map chunking so we can play the game.
/// 
/// Build first map chunk so we can place the player.
/// Expand existing chunks as the player expands.
/// Simulate distant chunks so we can spawn enemies we don't yet have terrain for.
/// </summary>
public class ChunkManager : MonoBehaviour {
    public Dictionary<Vector2Int, TerrainChunk> _chunks = new Dictionary<Vector2Int, TerrainChunk>();

    private GameObject _chunkPrefab;

    private string testData;

	// Use this for initialization
	public void Start () {
        testData = Resources.Load<TextAsset>("chunks").text;

        // Load the chunk prefab
        _chunkPrefab = Resources.Load<GameObject>(GameGlobals.CHUNK_PREFAB_NAME);

        // Init the first chunk
        TerrainChunk initialChunk = AddNewChunkAt(Vector2Int.zero);
        AddNewChunkAt(Vector2Int.zero);
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

    // Translate a Vector2Int chunk position into world coordinates by setting Y to the chunk default and
    // multiplying x and y by the global chunk size.
    private Vector3 TranslatePosToWorldPos(Vector2Int pos)
    {
        return new Vector3(pos.x * GameGlobals.CHUNK_SIZE, GameGlobals.CHUNK_TILE_Y_POS, pos.y * GameGlobals.CHUNK_SIZE);
    }

    // Get the 8 positions surrounding the passed chunk.
    private List<Vector2Int> GetNeighborChunkIds(TerrainChunk chunk)
    {
        List<Vector2Int> chunkIds = new List<Vector2Int>();
        Vector2Int id = new Vector2Int();

        for (int row = chunk._id.y - 1; row <= chunk._id.y + 1; row++)
        {
            id.y = row;
            for (int col = chunk._id.x - 1; col <= chunk._id.x + 1; col++)
            {
                id.x = col;

                if (id != chunk._id)
                {
                    chunkIds.Add(new Vector2Int(id.x, id.y));
                }
            }
        }
        foreach (Vector2Int x in chunkIds) Debug.Log(x);
        return chunkIds;
    }

    /// <summary>
    /// Add a new chunk to the chunks dictionary at the given position.
    /// If a chunk at that position already exists, but has not been built,
    /// build it.
    /// </summary>
    /// <param name="pos">The position of the new chunk.</param>
    /// <returns>The chunk added to the manager.</returns>
    public TerrainChunk AddNewChunkAt(Vector2Int pos, bool addNeighbors = true)
    {
        TerrainChunk chunk;

        if (!_chunks.ContainsKey(pos))
        {
            chunk = Instantiate<GameObject>(_chunkPrefab, TranslatePosToWorldPos(pos), Quaternion.identity).GetComponent<TerrainChunk>();
            _chunks.Add(pos, chunk);
            chunk._id = pos;
            chunk.SetChunkMap(testData);
        }
        else
        {
            chunk = _chunks[pos];
            if (!chunk.builtChunk)
            {
                chunk.InitChunk();
            }
            else return chunk;
        }

        // Handle neighbor chunks
        if (addNeighbors)
        {
            foreach (Vector2Int nPos in GetNeighborChunkIds(chunk))
            {
                if (!_chunks.ContainsKey(nPos))
                {
                    TerrainChunk simChunk = AddNewChunkAt(nPos, false);
                    simChunk.SetChunkMap(testData);
                }
            }
        }

        return chunk;
    }

    /// <summary>
    /// Check adjacent chunks for simulation, and build them if necessary.
    /// </summary>
    /// <param name="pos"></param>
    public void ChunkBuildPostProcess(Vector2Int pos)
    {
        foreach (Vector2Int adjPos in GetNeighborChunkIds(_chunks[pos]))
        {
            TerrainChunk adjacentChunk = _chunks[adjPos];
            if (!adjacentChunk.builtChunk)
            {
                AddNewChunkAt(adjacentChunk._id);
            }
        }
    }
}
