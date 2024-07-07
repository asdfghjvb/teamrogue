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
        public RectInt roomBoarder;

        public Room(Vector2Int pos, Vector2Int size)
        {
            roomBoarder = new RectInt(pos, size);
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
        Debug.Log("Generating rooms...");
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

        /*
        //Place rooms. this is the debug version
        foreach(Room room in rooms)
        {
            Vector3 roomCenter = new Vector3(room.roomBoarder.x + room.roomBoarder.width / 2f, 0f, room.roomBoarder.y + room.roomBoarder.height / 2f);

            // Instantiate roomBuilder (colored cube)
            GameObject newRoomBuilder = Instantiate(roomBuilder, roomCenter, Quaternion.identity);

            // Adjust scale to match room size
            newRoomBuilder.transform.localScale = new Vector3(room.roomBoarder.width, 1f, room.roomBoarder.height);
        }
        */
    }

    private void SeperateRooms()
    {
        int attempt = 0;

        while (attempt <= 3)
        {
            foreach (Room room in rooms)
            {
                foreach (Room other in rooms)
                {
                    if (room.roomBoarder.Overlaps(other.roomBoarder) && attempt < 3)
                    {// if there is overlap, the rooms will push away from eachother
                        Vector2 otherDirTemp = room.roomBoarder.center - other.roomBoarder.center;
                        otherDirTemp.Normalize();
                        Vector2Int otherDir = new Vector2Int(Mathf.RoundToInt(otherDirTemp.x), Mathf.RoundToInt(otherDirTemp.y));
                        Vector2Int pushDir = new Vector2Int(-otherDir.x, -otherDir.y);
                    }
                    else if (room.roomBoarder.Overlaps(other.roomBoarder) && attempt >= 3)
                    {

                    }
                }
            }

            ++attempt;
        }
    }
}
