using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils {
    public enum POS { UPPER_LEFT, UPPER_RIGHT, LOWER_LEFT, LOWER_RIGHT };

    /// <summary>
    /// Get a tower position on a tile based on the tile position enum passed.
    /// Towers are less than a quarter of the size of a tile, and so they go into one of the four corners rather than in the center.
    /// Tower position is based on local position rather than world position. Towers should be made children of a tile parent (this makes positioning simple).
    /// </summary>
    /// <param name="tilePos">The postion of the tower on the tile</param>
    /// <returns>The correct Vector3 position.</returns>
    public static Vector3 GetTowerPos(POS tilePos)
    {
        switch(tilePos)
        {
            case POS.UPPER_LEFT:
                return new Vector3(-GameGlobals.TOWER_POSITION_OFFSET, GameGlobals.TOWER_Y_OFFSET, GameGlobals.TOWER_POSITION_OFFSET);
            case POS.UPPER_RIGHT:
                return new Vector3(GameGlobals.TOWER_POSITION_OFFSET, GameGlobals.TOWER_Y_OFFSET, GameGlobals.TOWER_POSITION_OFFSET);
            case POS.LOWER_LEFT:
                return new Vector3(-GameGlobals.TOWER_POSITION_OFFSET, GameGlobals.TOWER_Y_OFFSET, GameGlobals.TOWER_POSITION_OFFSET);
            case POS.LOWER_RIGHT:
                return new Vector3(GameGlobals.TOWER_POSITION_OFFSET, GameGlobals.TOWER_Y_OFFSET, -GameGlobals.TOWER_POSITION_OFFSET);
            default:
                throw new System.Exception("Invalid tower position value passed");
        }
    }

    public static Vector3 GetTilePosAt(int x, int y)
    {
        return new Vector3((float)x, GameGlobals.TILE_Y_OFFSET, (float)y);
    }
}
