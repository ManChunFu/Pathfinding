using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DijkstraAI _dijkstraAI;
    [SerializeField] private AStarAI _aStarAI;
    [SerializeField] private Player _player;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private Texture2D _hand;
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private Transform _seedParent;

    public bool HasTested = false;

    private void Awake()
    {
        Assert.IsNotNull(_dijkstraAI, "No reference to DijkstraAI script.");

        Assert.IsNotNull(_aStarAI, "No reference to AStarAI script.");

        Assert.IsNotNull(_player, "No reference to Player script.");

        Assert.IsNotNull(_uiManager, "No reference to UIManager script.");

        Assert.IsNotNull(_hand, "No reference to Hand image.");

        if (_nodeParent == null)
            _nodeParent = GameObject.Find("Nodes").GetComponent<Transform>();
        Assert.IsNotNull(_nodeParent, "No reference to Nodes transform.");

        if (_seedParent == null)
            _seedParent = GameObject.Find("Seeds").GetComponent<Transform>();
        Assert.IsNotNull(_seedParent, "No referencet to Seeds transform.");
    }

    private void Start()
    {
        Cursor.SetCursor(_hand, Vector2.zero, CursorMode.Auto);

        _aStarAI.enabled = false;
        _dijkstraAI.enabled = false;
    }

    private void Update()
    {
        if (_dijkstraAI.NoPathFound || _aStarAI.NoPathFound )
            _uiManager.ActivateNoWayPanel();
    }

    private void CleanPathFinding()
    {
        foreach (Transform item in _nodeParent)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in _seedParent)
        {
            Destroy(item.gameObject);
        }
    }

    public void ActivateDijkstraAIScript()
    {
        if (HasTested)
            CleanPathFinding();

        _player.SetAIPathSource(PathFindingWays.DijkstraAI);

        _dijkstraAI.enabled = true;
        _aStarAI.enabled = false;
        _uiManager.StonePickedUp = false;
    }

    public void ActivateAStarAIScript()
    {
        if (HasTested)
            CleanPathFinding();

        _player.SetAIPathSource(PathFindingWays.AstarAI);

        _dijkstraAI.enabled = false;
        _aStarAI.enabled = true;
        _uiManager.StonePickedUp = false;
    }

    public void CleanUpScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitScene()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

