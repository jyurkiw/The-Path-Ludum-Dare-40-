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
    
    private GameState _currentGameState;
    private int _buildLayerMask;
    private string _buildType = null;

    private GameObject _tileSelectOverlay;
    private GameObject _tileSelector;

    private Dictionary<string, GameObject> _structurePrefabs;

	/// <summary>
    /// Set the BuilderController's link to the GameState component on the AMasterLogicNode. There should only be one GameState object per scene.
    /// Same thing for the EventSystem. We need the event system to check for mouse clicks over the build UI buttons. Without that check we
    /// kick ourselves out of build state by clicking on the build state buttons.
    /// </summary>
	void Start () {
        _currentGameState = GameObject.FindObjectOfType<GameState>();
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();

        _structurePrefabs = GameUtils.LoadResourcePrefabs(GameGlobals.TOWER_PREFAB_FILE);
	}
	
	// Update is called once per frame
	void Update () {
        if (_currentGameState._gameState == GameState.GAME_STATE.BUILD)
        {
            Ray clickCastDirection = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(clickCastDirection.origin, clickCastDirection.direction, out hit, 100, 1 << _buildLayerMask))
            {
                Vector3 overlayLoc = new Vector3(hit.transform.position.x, GameGlobals.TILE_SELECT_Y_OFFSET, hit.transform.position.z);
                if (_tileSelector == null)
                {
                    InitSelectOverlay(_tileSelectOverlay, _buildType, overlayLoc);
                }
                _tileSelector.transform.SetPositionAndRotation(overlayLoc, Quaternion.identity);
            }

            // Build the tower and exit build mode
            if (Input.GetMouseButtonUp(0) && !_eventSystem.IsPointerOverGameObject())
            {
                _currentGameState._gameState = GameState.GAME_STATE.VIEW;
                BuildStructure(hit.transform, _buildType, hit.transform.parent.parent);
                KillSelectOverlay();
            }
        }
	}

    /// <summary>
    /// Set the current state to build mode and configure the build controller
    /// to build towers.
    /// </summary>
    /// <param name="type">The name of the tower prefab to build.</param>
    public void EnterBuildMode_Tower(string type)
    {
        _buildType = type.Trim();
        _currentGameState._gameState = GameState.GAME_STATE.BUILD;
        _buildLayerMask = GameGlobals.BUILD_TOWER_LAYER_MASK;
        _tileSelectOverlay = _towerQuadSelectorPrefab;
    }

    /// <summary>
    /// Initialize the select overlay game object with the passed game object prefab
    /// </summary>
    /// <param name="overlayPrefab">The overlay prefab to instantiate.</param>
    private void InitSelectOverlay(GameObject overlayPrefab, string buildingMarkerKey, Vector3 position)
    {
        _tileSelector = new GameObject(GameGlobals.TILE_SELECT_INIT_NAME);
        _tileSelector.transform.SetPositionAndRotation(position, Quaternion.identity);

        GameObject tileOverlay = Instantiate<GameObject>(overlayPrefab, _tileSelector.transform, false);
        GameObject buildOverlay = Instantiate<GameObject>(_structurePrefabs[buildingMarkerKey], _tileSelector.transform, false);

        Renderer buildRenderer = buildOverlay.GetComponent<Renderer>();
        Color buildColor = buildRenderer.material.color;
        buildRenderer.material.color = new Color(buildColor.r, buildColor.g, buildColor.b, 0.75f);
        
    }

    /// <summary>
    /// Kill the current select overlay gameobject.
    /// </summary>
    private void KillSelectOverlay()
    {
        Destroy(_tileSelector);
    }

    /// <summary>
    /// Place the current build structure in the world.
    /// </summary>
    public GameObject BuildStructure(Transform buildTile, string structureName, Transform parentChunk)
    {
        Vector3 quadOffset = new Vector3(
            buildTile.localPosition.x * buildTile.parent.localScale.x,
            _structurePrefabs[structureName].transform.position.y,
            buildTile.localPosition.z * buildTile.parent.localScale.z
            );
        Vector3 buildPosition = buildTile.parent.localPosition + quadOffset;

        return Instantiate<GameObject>(_structurePrefabs[structureName], buildPosition, _structurePrefabs[structureName].transform.rotation, parentChunk);
    }
}
