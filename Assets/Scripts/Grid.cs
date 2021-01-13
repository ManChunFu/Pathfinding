using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask Unwalkable;
    public Vector2 GridWorldSize;
    public float nodeRadius;
    private Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GridWorldSize);

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 center = new Vector2(transform.position.x, transform.position.y);
        Vector2 worldBottomLeft = center - Vector2.right * GridWorldSize.x / 2 - Vector2.up * GridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = Physics2D.OverlapCircle(worldPoint, nodeRadius, Unwalkable);
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

}
