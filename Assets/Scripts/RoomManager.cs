using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Dictionary<int, Room> allRooms = new();
    public Room currentRoom;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    public void RegisterRoom(int id, Room room)
    {
        if (!allRooms.ContainsKey(id))
            allRooms.Add(id, room);
    }

    public void WarpToRoom(int targetRoomID, Vector3 targetPosition)
    {
        if (allRooms.TryGetValue(targetRoomID, out Room room))
        {
            currentRoom.gameObject.SetActive(false);
            room.gameObject.SetActive(true);
            currentRoom = room;

            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = targetPosition;
        }
    }
}