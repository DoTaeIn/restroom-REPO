using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public Dictionary<string, RoomEX> allRooms = new();
    public RoomEX currentRoom;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterRoom(string id, RoomEX room)
    {
        if (!allRooms.ContainsKey(id))
            allRooms.Add(id, room);
    }

    public void WarpToRoom(string targetRoomID, Vector3 targetPosition)
    {
        if (allRooms.TryGetValue(targetRoomID, out RoomEX room))
        {
            // 방 활성화 + 플레이어 위치 이동
            currentRoom.gameObject.SetActive(false);
            room.gameObject.SetActive(true);
            currentRoom = room;

            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = targetPosition;
        }
    }
}