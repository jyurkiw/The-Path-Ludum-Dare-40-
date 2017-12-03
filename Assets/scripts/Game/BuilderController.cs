using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control build mode mouse and keyboard interactions.
/// </summary>
public class BuilderController : MonoBehaviour {
    public GameObject _tileSelectorPrefab;

    private Vector3 lastMousePosition;
    private GameState _currentGameState;
    private GameState.GAME_STATE _lastGameState;

    private GameObject _tileSelectOverlay;

	/// <summary>
    /// Set the BuilderController's link to the GameState component on the AMasterLogicNode. There should only be one GameState object per scene.
    /// </summary>
	void Start () {
        _currentGameState = GameObject.FindObjectOfType<GameState>();
        _lastGameState = _currentGameState._gameState;
	}
	
	// Update is called once per frame
	void Update () {
        if (_lastGameState != GameState.GAME_STATE.BUILD && _currentGameState._gameState == GameState.GAME_STATE.BUILD)
        {
            _tileSelectOverlay = Instantiate(_tileSelectorPrefab, GameGlobals.TILE_SELECT_INIT_POS, Quaternion.identity);
            _lastGameState = GameState.GAME_STATE.BUILD;
        }
        else if (_lastGameState == GameState.GAME_STATE.BUILD && _currentGameState._gameState != GameState.GAME_STATE.BUILD)
        {
            Destroy(_tileSelectOverlay);
            _tileSelectOverlay = null;
            _lastGameState = _currentGameState._gameState;
        }

        if (_currentGameState._gameState == GameState.GAME_STATE.BUILD)
        {
            if (Input.mousePosition != lastMousePosition)
            {
                lastMousePosition = Input.mousePosition;

                Ray clickCastDirection = Camera.main.ScreenPointToRay(lastMousePosition);
                RaycastHit hit;
                if (Physics.Raycast(clickCastDirection.origin, clickCastDirection.direction, out hit))
                {
                    Vector3 overlayLoc = new Vector3(hit.transform.position.x, GameGlobals.TILE_SELECT_Y_OFFSET, hit.transform.position.z);
                    _tileSelectOverlay.transform.SetPositionAndRotation(overlayLoc, Quaternion.identity);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _currentGameState._gameState = GameState.GAME_STATE.VIEW;
        }
	}

    public void EnterBuildMode_Tower(string type)
    {
        Debug.Log(type);
        _currentGameState._gameState = GameState.GAME_STATE.BUILD;
    }
}
