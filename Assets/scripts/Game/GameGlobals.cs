﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobals {
    public const int BUILD_TOWER_LAYER_MASK = 8;

    public const float CAMERA_MOVE_SPEED = 0.25f;
    public const float CAMERA_MOUSE_MIN_MOVE_SPEED = 0.05F;

    public const string CHUNK_PREFAB_NAME = "prefabs/chunk";
    public const int CHUNK_SIZE = 32;

    public const string TOWER_PREFAB_FILE = "towers";
    
    public const float TILE_Y_OFFSET = 0f;

    public const float TILE_SELECT_Y_OFFSET = 0.01f;
    public static readonly string TILE_SELECT_INIT_NAME = "TileSelectionOverlay";

    public const string VALID_WELL_TILES = "123456";
    public const string WELL_PREFAB_NAME = "player_well";
    public static readonly RectInt WELL_START_AREA = new RectInt((int)(CHUNK_SIZE / 4), (int)(CHUNK_SIZE / 4), (int)(CHUNK_SIZE / 2), (int)(CHUNK_SIZE / 2));
}