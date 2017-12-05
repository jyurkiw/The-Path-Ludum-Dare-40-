using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Paints procedural textures for the world map.
/// 
/// IMPORTANT NOTE: 0,0 is the lower left corner of the texture. This is not like pixel coordinates on a screen or terminal!!!
/// All chunk blueprint strings assume [0][0] is lower-left corner.
/// </summary>
public static class ChunkTexturePainter
{
    public static readonly Dictionary<string, Texture2D> TEXTURE_MAP;
    public static readonly int CHUNK_SIZE = GameGlobals.CHUNK_SIZE * GameGlobals.TILE_SZIE;
    private const string TILE_CODE_FORMAT = "{0},{1},{2}";
    private const char DEFAULT_TILE_CODE = 'X';
    private const string DEFAULT_TILE_ROW = "XXX";
    
    /// <summary>
    /// Initialize a chunk texture
    /// </summary>
    /// <param name="chunkBlueprint"></param>
	static ChunkTexturePainter()
    {
        TEXTURE_MAP = GameUtils.LoadResourceTextures("chunk_textures");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chunkBlueprint"></param>
    /// <returns></returns>
    public static Texture2D PaintChunkTexture(string chunkBlueprint)
    {
        Texture2D chunkTexture = new Texture2D(CHUNK_SIZE, CHUNK_SIZE);
        List<string> chunkMap = MapFromBlueprint(chunkBlueprint);
        
        for (int row = 0; row < GameGlobals.CHUNK_SIZE; row++)
        {
            for (int col = 0; col < GameGlobals.CHUNK_SIZE; col++)
            {
                string tileCode = GetTileCode(row, col, chunkMap);
                Assert.IsTrue(TEXTURE_MAP.ContainsKey(tileCode), string.Format("{0} was not in the dict", tileCode));
                Texture2D tileTexture = TEXTURE_MAP[tileCode];
                CopyTileToChunk(ref chunkTexture, tileTexture, row, col);
            }
        }

        chunkTexture.Apply();
        return chunkTexture;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chunkTexture"></param>
    /// <param name="tileTexture"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    public static void CopyTileToChunk(ref Texture2D chunkTexture, Texture2D tileTexture, int row, int col)
    {
        for (int sourceX = 0, destX = col * tileTexture.width; sourceX < tileTexture.width; sourceX++, destX++)
        {
            for (int sourceY = 0, destY = row * tileTexture.height; sourceY < tileTexture.height; sourceY++, destY++)
            {
                chunkTexture.SetPixel(destX, destY, tileTexture.GetPixel(sourceX, sourceY));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static string GetTileCode(int row, int col, List<string> map)
    {
        if (map[row][col] == 'G') return "G";
        if (map[row][col] == 'C') return "C";
        if (map[row][col] == 'I') return "I";
        if (map[row][col] == 'M') return "M";

        string code = string.Empty;
        if (row - 1 >= 0 && map[row - 1][col] == 'P') code += 'N';
        if (row + 1 < map.Count && map[row + 1][col] == 'P') code += 'S';

        try
        {
            if (col + 1 < map[row].Length && map[row][col + 1] == 'P') code += 'E';
            if (col - 1 >= 0 && map[row][col - 1] == 'P') code += 'W';
        }
        catch (Exception)
        {
            Debug.Log(string.Format("Row: {0}", row));
            Debug.Log(string.Format("Map.Count: {0}", map.Count));
            Debug.Log(string.Format("Col + 1: {0}", col + 1));
            Debug.Log(string.Format("String Length: {0}", map[row].Length));
            try
            {
                Debug.Log(string.Format("Target Character: {0}", map[row][col + 1]));
            }
            catch (Exception)
            {
                Debug.Log("FUUUUUUUUUUUUU");
            }
        }

        if (code.CompareTo("N") == 0 || code.CompareTo("S") == 0) code = "NS";
        if (code.CompareTo("E") == 0 || code.CompareTo("W") == 0) code = "EW";

        return code;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="blueprint"></param>
    /// <returns></returns>
    public static List<string> MapFromBlueprint(string blueprint)
    {
        return new List<string>(blueprint.Split('\n').Select(x => x.TrimEnd()));
    }
}