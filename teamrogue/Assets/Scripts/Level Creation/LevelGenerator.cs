using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.AI.Navigation;
using Unity.VisualScripting;
//using UnityEditor.MemoryProfiler;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] GameObject player;
    [SerializeField] public Vector2Int levelSize;

    [Header("Rooms")]
    [SerializeField] int maxRoomQuantity;
    [SerializeField] Vector2Int minRoomSize;
    [SerializeField] Vector2Int maxRoomSize;

    [Tooltip("The maximum amount of times the program will try to seperate overlapping rooms before removing them")]
    [SerializeField] int maxSeperationTries = 30;
    [Tooltip("The minimum amount of space between each room")]
    [SerializeField] int roomSpacer = 5;

    [Tooltip("The chance that each non essential connection becomes a hallway")]
    [Range(0, 1)]
    [SerializeField] float extraHallSpawnChance = 0.15f;

    [Header("Assets")]
    [SerializeField] public GameObject[] floors;
    [SerializeField] public GameObject[] walls;
    [SerializeField] public GameObject[] doorways;

    [Header("Enemies")]
    [SerializeField] public NavMeshSurface navMeshSurface;
    
    [Space(2)]
    
    [SerializeField] public GameObject[] lowLvlEnemies;
    [Tooltip("The chance that each walkable tile will spawn a low level enemy")]
    [Range(0, 1)]
    [SerializeField] float lowLvlSpawnChance;

    [SerializeField] public GameObject[] midLvlEnemies;
    [Tooltip("The chance that each walkable tile will spawn a mid level enemy")]
    [Range(0, 1)]
    [SerializeField] float midLvlSpawnChance;


    [Header("Debug")]
    [SerializeField] int gridNumber;

    [HideInInspector]
    public Vector3Int generatorOrgin;

    [HideInInspector]
    public List<Room> rooms;

    PathBuilder pathFinder;

    public class Room
    {
        public RectInt rect;
        public List<PathBuilder.Node> nodes;

        public Room(Vector2Int pos, Vector2Int size)
        {
            rect = new RectInt(pos, size);
            nodes = new();
        }

        public bool Intersects(in Room other, int bufferAmount = 0)
        {
            if (rect.xMax + bufferAmount > other.rect.xMin - bufferAmount
                && other.rect.xMax + bufferAmount > rect.xMin - bufferAmount
                && rect.yMax + bufferAmount > other.rect.yMin - bufferAmount
                && other.rect.yMax + bufferAmount > rect.yMin - bufferAmount)
            {
                return true;
            }
            else
                return false;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return rect.Contains(new Vector2Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y)));
        }
    }

    public class Line
    {
        public Vector2 start;
        public Vector2 end;

        public float length;

        public Line(Vector2 _start, Vector2 _end)
        {
            start = _start;
            end = _end;

            length = Vector2.Distance(start, end);
        }

        public static bool operator ==(Line line1, Line line2)
        {
            if ((line1.start == line2.start && line1.end == line2.end) ||
                (line1.start == line2.end && line1.end == line2.start))
                return true;
            else
                return false;
        }

        public static bool operator !=(Line line1, Line line2)
        {
            if ((line1.start == line2.start && line1.end == line2.end) ||
                (line1.start == line2.end && line1.end == line2.start))
                return false;
            else
                return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Line)
                return false;

            Line other = (Line)obj;

            if (this == other)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                int smaller = start.GetHashCode();
                int larger = end.GetHashCode();

                if (smaller > larger)
                {
                    smaller = end.GetHashCode();
                    larger = start.GetHashCode();
                }

                hash = 17 * hash + smaller;
                hash = 17 * hash + larger;

                return hash;
            }
        }

        public bool Intersects(Line other)
        {
            if (start.Equals(other.start) || start.Equals(other.end) || end.Equals(other.start) || end.Equals(other.end))
            {
                return false;
            }

            int o1 = Orientation(start, end, other.start);
            int o2 = Orientation(start, end, other.end);
            int o3 = Orientation(other.start, other.end, start);
            int o4 = Orientation(other.start, other.end, end);

            if (o1 != o2 && o3 != o4)
                return true;

            if (o1 == 0 && OnSegment(start, other.start, end))
                return true;

            if (o2 == 0 && OnSegment(start, other.end, end))
                return true;

            if (o3 == 0 && OnSegment(other.start, start, other.end))
                return true;

            if (o4 == 0 && OnSegment(other.start, end, other.end))
                return true;

            return false;
        }

        private bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
                q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
                return true;

            return false;
        }

        private int Orientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);

            if (Mathf.Approximately(val, 0))
                return 0;

            if (val > 0)
                return 1;
            else
                return 2;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(GameManager.instance.seed);
        GameManager.instance.player = player;

        //Sets the transform to the closest int value. this makes a grid system much simpler 
        generatorOrgin = new Vector3Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.y), Mathf.CeilToInt(transform.position.z));
        transform.position = generatorOrgin;

        GenerateRooms();

        pathFinder = new PathBuilder(this);

        List<Line> possibleConnections = CreatePossibleConnections();
        List<Line> mst = MST(possibleConnections);
        List<Line> connections = AddExtraHallways(possibleConnections, mst);

        List<List<PathBuilder.Node>> paths = new();

        foreach (Line connection in connections)
        {
            paths.Add(pathFinder.FindPath(new Vector3(connection.start.x, generatorOrgin.y, connection.start.y),
            new Vector3(connection.end.x, generatorOrgin.y, connection.end.y)));
        }
        foreach (List<PathBuilder.Node> path in paths)
        {
            pathFinder.ExpandHallway(path);
        }

        BuildStructures();
        navMeshSurface.BuildNavMesh();

        /* Decorate Dungeon */

        /* Populate the dungeon */
        GameObject entitiesParentObject = new GameObject("Entities");

        SetSpawnPositions();
        SpawnEntities(entitiesParentObject);
    }

    // Update is called once per frame
    void Update()
    {
        //pathFinder.grid.DebugDrawGrid(gridNumber);
    }

    private void OnDrawGizmos()
    {
        /*
         Creates a gizmo in the scene view to visualize where the level will be built
         Step 1, draw the outer boader
        */

        Gizmos.color = Color.black;

        Vector3 bottomLeft = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x + levelSize.x, transform.position.y, transform.position.z);
        Vector3 topLeft = new Vector3(transform.position.x, transform.position.y, transform.position.z + levelSize.y);
        Vector3 topRight = new Vector3(transform.position.x + levelSize.x, transform.position.y, transform.position.z + levelSize.y);

        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }

    void SetSpawnPositions(bool setBossRoomInactive = true, bool setPlayerSpawnRoomInactive = true)
    {
        /* find the biggest and smallest rooms to be the player and boss rooms */

        Room biggest = rooms[0];
        Room smallest = rooms[0];

        foreach (Room room in rooms)
        {
            if(room.nodes.Count > biggest.nodes.Count)
            {
                biggest = room;
            }
            
            if(room.nodes.Count < smallest.nodes.Count)
            {
                smallest = room;
            }
        }

        /* Pick a random node in each room to be the spawn point */
        int playerSpawnIndex = Random.Range(0, smallest.nodes.Count);
        int bossSpawnIndex = Random.Range(0, biggest.nodes.Count);

        if(setPlayerSpawnRoomInactive)
            for(int i = 0; i < smallest.nodes.Count; i++)
            {
                if (i == playerSpawnIndex)
                    smallest.nodes[i].spawnType = PathBuilder.NodeSpawnType.player;
                else
                    smallest.nodes[i].spawnType = PathBuilder.NodeSpawnType.inactive;
            }
        else
            smallest.nodes[playerSpawnIndex].spawnType = PathBuilder.NodeSpawnType.player;

        if (setBossRoomInactive)
            for (int i = 0; i < biggest.nodes.Count; i++)
            {
                if (i == bossSpawnIndex)
                    biggest.nodes[i].spawnType = PathBuilder.NodeSpawnType.boss;
                else
                    biggest.nodes[i].spawnType = PathBuilder.NodeSpawnType.inactive;
            }
        else
            biggest.nodes[bossSpawnIndex].spawnType = PathBuilder.NodeSpawnType.boss;
    }

    GameObject SpawnEntities(GameObject parent = null)
    {
        GameObject lowLvlParentObj = new GameObject("Low Lvl Enemies");
        GameObject midLvlParentObj = new GameObject("Mid Lvl Enemies");

        for (int x = 0; x < pathFinder.grid.nodeCountX; x++)
        {
            for (int y = 0; y < pathFinder.grid.nodeCountY; y++)
            {
                PathBuilder.Node node = pathFinder.grid.nodes[x, y];

                if (node.spawnType == PathBuilder.NodeSpawnType.active)
                {
                    SpawnEnemyMidLvl(node, midLvlParentObj);
                    SpawnEnemyLowLvl(node, lowLvlParentObj);
                }

                if (node.spawnType == PathBuilder.NodeSpawnType.player)
                    Instantiate(player, new Vector3(node.pos.x, node.pos.y + (PathBuilder.nodeHalfDiagonal * 2), node.pos.z), Quaternion.identity);
            }
        }

        if (parent != null)
        {
            lowLvlParentObj.transform.SetParent(parent.transform);
            midLvlParentObj.transform.SetParent(parent.transform);
        }

        return lowLvlParentObj;
    }

    GameObject SpawnEnemyLowLvl(PathBuilder.Node node, GameObject patrolParent)
    {
        if (node.spawnType != PathBuilder.NodeSpawnType.active)
            return null;

        float spawnAttempt = UnityEngine.Random.Range(0f, 1.0f);

        if (spawnAttempt <= lowLvlSpawnChance)
        {
            int enemyIndex = UnityEngine.Random.Range(0, lowLvlEnemies.Length);

            GameObject baddie = Instantiate(lowLvlEnemies[enemyIndex], node.pos, Quaternion.identity);
            node.spawnType = PathBuilder.NodeSpawnType.inactive;

            baddie.transform.SetParent(patrolParent.transform);

            return baddie;
        }

        return null;
    }

    GameObject SpawnEnemyMidLvl(PathBuilder.Node node, GameObject patrolParent)
    {
        if (node.spawnType != PathBuilder.NodeSpawnType.active)
            return null;

        float spawnAttempt = UnityEngine.Random.Range(0f, 1.0f);

        if (spawnAttempt <= midLvlSpawnChance)
        {
            int enemyIndex = UnityEngine.Random.Range(0, midLvlEnemies.Length);

            GameObject baddie = Instantiate(midLvlEnemies[enemyIndex], node.pos, Quaternion.identity);
            node.spawnType = PathBuilder.NodeSpawnType.inactive;

            baddie.transform.SetParent(patrolParent.transform);

            return baddie;
        }

        return null;
    }

    private void GenerateRooms()
    {
        rooms = new List<Room>();

        //Create random rooms
        for (int i = 0; i < maxRoomQuantity; ++i)
        {
            Vector2Int size = new Vector2Int(Random.Range(minRoomSize.x, maxRoomSize.x + 1), Random.Range(minRoomSize.y, maxRoomSize.y + 1));

            // max pos uses the size of the room to ensure that it doesnt spawn partially out of bounds
            int maxXPos = (generatorOrgin.x + levelSize.x) - size.x;
            int maxYPos = (generatorOrgin.z + levelSize.y) - size.y;

            Vector2Int pos = new Vector2Int(Random.Range(generatorOrgin.x, maxXPos + 1), Random.Range(generatorOrgin.z, maxYPos + 1));

            Room temp = new(pos, size);

            rooms.Add(temp);
        }

        SeperateRooms(); //if any rooms overlap, attempt to seperate
    }

    private void SeperateRooms()
    {
        int maxAttempts = maxSeperationTries; //doesnt need to be it's own var, i think it's just more readable
        int attempt = 0;
        bool areIntersects = false;

        List<Room> invalidRooms = new List<Room>();

        while (attempt <= maxAttempts)
        {
            foreach (Room room in rooms)
            {
                foreach (Room other in rooms)
                {
                    if (room == other)
                        continue;

                    if (room.Intersects(other, roomSpacer)
                        && attempt < maxAttempts)
                    {
                        PushRooms(room, other);
                        areIntersects = true;
                    }
                    else if (room.Intersects(other, roomSpacer) && attempt >= maxAttempts)
                    {
                        if (!invalidRooms.Contains(room) && !invalidRooms.Contains(other))
                            invalidRooms.Add(other);
                    }
                }
            }

            if (areIntersects == false)
                break;
            else
                areIntersects = false;

            ++attempt;
        }

        foreach (Room room in invalidRooms)
        {
            rooms.Remove(room);
        }
    }

    private void PushRooms(Room room, Room other)
    {
        // Calculate direction from other room to room
        Vector2 direction = (room.rect.center - other.rect.center).normalized;

        Vector2Int roomPushDir = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));
        Vector2Int otherPushDir = -roomPushDir; // Opposite direction as "other"

        Vector2Int newRoomPos = room.rect.position + roomPushDir;
        Vector2Int newOtherPos = other.rect.position + otherPushDir;

        //if new pos is still within bounds, push the room
        if (newRoomPos.x >= generatorOrgin.x
            && newRoomPos.x <= (generatorOrgin.x + levelSize.x) - room.rect.width
            && newRoomPos.y >= generatorOrgin.z
            && newRoomPos.y <= (generatorOrgin.z + levelSize.y) - room.rect.height)
        {
            room.rect.position = newRoomPos;
        }

        if (newOtherPos.x >= generatorOrgin.x
            && newOtherPos.x <= (generatorOrgin.x + levelSize.x) - other.rect.width
            && newOtherPos.y >= generatorOrgin.z
            && newOtherPos.y <= (generatorOrgin.z + levelSize.y) - other.rect.height)
        {
            other.rect.position = newOtherPos;
        }
    }

    void BuildStructures()
    {
        GameObject structures = new GameObject("Structure");
        Rigidbody structuresRB = structures.AddComponent<Rigidbody>();
        structuresRB.useGravity = false;
        structuresRB.isKinematic = true;

        GameObject floorsParent = new GameObject("Floors");
        GameObject wallsParent = new GameObject("Walls");
        GameObject doorwaysParent = new GameObject("Doorways");

        floorsParent.transform.SetParent(structures.transform, true);
        wallsParent.transform.SetParent(structures.transform, true);
        doorwaysParent.transform.SetParent(structures.transform, true);

        for (int x = 0; x < pathFinder.grid.nodeCountX; x++)
        {
            for (int y = 0; y < pathFinder.grid.nodeCountY; y++)
            {
                PathBuilder.Node node = pathFinder.grid.nodes[x, y];

                if (node.type == PathBuilder.NodeType.empty)
                    continue;

            /* Build Floors */
                BuildFloor(node, floorsParent);

            /* Build Walls */
                BuildWalls(node, wallsParent);

            /* Build Door (if applicable) */
                if (node.type == PathBuilder.NodeType.door)
                    BuildDoorWay(node, wallsParent, doorwaysParent);
            }
        }
    }

    void BuildDoorWay(PathBuilder.Node node, GameObject wallParent = null, GameObject doorParent = null)
    {
        const float wallCoverageOfNode = 0.15f;
        float nodeSize = PathBuilder.nodeHalfDiagonal * 2;
        bool builtDoor = false;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (x != 0 && y != 0))
                    continue; //Skips diagonals and itself
                if (!pathFinder.grid.Contains(new Vector2Int(node.cordinates.x + x, node.cordinates.y + y)))
                    continue;

                PathBuilder.Node neighbor = pathFinder.grid.nodes[node.cordinates.x + x, node.cordinates.y + y];

                if (neighbor.type == PathBuilder.NodeType.hallway && !builtDoor)
                {//builds a door
                    GameObject doorObject;
                    int doorObjectIndex = UnityEngine.Random.Range(0, doorways.Length); //Range function max is exclusive, therefore already size - 1
                    GameObject doorPrefab = doorways[doorObjectIndex];
                    Renderer renderer = doorPrefab.GetComponent<Renderer>();

                    Vector3 prefabSize = renderer.bounds.size;
                    Vector3 scaler = new Vector3(nodeSize / prefabSize.x, (nodeSize * wallCoverageOfNode) / prefabSize.z, (2 * nodeSize) / prefabSize.y);
                    Vector3 offset = new Vector3(0, 0, 0);
                    Quaternion rotation = Quaternion.identity;

                    if (x == -1 && y == 0) // left neighbor
                    {
                        offset = new Vector3(-(nodeSize / 2), nodeSize / 2, 0);
                        rotation = Quaternion.Euler(-90, -90, 0);
                    }
                    else if (x == 1 && y == 0) // right neighbor
                    {
                        offset = new Vector3(nodeSize / 2, nodeSize / 2, 0);
                        rotation = Quaternion.Euler(-90, 90, 0);
                    }
                    else if (x == 0 && y == -1) // bottom neighbor
                    {
                        offset = new Vector3(0, nodeSize / 2, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 180, 0);
                    }
                    else if (x == 0 && y == 1) // top neighbor
                    {
                        offset = new Vector3(0, nodeSize / 2, nodeSize / 2);
                        rotation = Quaternion.Euler(-90, 0, 0);
                    }

                    doorObject = Instantiate(doorPrefab, node.pos + offset, rotation);
                    doorObject.transform.localScale = Vector3.Scale(doorObject.transform.localScale, scaler);
                    doorObject.transform.position = node.pos + offset;

                    builtDoor = true;

                    if (doorParent != null)
                        doorObject.transform.SetParent(doorParent.transform, true);
                }
                else if (builtDoor && (neighbor.type == PathBuilder.NodeType.hallway || neighbor.type == PathBuilder.NodeType.empty))
                {//builds a wall
                    int wallObjectIndex = UnityEngine.Random.Range(0, walls.Length); //Range function max is exclusive, therefore already size - 1
                    GameObject wallPrefab = walls[wallObjectIndex];
                    Renderer renderer = wallPrefab.GetComponent<Renderer>();

                    Vector3 prefabSize = renderer.bounds.size;
                    Vector3 offset = new();
                    Vector3 scaler = new Vector3(nodeSize / prefabSize.x, (nodeSize * wallCoverageOfNode) / prefabSize.z, (2 * nodeSize) / prefabSize.y); //the scale axis for y & z are fliped on these models
                    Quaternion rotation = Quaternion.identity;

                    if (x == -1 && y == 0) // left neighbor
                    {
                        offset = new Vector3(-(nodeSize / 2), 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 90, 0);
                    }
                    else if (x == 1 && y == 0) // right neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 90, 0);
                    }
                    else if (x == 0 && y == -1) // bottom neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 0, 0);
                    }
                    else if (x == 0 && y == 1) // top neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, nodeSize / 2);
                        rotation = Quaternion.Euler(-90, 0, 0);
                    }

                    GameObject wallObject = Instantiate(wallPrefab, node.pos, rotation);
                    wallObject.transform.localScale = Vector3.Scale(wallObject.transform.localScale, scaler);
                    wallObject.transform.position = node.pos + offset;

                    if (wallParent != null)
                        wallObject.transform.SetParent(wallParent.transform, true);
                }
            }
        }
    }

    void BuildFloor(PathBuilder.Node node, GameObject parent)
    {
        float nodeSize = PathBuilder.nodeHalfDiagonal * 2;

        int floorObjectIndex = UnityEngine.Random.Range(0, floors.Length); //Range function max is exclusive, therefore already size - 1
        GameObject floorPrefab = floors[floorObjectIndex];
        Renderer renderer = floorPrefab.GetComponent<Renderer>();

        Vector3 prefabSize = renderer.bounds.size;
        Vector3 offset = new Vector3(nodeSize / 2f, 0, -(nodeSize / 2f));
        Vector3 scaler = new Vector3(nodeSize / prefabSize.x, 2f, nodeSize / prefabSize.z);

        GameObject floorTile = Instantiate(floorPrefab, node.pos + offset, floorPrefab.transform.rotation);
        floorTile.transform.localScale = Vector3.Scale(floorTile.transform.localScale, scaler);
        floorTile.transform.position = node.pos + offset;

        if (parent != null)
            floorTile.transform.SetParent(parent.transform, true);
    }

    void BuildWalls(PathBuilder.Node node, GameObject parent)
    {
        const float wallCoverageOfNode = 0.15f; //what percent of the node is covered by a wall
        float nodeSize = PathBuilder.nodeHalfDiagonal * 2;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ((x == 0 && y == 0) || (x != 0 && y!= 0))
                    continue; //Skips diagonals and itself

                PathBuilder.Node neighbor;

                if (!pathFinder.grid.Contains(new Vector2Int(node.cordinates.x + x, node.cordinates.y + y)))
                    neighbor = null;
                else
                    neighbor = pathFinder.grid.nodes[node.cordinates.x + x, node.cordinates.y + y];

                if (neighbor == null 
                    || (node.type == PathBuilder.NodeType.room && neighbor.type == PathBuilder.NodeType.hallway) 
                    || neighbor.type == PathBuilder.NodeType.empty)
                {
                    int wallObjectIndex = UnityEngine.Random.Range(0, walls.Length); //Range function max is exclusive, therefore already size - 1
                    GameObject wallPrefab = walls[wallObjectIndex];
                    Renderer renderer = wallPrefab.GetComponent<Renderer>();

                    Vector3 prefabSize = renderer.bounds.size;
                    Vector3 offset = new();
                    Vector3 scaler = new Vector3(nodeSize / prefabSize.x, (nodeSize * wallCoverageOfNode) / prefabSize.z, (2 * nodeSize) / prefabSize.y); //the scale axis for y & z are fliped on these models
                    Quaternion rotation = Quaternion.identity;

                    if (x == -1 && y == 0) // left neighbor
                    {
                        offset = new Vector3(-(nodeSize / 2), 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 90, 0); 
                    }
                    else if (x == 1 && y == 0) // right neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 90, 0);
                    }
                    else if (x == 0 && y == -1) // bottom neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, -(nodeSize / 2));
                        rotation = Quaternion.Euler(-90, 0, 0);
                    }
                    else if (x == 0 && y == 1) // top neighbor
                    {
                        offset = new Vector3(nodeSize / 2, 0, nodeSize / 2);
                        rotation = Quaternion.Euler(-90, 0, 0);
                    }

                    GameObject wallObject = Instantiate(wallPrefab, node.pos, rotation);
                    wallObject.transform.localScale = Vector3.Scale(wallObject.transform.localScale, scaler);
                    wallObject.transform.position = node.pos + offset;

                    if (parent != null)
                        wallObject.transform.SetParent(parent.transform, true);
                }
            } 
        }
    }

    private List<Line> CreatePossibleConnections()
    {
        List<Vector2> points = new();
        foreach(Room room in rooms)
        {
            points.Add(room.rect.center);
        }

        HashSet<Line> lines = new();
        for(int i = 0; i < points.Count; i++)
        {
            for(int j = i + 1; j < points.Count; j++)
            {
                lines.Add(new Line(points[i], points[j]));
            }
        }

        List<Line> lineList = new List<Line>(lines);
        HashSet<Line> badLines = new();

        bool intersectionFound;
        do
        {
            intersectionFound = false;

            for (int i = 0; i < lineList.Count; i++)
            {
                for (int j = i + 1; j < lineList.Count; j++)
                {
                    if (lineList[i].Intersects(lineList[j]))
                    {
                        intersectionFound = true;

                        //remove the longer line so hallways are shorter on average
                        if (lineList[i].length > lineList[j].length)
                        {
                            badLines.Add(lineList[i]);
                        }
                        else
                        {
                            badLines.Add(lineList[j]);
                        }
                    }
                }
            }

            foreach (Line line in badLines)
            {
                lineList.Remove(line);
            }

            badLines.Clear();

        } while (intersectionFound);

        /* Next it needs to check if any of the lines are running thru rooms*/

        foreach (Line line in lineList)
        {
            foreach(Room room in rooms)
            {
                //Exclude rooms that are the start or end point of the line
                if (room.rect.center == line.start || room.rect.center == line.end)
                    continue;

                Vector2 BL = new Vector2(room.rect.xMin, room.rect.yMin);
                Vector2 BR = new Vector2(room.rect.xMax, room.rect.yMin);
                Vector2 TL = new Vector2(room.rect.xMin, room.rect.yMax);
                Vector2 TR = new Vector2(room.rect.xMax, room.rect.yMax);

                Line leftEdge = new Line(BL, TL);
                Line rightEdge = new Line(BR, TR);
                Line bottomEdge = new Line(BL, BR);
                Line topEdge = new Line(TL, TR);

                if (line.Intersects(leftEdge)
                    || line.Intersects(rightEdge)
                    || line.Intersects(bottomEdge)
                    || line.Intersects(topEdge))
                    badLines.Add(line);
            }
        }

        foreach (Line line in badLines)
        {
            lineList.Remove(line);
        }

        /*
        foreach (Line line in lineList)
        {
            Debug.DrawLine(new Vector3(line.start.x, generatorOrgin.y, line.start.y), new Vector3(line.end.x, generatorOrgin.y, line.end.y), Color.grey, 1000f);
        }
        */

        return lineList;
    }

    public List<Line> MST(List<Line> graph)
    {
        /* Intialize */

        HashSet<Line> MST = new();
        HashSet<Vector2> vertices = new();

        Vector2 starter = graph[0].start;
        vertices.Add(starter);

        //list of all the lines that could be added sorted by weight (using length for weight)
        var potentialEdges = new SortedSet<Line>(Comparer<Line>.Create((line1, line2) =>
        {
            return line1.length.CompareTo(line2.length);
        }));

        foreach(Line line in graph)
        {
            if (line.start == starter || line.end == starter)
                potentialEdges.Add(line);
        }

        /* Main Loop */

        while(true)
        {
            Line minWeightLine = potentialEdges.Min;

            MST.Add(minWeightLine);

            /*
            Debug.DrawLine(new Vector3(minWeightLine.start.x, generatorOrgin.y, minWeightLine.start.y), 
                new Vector3(minWeightLine.end.x, generatorOrgin.y, minWeightLine.end.y), Color.green, 1000f);
            */

            vertices.Add(minWeightLine.start);
            vertices.Add(minWeightLine.end);

            potentialEdges.Clear(); //Clear and create new list with new data
            foreach(Line line in graph)
            {
                int counter = 0;
                foreach(Vector2 vertex in vertices)
                {// if a line contains exactly 1 vertex, add it to potential

                    if (line.start == vertex || line.end == vertex)
                        counter++;

                    if (counter > 1)
                        continue;
                }

                if(counter == 1)
                    potentialEdges.Add(line);
            }

            if (potentialEdges.Count <= 0)
                break;
        }

        return MST.ToList();
    }

    List<Line> AddExtraHallways(List<Line> allPossibleConnections, List<Line> requiredConnections)
    {
        if (extraHallSpawnChance == 0)
            return requiredConnections;

        HashSet<Line> hallways = new HashSet<Line>(requiredConnections);

        foreach(Line connection in allPossibleConnections)
        {
            if (hallways.Contains(connection))
                continue;

            float rand = Random.Range(0f, 1f);

            if (rand <= extraHallSpawnChance)
                hallways.Add(connection);
        }

        return new List<Line>(hallways);
    }
}
