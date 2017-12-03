using System.Collections;
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
            .ToDictionary(k => k.Name, v => Resources.Load<GameObject>(v.Path));
    }

    /// <summary>
    /// Parse prefab file lines so we can import them and keep everything straight.
    /// I've been reading that resource importing can be kind of random sometimes
    /// unless you drive it.
    /// </summary>
    private class PrefabEntry
    {
        public string Path { get; private set; }
        public string Name { get; private set; }

        public PrefabEntry(string line)
        {
            int space = line.IndexOf(' ');
            Path = line.Substring(0, space).Trim();
            Name = line.Substring(space).Trim();
        }
    }
}