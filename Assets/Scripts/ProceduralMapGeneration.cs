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
using UnityEditor.Experimental.GraphView;

public class ProceduralMapGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public TileBase wallTile;
    public int numberOfRooms = 20;
    public event Action OnMapGenerated;
    private MiniMapManager minimapmanager;
    RoomManager roomManager;
    NavMeshSurface surface;
    Room room;
    public int roomGap = 25;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
        minimapmanager = FindFirstObjectByType<MiniMapManager>();
    }

    public int toiletSeed;
    void Start()
    {
        toiletSeed = Random.Range(numberOfRooms/2, numberOfRooms);

        for (int i = 0; i < numberOfRooms; i++)
        {
            int x = (i % 5) * roomGap + 5;
            int y = Mathf.FloorToInt(i / 5) * roomGap + 5;

            if (i != toiletSeed)
            {
                GameObject roomGO = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity);
                Room room = roomGO.GetComponent<Room>();
                room.InitRoom(i, x, y);
                roomManager.RegisterRoom(i, room);
                room.SetupDoors();
            }
            else
            {
                GameObject Toilet = Instantiate(roomPrefabs[1], Vector3.zero, Quaternion.identity);
                Toilet.name = "Toilet";
                Room toilet = Toilet.GetComponent<Room>();
                toilet.CreateToilet(i, x, y);
                roomManager.RegisterRoom(i, toilet);
                toilet.SetupDoors();


            }
        }

        GameObject LivingRoom = Instantiate(roomPrefabs[2], Vector3.zero, Quaternion.identity);
        LivingRoom.name = "LivingRoom";

        Room livingRoom = LivingRoom.GetComponent<Room>();
        livingRoom.GenerateLivingRoom(35, -20, 30, 20);
        roomManager.RegisterRoom(-3, livingRoom);

        livingRoom.SetupDoors();

        NavMeshSurface surface = FindFirstObjectByType<NavMeshSurface>();
        surface.BuildNavMesh();
        
        OnMapGenerated?.Invoke();
        
        minimapmanager.BuildMiniMap(RoomManager.Instance.allRoomsV);
    }

    
    
}