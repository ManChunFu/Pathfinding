using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _stonePrefab;
    [SerializeField] private Transform _stoneParent;
    [SerializeField] private GameObject _noWayPanel;

    public bool StonePickedUp;

    const float minX = -7f, maxX = 7f, minY = -4f, maxY = 3f;

    private void Awake()
    {
        Assert.IsNotNull(_stonePrefab, "No reference to Stone prefab.");

        Assert.IsNotNull(_stoneParent, "No reference to Obstacles game object.");

        Assert.IsNotNull(_noWayPanel, "No reference to No_Way_Panel.");
    }

    private void Start()
    {
        _noWayPanel.SetActive(false);
    }

    private void Update()
    {
        if (StonePickedUp)
        {
            if (Input.GetMouseButtonDown(0))
                PlaceStoneOnScene();
        }
    }

    public void ActivateNoWayPanel()
    {
        _noWayPanel.SetActive(true);
    }

    public void PickUpStone()
    {
        StonePickedUp = true;
    }

    private void PlaceStoneOnScene()
    {
        Vector3 placePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var roundedX = Mathf.Round(placePos.x);
        var roundedY = placePos.y < 0 ? (int)placePos.y - 1 : (int)placePos.y;
        placePos = new Vector3(roundedX, roundedY, 0f);

        if ((placePos.x >= minX && placePos.x <= maxX) && (placePos.y >= minY && placePos.y <= maxY))
            Instantiate(_stonePrefab, placePos, Quaternion.identity, _stoneParent);

    }
}
