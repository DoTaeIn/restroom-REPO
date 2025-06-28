using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.AI.Navigation;
using Random = UnityEngine.Random;
using NavMeshPlus.Components;
using NavMeshSurface = NavMeshPlus.Components.NavMeshSurface;

public class ProceduralMapGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public TileBase wallTile;
    public int numberOfRooms = 45;
    public event Action OnMapGenerated;
    
    RoomManager roomManager;
    Room room;
    NavMeshSurface surface;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
        room = FindFirstObjectByType<Room>();
    }

    void Start()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector3 position = new Vector3(0, 0, 0);
            GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], position, Quaternion.identity);


            Room room = roomGO.GetComponent<Room>();


            int x = (i % 5) * 20 + 5;
            int y = Mathf.FloorToInt(i / 5) * 20 + 5;

            room.InitRoom(i, x, y);
            roomManager.RegisterRoom(i, room);
            //Debug.Log($"Room {i} position: {x}x{y}");

            room.SetupDoors();
        }
        
        Debug.Log("All rooms initialized and registered.");

        surface = FindFirstObjectByType<NavMeshSurface>();
        surface.BuildNavMesh();
        Debug.Log(surface.size.magnitude);
        OnMapGenerated?.Invoke();
    }
    
    
}