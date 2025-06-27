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

    void Start()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector3 position = new Vector3(0, 0, 0);
            GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], position, Quaternion.identity);


            RoomEX room = roomGO.GetComponent<RoomEX>();

            string roomID = $"{i}";



            int x = (i % 5)*20 + 5;
            int y = Mathf.FloorToInt(i/5)*20 + 5;


            room.Initialize(roomID, x, y);
            RoomManager.Instance.RegisterRoom(roomID, room);
            Debug.Log($"Room {roomID} position: {x}x{y}");

        }
    }
}