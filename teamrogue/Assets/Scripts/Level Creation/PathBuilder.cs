using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class PathBuilder
{
    public const float nodeHalfDiagonal = 1f;

    /* These outline the cost of searching different conditions */
    const int diagonalSearchCost = 60; //The hallways look better if diagonal movements are heavily discourged
    const int orthogonalSearchCost = 10;

    //modifiers to encourage certain behaviors, such as moving around something or thru something else
    const float hallwayModifier = 0.2f; 
    const float roomModifier = 1.15f;

    LevelGenerator level;

    List<LevelGenerator.Room> rooms;
    Vector3 gridOrigin;
    Vector2 areaSize;

    public Grid grid;

    public enum NodeType
    {
        empty,
        room,
        hallway,
        door
    }

    public enum NodeSpawnType
    {
        active,
        inactive,
        player,
        boss
    }

    public class Node
    {
        public Vector3 pos;
       
        public NodeType type { get; set; }
        public NodeSpawnType spawnType { get; set; }

        public Vector2Int cordinates;

        public int gCost;
        public int hCost;

        public Node parent;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Node(Vector3 _pos, Vector2Int _cordinates, NodeType _type = NodeType.empty, NodeSpawnType _spawnType = NodeSpawnType.inactive)
        {
            pos = _pos;
            type = _type;
            spawnType = _spawnType;
            cordinates = _cordinates;
        }
    }

    public class Grid
    {
        public Node[,] nodes;

        List<LevelGenerator.Room> rooms;

        Vector3 origin;
        Vector2 size;

        public int nodeCountX, nodeCountY;

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
                    //Vector3 worldPos = origin + Vector3.right * (x * nodeSize + nodeHalfDiagonal) + Vector3.forward * (y * nodeSize + nodeHalfDiagonal);
                    Vector3 worldPos = origin + new Vector3(x * nodeSize + nodeHalfDiagonal, origin.y, y * nodeSize + nodeHalfDiagonal);

                    Node temp = new Node(worldPos, new Vector2Int(x,y));

                    foreach(LevelGenerator.Room room in rooms)
                    {//check if node is inside a room

                        if (room.rect.Contains(new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.z))))
                        {
                            room.nodes.Add(temp);
                            temp.type = NodeType.room;
                            temp.spawnType = NodeSpawnType.active;
                        }
                    }

                    grid[x, y] = temp;
                }
            }

            return grid;
        }

        public bool Contains(Vector2Int cordinate)
        {
            if (cordinate.x >= origin.x
                && cordinate.x < nodeCountX
                && cordinate.y >= origin.z
                && cordinate.y < nodeCountY)
                return true;
            else
                return false;
        }

        public Node FindNode(Vector3 pos)
        {
            Vector3 localPos = pos - origin;

            int x = Mathf.FloorToInt(localPos.x / (nodeHalfDiagonal * 2));
            int y = Mathf.FloorToInt(localPos.z / (nodeHalfDiagonal * 2));

            // clamp acts as a bounds check
            x = Mathf.Clamp(x, 0, nodeCountX - 1);
            y = Mathf.Clamp(y, 0, nodeCountY - 1);

            return nodes[x, y];
        }

        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new();

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0) //0,0 is itself so skip
                        continue;

                    Vector2Int checkCord = new Vector2Int(node.cordinates.x + x, node.cordinates.y + y);

                    if(Contains(checkCord))
                    { //"if bounds check is true"
                        neighbors.Add(nodes[checkCord.x, checkCord.y]);
                    }
                }
            }

            return neighbors;
        }

        public int GetDistance(Node a, Node b)
        {
            int distX = Mathf.Abs(a.cordinates.x - b.cordinates.x);
            int distY = Mathf.Abs(a.cordinates.y - b.cordinates.y);

            if (distX > distY)
                return diagonalSearchCost * distY + orthogonalSearchCost * (distX - distY);
            else
                return diagonalSearchCost * distX + orthogonalSearchCost * (distY - distX);
        }

        public void DebugDrawGrid(int gridNumber)
        {
            if (gridNumber != 1 && gridNumber != 2)
                return;

            int nodeCountX = nodes.GetLength(0);
            int nodeCountY = nodes.GetLength(1);

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Node node = nodes[x, y];
                    Vector3 worldPos = node.pos;
                    UnityEngine.Color color = UnityEngine.Color.white;

                    if (gridNumber == 1)
                    {
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
                            case NodeType.door:
                                color = UnityEngine.Color.green;
                                break;
                        }
                    }

                    if (gridNumber == 2)
                    {
                        switch (node.spawnType)
                        {
                            case NodeSpawnType.inactive:
                                color = UnityEngine.Color.red;
                                break;
                            case NodeSpawnType.active:
                                color = UnityEngine.Color.green;
                                break;
                            case NodeSpawnType.player:
                                color = UnityEngine.Color.blue;
                                break;
                            case NodeSpawnType.boss:
                                color = UnityEngine.Color.blue;
                                break;
                        }
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

    public PathBuilder(LevelGenerator gen)
    {
        level = gen;
        gridOrigin = level.generatorOrgin;
        areaSize = level.levelSize;
        rooms = level.rooms;

        grid = new Grid(gridOrigin, areaSize, rooms);
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 endPos)
    {
        List<Node> path = new();

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
                //Whichever node has the lowest f cost becomes the new node, if fCost is the same then compare the hcost
                if (open[i].fCost < temp.fCost
                    || (open[i].fCost == temp.fCost && open[i].hCost < temp.hCost))
                    temp = open[i];
            }

            open.Remove(temp);
            closed.Add(temp);

            if (temp == endNode)
            {
               path = Retrace(startNode, endNode);
               return path;
            }

            foreach(Node node in grid.GetNeighbors(temp))
            {
                if (closed.Contains(node))
                    continue;

                int newMovementCost = temp.gCost + grid.GetDistance(temp, node);

                //Apply modifiers
                if (node.type == NodeType.room)
                {
                    newMovementCost = (int)(newMovementCost * roomModifier);
                }
                else if (node.type == NodeType.hallway)
                {
                    newMovementCost = (int)(newMovementCost * hallwayModifier);
                }

                if (newMovementCost < node.gCost || !open.Contains(node))
                {
                    node.gCost = newMovementCost;
                    node.hCost = grid.GetDistance(node, endNode);
                    node.parent = temp;

                    if (!open.Contains(node))
                        open.Add(node);
                }
            }
        }

        return path; //this should never actually trigger, just clears the error
    }

    public void ExpandHallway(List<Node> path)
    {
        foreach (Node node in path)
        {
            foreach (Node neighbor in grid.GetNeighbors(node))
            {
                if (neighbor.type == NodeType.empty)
                {
                    neighbor.type = NodeType.hallway;
                    neighbor.spawnType = NodeSpawnType.active;
                }
            }
        }
    }

    List<Node> Retrace(Node startNode, Node endNode)
    {
        List<Node> path = new();
        Node temp = endNode;

        while(temp != startNode)
        {
            path.Add(temp);

            if (temp.type == NodeType.empty && temp.parent.type == NodeType.room)
            {
                temp.type = NodeType.hallway;
                temp.parent.type = NodeType.door;

                temp.spawnType = NodeSpawnType.active;
            }
            else if (temp.type == NodeType.room && temp.parent.type == NodeType.empty)
            {
                temp.type = NodeType.door;
            }
            else if (temp.type == NodeType.empty)
            {
                temp.type = NodeType.hallway;
                temp.spawnType = NodeSpawnType.active;
            }

            temp = temp.parent;
        }

        path.Reverse();
        return path;
    }
}
