using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    private const int MOVE_STRAIGHT_COST = 10;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystemHex<PathNode> gridSystem;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than one instance of Pathfinding! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
    }


    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystemHex<PathNode>(width, height, cellSize,
            (GridSystemHex<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffsetDistance,
                    Vector3.up,
                    raycastOffsetDistance * 2,
                    obstaclesLayerMask))
                {
                    // There is an bstacle on this grid position
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }


    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.getHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateHeuristicDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                // Reachde the final node.
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + MOVE_STRAIGHT_COST;

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateHeuristicDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No path found!
        pathLength = 0;
        return null;
    }

    public int CalculateHeuristicDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        return Mathf.RoundToInt(MOVE_STRAIGHT_COST *
            Vector3.Distance(gridSystem.GetWorldPosition(gridPositionA), gridSystem.GetWorldPosition(gridPositionB)));
    }


    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (lowestFCostPathNode.GetFCost() > pathNodeList[i].GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }


    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }


    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        bool oddRow = gridPosition.z % 2 == 1;

        List<GridPosition> neightbourGridPositionList = new List<GridPosition>();

        if (gridPosition.x - 1 >= 0)
        {
            neightbourGridPositionList.Add(gridPosition + new GridPosition(-1, 0));

            if (!oddRow && gridPosition.z - 1 >= 0)
            {
                neightbourGridPositionList.Add(gridPosition + new GridPosition(-1, -1));
            }

            if (!oddRow && gridPosition.z + 1 < gridSystem.getHeight())
            {
                neightbourGridPositionList.Add(gridPosition + new GridPosition(-1, +1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            neightbourGridPositionList.Add(gridPosition + new GridPosition(+1, 0));

            if (oddRow && gridPosition.z - 1 >= 0)
            {
                neightbourGridPositionList.Add(gridPosition + new GridPosition(+1, -1));
            }

            if (oddRow && gridPosition.z + 1 < gridSystem.getHeight())
            {
                neightbourGridPositionList.Add(gridPosition + new GridPosition(+1, +1));
            }
        }

        if (gridPosition.z - 1 >= 0)
        {
            neightbourGridPositionList.Add(gridPosition + new GridPosition(0, -1));
        }

        if (gridPosition.z + 1 < gridSystem.getHeight())
        {
            neightbourGridPositionList.Add(gridPosition + new GridPosition(0, +1));
        }

        foreach (GridPosition gp in neightbourGridPositionList)
        {
            neighbourList.Add(GetNode(gp.x, gp.z));
        }

        return neighbourList;
    }


    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>
        {
            endNode
        };

        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();

        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
