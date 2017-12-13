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

    private BackgroundTexturePainter _backgroundTexturePainter;

	// Use this for initialization
	public void Start () {
        _backgroundTexturePainter = GetComponent<BackgroundTexturePainter>();
        testData = Resources.Load<TextAsset>("chunks").text;

        //// Load the chunk prefab
        _chunkPrefab = Resources.Load<GameObject>(GameGlobals.CHUNK_PLANE_PREFAB_RESOURCE_NAME);

        //// Init the first chunk
        //TerrainChunk initialChunk = AddNewChunkAt(Vector2Int.zero);
        //AddNewChunkAt(Vector2Int.zero);

        // Hand initialize the first chunk for testing
        GameObject chunk1 = Instantiate(_chunkPrefab);
        chunk1.GetComponent<TerrainChunk>().ChunkMap = testData;
        _backgroundTexturePainter.EnqueueChunk(chunk1);
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

        for (int row = chunk.Id.y - 1; row <= chunk.Id.y + 1; row++)
        {
            id.y = row;
            for (int col = chunk.Id.x - 1; col <= chunk.Id.x + 1; col++)
            {
                id.x = col;

                if (id != chunk.Id)
                {
                    chunkIds.Add(new Vector2Int(id.x, id.y));
                }
            }
        }
        foreach (Vector2Int x in chunkIds) Debug.Log(x);
        return chunkIds;
    }

    /// <summary>
    /// Add a new chunk to the chunks dictionary at the given position unless that chunk already exists.
    /// </summary>
    /// <param name="pos">The position of the new chunk.</param>
    /// <returns>The chunk added to the manager.</returns>
    public TerrainChunk ActivateChunkAt(Vector2Int pos)
    {
        if (!_chunks.ContainsKey(pos))
        {
            TerrainChunk chunk = CreateNewChunkAt(pos);
            _chunks.Add(pos, chunk);

            return chunk;
        }
        else
        {
            return _chunks[pos];
        }
    }

    /// <summary>
    /// Create a new chunk at the passed position.
    /// TODO: Access the procedural path generator to initialize the ChunkMap rather than test data.
    /// </summary>
    /// <param name="pos">The location of the new chunk.</param>
    /// <returns></returns>
    public TerrainChunk CreateNewChunkAt(Vector2Int pos)
    {
        TerrainChunk chunk = Instantiate<GameObject>(_chunkPrefab, TranslatePosToWorldPos(pos), Quaternion.identity).GetComponent<TerrainChunk>();
        chunk.ChunkMap = testData;

        return chunk;
    }
}
