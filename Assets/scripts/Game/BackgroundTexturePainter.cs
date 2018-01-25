using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BackgroundTexturePainter : MonoBehaviour
{
    private Queue<TerrainChunk> ChunksToPaint = new Queue<TerrainChunk>();
    private bool IsPainterReady = true;
    private TerrainChunk chunkOnEasel = null;

    private PathBuilder _pathBuilder = null;

    public void Start()
    {
        _pathBuilder = GetComponent<PathBuilder>();
    }

    /// <summary>
    /// Painting starts here.
    /// </summary>
    private void Update()
    {
        if (ChunksToPaint.Count > 0 && IsPainterReady)
        {
            IsPainterReady = false;
            chunkOnEasel = ChunksToPaint.Dequeue();
            IEnumerator routine = PaintChunkTexture(chunkOnEasel.ChunkMap);
            StartCoroutine(routine);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chunk"></param>
    public void EnqueueChunk(GameObject chunk)
    {
        ChunksToPaint.Enqueue(chunk.GetComponent<TerrainChunk>());
    }

    /// <summary>
    /// TODO: Change to use SetPixels rather than setPixel. The apply that follows should be a lot faster.
    /// </summary>
    /// <param name="chunkBlueprint"></param>
    /// <returns></returns>
    public IEnumerator PaintChunkTexture(string chunkBlueprint)
    {
        Texture2D chunkTexture = ChunkTexturePainter.ChunkTextureFactory();

        for (int row = 0; row < GameGlobals.CHUNK_SIZE; row++)
        {
            for (int col = 0; col < GameGlobals.CHUNK_SIZE; col++)
            {
                string tileCode = _pathBuilder._path.GetNodesAt(col, row).GetPathEntrancesAndExits().ToString();

                Assert.IsTrue(ChunkTexturePainter.TEXTURE_MAP.ContainsKey(tileCode), string.Format("{0} was not in the dict", tileCode));
                Texture2D tileTexture = ChunkTexturePainter.TEXTURE_MAP[tileCode];

                // CHUNK_SIZE - row - 1 offset note
                // row is being offset twice. The first flips the orientation of the Y axis because while we are using standard cartesian coordinates,
                // textures are drawn from the top-left to the bottom-right of the texture in a similar manner to screen consoles and 2d texture maps.
                // Considering a texture IS a 2d texture, this is hardly surprising. Therefore, we must flip the Y axis or else everything is drawn
                // upside-down. The additonal -1 is the array offset. Otherwise we start by trying to write to row CHUNK_SIZE when the last writable
                // row is actually CHUNK_SIZE - 1.
                ChunkTexturePainter.CopyTileToChunk(ref chunkTexture, tileTexture, GameGlobals.CHUNK_SIZE - row - 1, col);

                if (col % (int)(GameGlobals.CHUNK_SIZE / GameGlobals.TERRAIN_YIELD_THRESHOLD) == 0)
                {
                    yield return 0;
                }
            }
        }

        chunkTexture.Apply();
        chunkOnEasel.Renderer.material.mainTexture = chunkTexture;
        IsPainterReady = true;
        yield break;
    }
}