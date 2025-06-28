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
    public int numberOfRooms = 10;
    
    RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
    }

    void Start()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector3 position = new Vector3(0, 0, 0);
            GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], position, Quaternion.identity);


            Room room = roomGO.GetComponent<Room>();
            

            int x = (i % 5)*20 + 5;
            int y = Mathf.FloorToInt(i/5)*20 + 5;
            
            room.InitRoom(i, x, y);
            roomManager.RegisterRoom(i, room);
            //Debug.Log($"Room {i} position: {x}x{y}");

        }
    }
}