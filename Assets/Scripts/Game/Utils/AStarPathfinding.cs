using System.Collections.Generic;
using UnityEngine;

// This a star implementation should works well with our BoardCharacter 2D array
public class AStarPathfinding
{
    private readonly BoardObject[,] _grid;
    private readonly int _width;
    private readonly int _height;

    public AStarPathfinding(BoardObject[,] grid)
    {
        _grid = grid;
        _width = grid.GetLength(0);
        _height = grid.GetLength(1);
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Node startNode = new Node(start, null);
        Node endNode = new Node(end, null);
        startNode.GCost = 0; // Set start node GCost to 0

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || 
                    (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Position == endNode.Position)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor) || !IsWalkable(neighbor.Position)) continue;

                int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, endNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        // Path not found
        return null;
    }


    private List<Vector2Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.Position);
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var direction in directions)
        {
            Vector2Int neighborPos = node.Position + direction;
            if (IsInBounds(neighborPos))
            {
                neighbors.Add(new Node(neighborPos, node));
            }
        }

        return neighbors;
    }

    private bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < _width && position.y >= 0 && position.y < _height;
    }

    private bool IsWalkable(Vector2Int position)
    {
        return _grid[position.x, position.y] == null; 
    }

    private int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.Position.x - b.Position.x);
        int dstY = Mathf.Abs(a.Position.y - b.Position.y);
        return dstX + dstY;
    }

    private class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public int GCost; 
        public int HCost; 
        public int FCost => GCost + HCost; 

        public Node(Vector2Int position, Node parent)
        {
            Position = position;
            Parent = parent;
            GCost = int.MaxValue;
            HCost = 0;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Position.Equals(node.Position);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

}
