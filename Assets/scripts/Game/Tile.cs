using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public enum DIRECTION { ENTRANCE, EXIT, BLOCKED, OPEN };

    public DIRECTION north = DIRECTION.OPEN;
    public DIRECTION south = DIRECTION.OPEN;
    public DIRECTION east = DIRECTION.OPEN;
    public DIRECTION west = DIRECTION.OPEN;
}
