﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameUtils {
    public enum POS { UPPER_LEFT, UPPER_RIGHT, LOWER_LEFT, LOWER_RIGHT };

    /// <summary>
    /// Get the tile position at the passed xy coord.
    /// This translates a plain xy coord into a Vector3 and deals with Unity's weird shit.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector3 GetTilePosAt(int x, int y)
    {
        return new Vector3((float)x, GameGlobals.TILE_Y_OFFSET, (float)y);
    }

    /// <summary>
    /// Parse a resource prefab list file.
    /// File format is:
    ///     One prefab per line
    ///     Each line follows the following format: path/to/prefab/in/resources name
    ///     Only use one space, or else the split call is going to choke.
    ///     
    /// NOTE: Only use linq during Start() functions. Linq has a bit of overhead that
    /// simple looping doesn't have.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static Dictionary<string, GameObject> LoadResourcePrefabs(string filename)
    {
        TextAsset prefabTextAsset = Resources.Load(filename) as TextAsset;

        return new List<string>(prefabTextAsset.text.Split('\n'))
            .Select(l => new PrefabEntry(l))
            .ToDictionary(k => k.Code, v => Resources.Load<GameObject>(v.Path));
    }

    public static Dictionary<string, Texture2D> LoadResourceTextures(string filename)
    {
        TextAsset prefabTextAsset = Resources.Load(filename) as TextAsset;

        return new List<string>(prefabTextAsset.text.Split('\n'))
            .Where(l=>l[0] != '#')
            .Select(l => new PrefabEntry(l))
            .ToDictionary(k => k.Code, v => Resources.Load<Texture2D>(v.Path));
    }

    /// <summary>
    /// Parse prefab file lines so we can import them and keep everything straight.
    /// I've been reading that resource importing can be kind of random sometimes
    /// unless you drive it.
    /// </summary>
    private class PrefabEntry
    {
        public string Path { get; private set; }
        public string Code { get; private set; }

        public PrefabEntry(string line)
        {
            int space = line.IndexOf(' ');
            Path = line.Substring(0, space).Trim();
            Code = line.Substring(space).Trim();
        }
    }
}

public static class Extensions
{
    /// <summary>
    /// Change a vector 3 to an integer vector 3.
    /// This just drops decimal place data and converts floats to ints.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3Int ToVector3Int(this Vector3 vec)
    {
        return new Vector3Int((int)vec.x, (int)vec.y, (int)vec.z);
    }

    /// <summary>
    /// Change a vector 3 to an integer vector 3.
    /// This rounds all float values before casting floats to ints.
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3Int RoundToVector3Int(this Vector3 vec)
    {
        return new Vector3Int((int)Mathf.Round(vec.x), (int)Mathf.Round(vec.y), (int)Mathf.Round(vec.z));
    }

    /// <summary>
    /// Change a Vector 3 to an integer vector 2.
    /// This rounds all float values before casting floats to ints and discards the y value (z becomes y).
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector2Int RoundToVector2Int(this Vector3 vec)
    {
        return new Vector2Int((int)Mathf.Round(vec.x), (int)Mathf.Round(vec.z));
    }

    /// <summary>
    /// Change a vector 2 int to a vector3.
    /// x => x
    /// y = 0
    /// y => z
    /// 
    /// </summary>
    /// <param name="vec">The vector2 to translate.</param>
    /// <returns>A vector3</returns>
    public static Vector3 ToVector3(this Vector2Int vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    /// <summary>
    /// Change a vector 3 int to a vector 2 int.
    /// x => x
    /// z => y
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector2Int ToVector2Int(this Vector3Int vec)
    {
        return new Vector2Int(vec.x, vec.z);
    }
}