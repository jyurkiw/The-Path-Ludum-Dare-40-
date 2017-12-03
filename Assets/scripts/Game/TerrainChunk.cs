using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class TerrainChunk : MonoBehaviour {
    private GameObject[,] tiles;
    public GameObject[] _tilePrefabs;

	// Use this for initialization
	void Start () {
		tiles = new GameObject[GameGlobals.CHUNK_SIZE, GameGlobals.CHUNK_SIZE];
        TextAsset chunkFile = Resources.Load("chunks") as TextAsset;

        List<string> chunkLines = new List<string>(chunkFile.text.Split('\n'));
        chunkLines.Reverse();
        
        Assert.AreEqual(GameGlobals.CHUNK_SIZE, chunkLines.Count);

        for (int row = 0; row < GameGlobals.CHUNK_SIZE; row++)
        {
            for (int col = 0; col < GameGlobals.CHUNK_SIZE; col++)
            {
                char idxChar = chunkLines[row].ToUpper()[col];
                int tileIndex;

                if (idxChar >= '0' && idxChar <= '9')
                {
                    tileIndex = int.Parse(idxChar.ToString());
                } else
                {
                    tileIndex = 10 + (int)(idxChar - 'A');
                }

                Assert.IsTrue(tileIndex < _tilePrefabs.Length, "You forgot to add a prefab to the chunk prefab tile list");
                tiles[row, col] = Instantiate(_tilePrefabs[tileIndex], GameUtils.GetTilePosAt(col, row), Quaternion.identity, transform);
            }
        }
    }
}
