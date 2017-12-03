using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Control build mode mouse and keyboard interactions.
/// </summary>
public class BuilderController : MonoBehaviour {
    private EventSystem _eventSystem;
    public GameObject _tileSelectorPrefab;
    public GameObject _towerQuadSelectorPrefab;

    private Vector3 lastMousePosition;
    private GameState _currentGameState;
    private int _buildLayerMask;

    private GameObject _tileSelectOverlay;

	/// <summary>
    /// Set the BuilderController's link to the GameState component on the AMasterLogicNode. There should only be one GameState object per scene.
    /// Same thing for the EventSystem. We need the event system to check for mouse clicks over the build UI buttons. Without that check we
    /// kick ourselves out of build state by clicking on the build state buttons.
    /// </summary>
	void Start () {
        _currentGameState = GameObject.FindObjectOfType<GameState>();
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_currentGameState._gameState == GameState.GAME_STATE.BUILD)
        {
            if (Input.mousePosition != lastMousePosition)
            {
                lastMousePosition = Input.mousePosition;

                Ray clickCastDirection = Camera.main.ScreenPointToRay(lastMousePosition);
                RaycastHit hit;
                //if (Physics.Raycast(clickCastDirection.origin, clickCastDirection.direction, out hit))
                if (Physics.Raycast(clickCastDirection.origin, clickCastDirection.direction, out hit, 100, 1 << _buildLayerMask))
                {
                    Debug.Log("hit " + hit.transform.name);
                    Vector3 overlayLoc = new Vector3(hit.transform.position.x, GameGlobals.TILE_SELECT_Y_OFFSET, hit.transform.position.z);
                    _tileSelectOverlay.transform.SetPositionAndRotation(overlayLoc, Quaternion.identity);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && !_eventSystem.IsPointerOverGameObject())
        {
            _currentGameState._gameState = GameState.GAME_STATE.VIEW;
            KillSelectOverlay();
        }
	}

    public void EnterBuildMode_Tower(string type)
    {
        Debug.Log(type);
        _currentGameState._gameState = GameState.GAME_STATE.BUILD;

        if (_buildLayerMask != 8 && _tileSelectOverlay != null)
        {
            KillSelectOverlay();
        }

        _buildLayerMask = 8;
        InitSelectOverlay(_towerQuadSelectorPrefab);
    }

    private void InitSelectOverlay(GameObject overlayPrefab)
    {
        _tileSelectOverlay = Instantiate(overlayPrefab, GameGlobals.TILE_SELECT_INIT_POS, Quaternion.identity);
    }

    private void KillSelectOverlay()
    {
        Destroy(_tileSelectOverlay);
        _tileSelectOverlay = null;
    }
}
