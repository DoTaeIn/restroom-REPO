using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public Dictionary<int, Room> allRooms = new();
    public Dictionary<Vector2Int, Room> allRoomsV = new();
    public Room room;

    void Awake()
    {
            Instance = this;
    }

    public void RegisterRoom(int id, Room room)
    {
        Debug.Log(Mathf.FloorToInt(id / 5) + 1);
        allRooms.Add(id, room);
        int x;
        int y;
        if (id >= 0)
        {
            x = id % 5 + 1; // Assuming a grid of 5 rooms per row
            y = Mathf.FloorToInt(id / 5) + 1;
        }
        else
        {
            x = 3;
            y = 0;
        }
        room.gridPosition = new Vector2Int(x, y);
        allRoomsV.Add(room.gridPosition, room);
    }

    public void WarpToRoom(int targetRoomID, Vector3 targetPosition)
    {
        if (allRooms.TryGetValue(targetRoomID, out Room room1))
        {
            room.gameObject.SetActive(false);
            room.gameObject.SetActive(true);
            room = room1;
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = targetPosition;
        }
    }
}