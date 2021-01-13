using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class DijkstraAI : MonoBehaviour, IPathFinding
{
    [SerializeField] Transform _player;
    [SerializeField] Transform _target;
    [SerializeField] Vector2 _startPoint;
    [SerializeField] Vector2 _endPoint;
    [SerializeField] Transform _obstacle;
    [SerializeField] List<Vector2> ObstacleList;
    [SerializeField] Vector2[] nodeChildrenStraightLinePosition;
    [SerializeField] Vector2[] nodeChildrenDiagnoalPosition;

    [SerializeField] private GameObject _seed;
    [SerializeField] private GameObject _node;
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private Transform _seedParent;


    const float minX = 2f, maxX = 15f, minY = 1f, maxY = 8f;

    public bool NoPathFound = false;
    public List<Vector3> Path { get; set; }
    public bool PathCompleted { get; set; }

    private void OnEnable()
    {
        Assert.IsNotNull(_player, "No reference to Player.");
        if (_player != null)
            _startPoint = WorldPositionToGridPosition(_player.position);

        Assert.IsNotNull(_target, "No reference to Target");
        if (_target != null)
            _endPoint = WorldPositionToGridPosition(_target.position);

        Assert.IsNotNull(_obstacle, "No reference to Obstacle.");
        if (_obstacle != null)
        {
            ObstacleList = new List<Vector2>();
            foreach (Transform o in _obstacle.transform)
                ObstacleList.Add(WorldPositionToGridPosition(o.transform.position));
        }

        if (_nodeParent == null)
            _nodeParent = GameObject.Find("Nodes").GetComponent<Transform>();
        Assert.IsNotNull(_nodeParent, "No reference to Nodes transform.");

        if (_seedParent == null)
            _seedParent = GameObject.Find("Seeds").GetComponent<Transform>();
        Assert.IsNotNull(_seedParent, "No referencet to Seeds transform.");


        nodeChildrenStraightLinePosition = new Vector2[4]
        {
            new Vector2(-1, 0), // left
            new Vector2(0, 1), // up
            new Vector2(1, 0), // right
            new Vector2(0, -1), // down
        };

        nodeChildrenDiagnoalPosition = new Vector2[4]
        {
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(-1, -1)
        };

        Path = new List<Vector3>();

        StartCoroutine(FindDestination());
    }

    private Vector2 WorldPositionToGridPosition(Vector3 worldPos)
    {
        float gridX = worldPos.x + 9f;
        float gridY = worldPos.y + 5f;

        return new Vector2(gridX, gridY);
    }

    private Vector3 GridPositionToWorldPosition(Vector2 gridPos)
    {
        float worldX = gridPos.x - 9f;
        float worldY = gridPos.y - 5f;

        return new Vector3(worldX, worldY, 0);
    }

    private float GetGcost(Vector2 newNodePosition)
    {
        float x = Mathf.Abs(newNodePosition.x - _startPoint.x);
        float y = Mathf.Abs(newNodePosition.y - _startPoint.y);

        return (x + y);
    }

    private float GetHCost(Vector2 newNodePosition)
    {
        float x = Mathf.Abs(newNodePosition.x - _endPoint.x);
        float y = Mathf.Abs(newNodePosition.y - _endPoint.y);

        return (x + y);
    }


    private IEnumerator FindDestination()
    {
        List<Node> NodeArea = new List<Node>();
        List<Node> closestNodes;
        NodeArea.Add(new Node(_startPoint, 0, GetHCost(_startPoint), null));

        bool reachGoal = false;
        do
        {
            float minFCost = NodeArea.Where(n => !n.NodeCompleted).Min(n => n.FCost);
            closestNodes = NodeArea.FindAll(n => n.FCost == minFCost && !n.NodeCompleted);

            if (!closestNodes.Any())
                yield break;

            foreach (Node current in closestNodes)
            {
                foreach (Vector2 childPosition in nodeChildrenStraightLinePosition)
                {
                    Vector2 newNodePosition = current.Position + childPosition;
                    if (newNodePosition == _endPoint)
                    {
                        reachGoal = true;
                        Node lastNode = new Node(newNodePosition, GetGcost(newNodePosition), 0, current);
                        StartCoroutine(DrawPath(lastNode));
                    }
                    else
                    {
                        if (newNodePosition.x >= minX && newNodePosition.x <= maxX && newNodePosition.y >= minY && newNodePosition.y <= maxY)
                            if (!ObstacleList.Any(o => o == newNodePosition))
                            {
                                if (!NodeArea.Any(n => n.Position == newNodePosition))
                                {
                                    Node newNode = new Node(newNodePosition, GetGcost(newNodePosition), GetHCost(newNodePosition), current);
                                    NodeArea.Add(newNode);
                                    Instantiate(_node, GridPositionToWorldPosition(newNodePosition), Quaternion.identity, _nodeParent);
                                    yield return new WaitForSeconds(0.1f);
                                }

                            }
                    }
                }

                if (!reachGoal)
                {
                    foreach (Vector2 childPosition in nodeChildrenDiagnoalPosition)
                    {
                        Vector2 newNodePosition = current.Position + childPosition;
                        Vector2 newNodePositionX = current.Position;
                        newNodePositionX.x += childPosition.x;
                        Vector2 newNodePositionY = current.Position;
                        newNodePositionY.y += childPosition.y;


                        if (newNodePosition.x >= minX && newNodePosition.x <= maxX && newNodePosition.y >= minY && newNodePosition.y <= maxY)
                            if (!ObstacleList.Any(o => o == newNodePosition))
                            {
                                if (!(ObstacleList.Any(o => o == newNodePositionX) &&
                                        ObstacleList.Any(o => o == newNodePositionY)))
                                {
                                    if (!NodeArea.Any(n => n.Position == newNodePosition))
                                    {
                                        Node newNode = new Node(newNodePosition, GetGcost(newNodePosition), GetHCost(newNodePosition), current);
                                        NodeArea.Add(newNode);
                                        Instantiate(_node, GridPositionToWorldPosition(newNodePosition), Quaternion.identity, _nodeParent);
                                        yield return new WaitForSeconds(0.1f);
                                    }
                                }
                            }

                    }
                }
                current.NodeCompleted = true;
            }

        } while (NodeArea.Any(n => !n.NodeCompleted) && !reachGoal);

        if (!reachGoal)
            NoPathFound = true;
    }

    private IEnumerator DrawPath(Node last)
    {
        Node current = last;

        while (current.Parent != null)
        {
            Path.Add(GridPositionToWorldPosition(current.Position));
            current = current.Parent;
            yield return null;
        }

        foreach (Vector3 item in Path)
        {
            Instantiate(_seed, item, Quaternion.identity, _seedParent);
        }
        PathCompleted = true;

        enabled = false;
    }
}
