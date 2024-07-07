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

    [Header("Debug")]
    [SerializeField] GameObject roomBuilder;
    [SerializeField] int seed;

    private List<Room> rooms;

    public class Room
    {
        public RectInt rect;

        public Room(Vector2Int pos, Vector2Int size)
        {
            rect = new RectInt(pos, size);
        }

        public bool Intersects(in Room other)
        {
            if(rect.xMax > other.rect.xMin
                && other.rect.xMax > rect.xMin
                && rect.yMax > other.rect.yMin
                && other.rect.yMax > rect.yMin)
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

        Vector3 bottomLeft = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x + levelSize.x, 0, transform.position.z);
        Vector3 topLeft = new Vector3(transform.position.x, 0, transform.position.z + levelSize.y);
        Vector3 topRight = new Vector3(transform.position.x + levelSize.x, 0, transform.position.z + levelSize.y);

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
            int maxXPos = levelSize.x - size.x;
            int maxYPos = levelSize.y - size.y;

            Vector2Int pos = new Vector2Int(Random.Range(0, maxXPos + 1), Random.Range(0, maxYPos + 1));

            Room temp = new(pos, size);

            rooms.Add(temp);
        }

        SeperateRooms(); //if any rooms overlap, attemp to seperate
        PlaceRooms(); //Once rooms are seperated, place them
        
    }

    private void SeperateRooms()
    {
        int maxAttempts = 30;
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

                    if (room.Intersects(other) 
                        && attempt < maxAttempts)
                    {
                        PushRooms(room, other);
                        areIntersects = true;
                    }
                    else if (room.Intersects(other) && attempt >= maxAttempts)
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
        if (newRoomPos.x >= 0 && newRoomPos.x <= levelSize.x - room.rect.width
            && newRoomPos.y >= 0 && newRoomPos.y <= levelSize.y - room.rect.height)
        {
            room.rect.position = newRoomPos;
        }

        if (newOtherPos.x >= 0 && newOtherPos.x <= levelSize.x - other.rect.width
            && newOtherPos.y >= 0 && newOtherPos.y <= levelSize.y - other.rect.height)
        {
            other.rect.position = newOtherPos;
        }
    }

    private void PlaceRooms()
    {
        foreach (Room room in rooms)
        {
            Vector3 roomCenter = new Vector3(room.rect.x + room.rect.width / 2f, 0f, room.rect.y + room.rect.height / 2f);

            GameObject newRoomBuilder = Instantiate(roomBuilder, roomCenter, Quaternion.identity);

            newRoomBuilder.transform.localScale = new Vector3(room.rect.width, 1f, room.rect.height);
        }
    }
}
