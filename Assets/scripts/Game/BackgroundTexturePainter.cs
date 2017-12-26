using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BackgroundTexturePainter : MonoBehaviour
{
    private Queue<TerrainChunk> ChunksToPaint = new Queue<TerrainChunk>();
    private bool IsPainterReady = true;
    private TerrainChunk chunkOnEasel = null;

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <param name="chunkBlueprint"></param>
    /// <returns></returns>
    public IEnumerator PaintChunkTexture(string chunkBlueprint)
    {
        Texture2D chunkTexture = ChunkTexturePainter.ChunkTextureFactory();
        List<string> chunkMap = GameUtils.MapFromBlueprint(chunkBlueprint);

        for (int row = 0; row < GameGlobals.CHUNK_SIZE; row++)
        {
            for (int col = 0; col < GameGlobals.CHUNK_SIZE; col++)
            {
                string tileCode = ChunkTexturePainter.GetTileCode(row, col, chunkMap);
                Assert.IsTrue(ChunkTexturePainter.TEXTURE_MAP.ContainsKey(tileCode), string.Format("{0} was not in the dict", tileCode));
                Texture2D tileTexture = ChunkTexturePainter.TEXTURE_MAP[tileCode];
                ChunkTexturePainter.CopyTileToChunk(ref chunkTexture, tileTexture, row, col);

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