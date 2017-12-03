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

	// Use this for initialization
	public void Start () {
        // Load the chunk prefab
        _chunkPrefab = Resources.Load<GameObject>(GameGlobals.CHUNK_PREFAB_NAME);

        // Init the first chunk
        _chunks.Add(Vector2Int.zero, Instantiate<GameObject>(_chunkPrefab, Vector3.zero, Quaternion.identity).GetComponent<TerrainChunk>());
        _chunks[Vector2Int.zero].InitChunk(Resources.Load<TextAsset>("chunks").text);
	}
	
	// Update is called once per frame
	public void Update () {
		
	}
}
