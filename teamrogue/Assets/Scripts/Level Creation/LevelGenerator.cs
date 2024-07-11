using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] Vector2Int levelSize;

    [Header("Rooms")]
    [SerializeField] int maxRoomQuantity;
    [SerializeField] Vector2Int minRoomSize;
    [SerializeField] Vector2Int maxRoomSize;

    [Tooltip("The maximum amount of times the program will try to seperate overlapping rooms before removing them")]
    [SerializeField] int maxSeperationTries = 30;
    [Tooltip("The minimum amount of space between each room")]
    [SerializeField] int roomSpacer = 5;

    [Header("Debug")]
    [SerializeField] GameObject roomBuilder;
    [SerializeField] int seed;

    Vector3Int generatorOrgin;
    private List<Room> rooms;

    public class Room
    {
        public RectInt rect;

        public Room(Vector2Int pos, Vector2Int size)
        {
            rect = new RectInt(pos, size);
        }

        public bool Intersects(in Room other, int bufferAmount = 0)
        {
            if(rect.xMax + bufferAmount > other.rect.xMin - bufferAmount
                && other.rect.xMax + bufferAmount > rect.xMin - bufferAmount
                && rect.yMax + bufferAmount > other.rect.yMin - bufferAmount
                && other.rect.yMax + bufferAmount > rect.yMin - bufferAmount)
            {
                return true;
            }
            else
                return false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);

        //Sets the transform to the closest int value. this makes a grid system much simpler 
        generatorOrgin = new Vector3Int(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.y), Mathf.CeilToInt(transform.position.z));
        transform.position = generatorOrgin;

        GenerateRooms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        /*Creates a gizmo in the scene view to visualize where the level will be built
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

        SeperateRooms(); //if any rooms overlap, attemp to seperate
        PlaceRooms(); //Once rooms are seperated, place them
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
                        if(!invalidRooms.Contains(room) && !invalidRooms.Contains(other))
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

    private void PlaceRooms()
    {
        foreach (Room room in rooms)
        {
            Vector3 roomCenter = new Vector3(room.rect.x + room.rect.width / 2f, generatorOrgin.y, room.rect.y + room.rect.height / 2f);

            GameObject newRoomBuilder = Instantiate(roomBuilder, roomCenter, Quaternion.identity);

            newRoomBuilder.transform.localScale = new Vector3(room.rect.width, 1f, room.rect.height);
        }
    }
}
