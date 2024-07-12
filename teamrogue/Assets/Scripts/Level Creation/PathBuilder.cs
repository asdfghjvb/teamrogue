using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PathBuilder
{
    const float nodeHalfDiagonal = 0.5f;

    List<LevelGenerator.Room> rooms;
    Vector3 gridOrigin;
    Vector2 areaSize;

    public Grid grid;

    public enum NodeType
    {
        empty,
        room,
        hallway
    }

    public class Node
    {
        public Vector3 pos;
        public NodeType type { get; set; }

        public int gCost;
        public int hCost;

        public Node(Vector3 _pos, NodeType _type)
        {
            pos = _pos;
            type = _type;
        }

        public Node(Vector3 _pos)
        {
            pos = _pos;
            type = NodeType.empty;
        }

        public int FCost()
        {
            return gCost + hCost;
        }
    }

    public class Grid
    {
        Node[,] nodes;

        List<LevelGenerator.Room> rooms;

        Vector3 origin;
        Vector2 size;

        int nodeCountX, nodeCountY;

        public Grid(Vector3 _gridOrigin, Vector2 _size, List<LevelGenerator.Room> _rooms)
        {
            origin = _gridOrigin;
            size = _size;
            rooms = _rooms;

            nodes = CreateGrid();
        }

        public Node[,] CreateGrid()
        {
            float nodeSize = nodeHalfDiagonal * 2;
            nodeCountX = Mathf.RoundToInt(size.x / nodeSize);
            nodeCountY = Mathf.RoundToInt(size.y / nodeSize);

            Node[,] grid = new Node[nodeCountX, nodeCountY];

            for(int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 worldPos = origin + Vector3.right * (x * nodeSize + nodeHalfDiagonal) + Vector3.forward * (y * nodeSize + nodeHalfDiagonal);

                    Node temp = new Node(worldPos);

                    foreach(LevelGenerator.Room room in rooms)
                    {//check if node is inside a room
                        if (room.rect.Contains(new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y))))
                            temp.type = NodeType.room;
                    }

                    grid[x, y] = temp;
                }
            }

            return grid;
        }

        public Node FindNode(Vector3 pos)
        {
            float percentX = (pos.x + size.x / 2) / size.x;
            float percentY = (pos.z + size.y / 2) / size.y;
            
            //use clamp to avoid out of bounds
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((nodeCountX - 1) * percentX);
            int y = Mathf.RoundToInt((nodeCountY - 1) * percentY);

            return nodes[x, y];
        }

        public void DebugDrawGrid()
        {
            int nodeCountX = nodes.GetLength(0);
            int nodeCountY = nodes.GetLength(1);

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Node node = nodes[x, y];
                    Vector3 worldPos = node.pos;
                    UnityEngine.Color color = UnityEngine.Color.white;

                    switch (node.type)
                    {
                        case NodeType.empty:
                            color = UnityEngine.Color.white;
                            break;
                        case NodeType.room:
                            color = UnityEngine.Color.red;
                            break;
                        case NodeType.hallway:
                            color = UnityEngine.Color.blue;
                            break;
                    }

                    // Calculate the corners of the square
                    Vector3 topLeft = worldPos + new Vector3(-nodeHalfDiagonal, 0, nodeHalfDiagonal);
                    Vector3 topRight = worldPos + new Vector3(nodeHalfDiagonal, 0, nodeHalfDiagonal);
                    Vector3 bottomRight = worldPos + new Vector3(nodeHalfDiagonal, 0, -nodeHalfDiagonal);
                    Vector3 bottomLeft = worldPos + new Vector3(-nodeHalfDiagonal, 0, -nodeHalfDiagonal);

                    // Draw the square
                    Debug.DrawLine(topLeft, topRight, color);
                    Debug.DrawLine(topRight, bottomRight, color);
                    Debug.DrawLine(bottomRight, bottomLeft, color);
                    Debug.DrawLine(bottomLeft, topLeft, color);
                }
            }
        }
    }

    public PathBuilder(Vector3 _gridOrigin, Vector2 _areaSize, List<LevelGenerator.Room> _rooms)
    {
        gridOrigin = _gridOrigin;
        areaSize = _areaSize;
        rooms = _rooms;

        grid = new Grid(_gridOrigin, _areaSize, _rooms);
    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.FindNode(startPos);
        Node endNode = grid.FindNode(endPos);

        List<Node> open = new();
        HashSet<Node> closed = new();

        open.Add(startNode);

        while(open.Count > 0)
        {
            Node temp = open[0];

            for(int i = 1; i < open.Count; i++)
            {
                ////////////////////////////
            }
        }
    }

}
