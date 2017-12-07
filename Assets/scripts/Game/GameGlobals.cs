using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobals {
    public const float CAMERA_MOVE_SPEED = 0.25f;
    public const float CAMERA_MOUSE_MIN_MOVE_SPEED = 0.05f;
    public const float CAMERA_INIT_POSITION_Z_OFFSET = -2f;

    public const string CHUNK_PREFAB_NAME = "prefabs/chunk";
    public const int CHUNK_SIZE = 32;
    public const float CHUNK_TILE_Y_POS = 0f;
    
    public const string CHUNK_PLANE_PREFAB_RESOURCE_NAME = "prefabs/chunk_plane";

    public const string DEFAULT_GRASS_KEY = "XXX,XXX,XXX";

    public const int TERRAIN_MASK = 8;
    public const int TERRAIN_YIELD_THRESHOLD = 8;

    public const string TOWER_PREFAB_FILE = "towers";
    public const string TOWER_PLACEMENT_KEY = "tower_placement";
    public const string TOWER_SELECT_KEY = "tower_select";
    
    public const float TILE_Y_OFFSET = 0f;

    public const float TILE_SELECT_Y_OFFSET = 0.01f;
    public static readonly string TILE_SELECT_INIT_NAME = "TileSelectionOverlay";
    public const int TILE_SZIE = 64;

    public const string VALID_WELL_TILES = "123456";
    public const string WELL_PREFAB_NAME = "player_well";
    public static readonly RectInt WELL_START_AREA = new RectInt((int)(CHUNK_SIZE / 4), (int)(CHUNK_SIZE / 4), (int)(CHUNK_SIZE / 2), (int)(CHUNK_SIZE / 2));
}