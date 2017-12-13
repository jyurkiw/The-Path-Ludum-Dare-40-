using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Control build mode mouse and keyboard interactions.
/// </summary>
public class BuilderController : MonoBehaviour {
    private EventSystem _eventSystem;
    private ChunkManager _chunkManager;
    
    private GameState _currentGameState;
    private int _buildLayerMask;
    private string _buildType = null;

    private GameObject _buildPositionMarker;

    private Dictionary<string, GameObject> _structurePrefabs;

	/// <summary>
    /// Set the BuilderController's link to the GameState component on the AMasterLogicNode. There should only be one GameState object per scene.
    /// Same thing for the EventSystem. We need the event system to check for mouse clicks over the build UI buttons. Without that check we
    /// kick ourselves out of build state by clicking on the build state buttons.
    /// </summary>
	void Start () {
        _currentGameState = GetComponent<GameState>();
        _eventSystem = GameObject.FindObjectOfType<EventSystem>();
        //_chunkManager = GetComponent<ChunkManager>();

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
                Vector3 overlayLoc = hit.point.ToVector3Int();

                // Check for existance of the build position marker (transparent tower gameobject) and instantiate if necessary
                if (_buildPositionMarker == null)
                {
                    _buildPositionMarker = Instantiate<GameObject>(_structurePrefabs[GameGlobals.TOWER_PLACEMENT_KEY]);
                }

                _buildPositionMarker.transform.SetPositionAndRotation(overlayLoc, Quaternion.identity);
            }

            // Build the tower and exit build mode
            if (Input.GetMouseButtonUp(0) && !_eventSystem.IsPointerOverGameObject())
            {
                _currentGameState._gameState = GameState.GAME_STATE.VIEW;
                BuildStructure(hit, _buildType);
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
        _buildLayerMask = GameGlobals.TERRAIN_MASK;
    }

    /// <summary>
    /// Kill the current select overlay gameobject.
    /// </summary>
    private void KillSelectOverlay()
    {
        Destroy(_buildPositionMarker);
    }

    /// <summary>
    /// Place the current build structure in the world.
    /// </summary>
    public GameObject BuildStructure(RaycastHit hit, string structureName)
    {
        Vector3 buildPosition = hit.point.ToVector3Int();

        return Instantiate<GameObject>(_structurePrefabs[structureName], buildPosition, Quaternion.identity);
    }

    /// <summary>
    /// Place the passed build structure in the world.
    /// </summary>
    /// <param name="buildPosition">The position to build at.</param>
    /// <param name="structureName">The structure to build.</param>
    /// <returns>The structure built.</returns>
    public GameObject BuildStructure(Vector2Int buildPosition, string structureName)
    {
        return Instantiate<GameObject>(_structurePrefabs[structureName], buildPosition.ToVector3(), Quaternion.identity);
    }
}
