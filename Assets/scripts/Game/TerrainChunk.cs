using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class TerrainChunk : MonoBehaviour {
    public Vector2Int Id
    {
        get
        {
            return new Vector2Int((int)transform.position.x, (int)transform.position.z);
        }
    }
    public Renderer Renderer { get; private set; }
    public string ChunkMap { get; set; }

    public Rect ChunkBounds
    {
        get
        {
            return new Rect(Id, new Vector2Int(GameGlobals.CHUNK_SIZE, GameGlobals.CHUNK_SIZE));
        }
    }

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
    }
}
