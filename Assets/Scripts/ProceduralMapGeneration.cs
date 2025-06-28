using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using JetBrains.Annotations;
using System;
using Random = UnityEngine.Random;

public class ProceduralMapGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public TileBase wallTile;
    public int numberOfRooms = 45;
    
    RoomManager roomManager;
    Room room;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
        room = FindFirstObjectByType<Room>();
    }

    void Start()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);

            Room room = roomGO.GetComponent<Room>();
            int x = (i % 5) * 20 + 5;
            int y = Mathf.FloorToInt(i / 5) * 20 + 5;

            room.InitRoom(i, x, y);
            roomManager.RegisterRoom(i, room);
            //Debug.Log($"Room {i} position: {x}x{y}");
        }

        GameObject LivingRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);
        LivingRoom.name = "LivingRoom";
        Room livingRoom = LivingRoom.GetComponent<Room>();
        livingRoom.GenerateLivingRoom(35, -20, 30, 20);
        roomManager.RegisterRoom(-3, livingRoom);


        room.SetupDoors();

    }
}