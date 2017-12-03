using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    public enum GAME_STATE { VIEW, BUILD, INTERACT, INIT };

    public GAME_STATE _gameState = GAME_STATE.INIT;

    private void Start()
    {
        _gameState = GAME_STATE.VIEW;
    }
}
