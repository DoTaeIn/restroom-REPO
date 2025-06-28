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
    NavMeshSurface surface;
    Room room;

    private void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
    }
    

    void Start()
    {
        int toiletSeed = Random.Range(30, numberOfRooms);

        for (int i = 0; i < numberOfRooms; i++)
        {
            if (i != toiletSeed)
            {
                GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);

                Room room = roomGO.GetComponent<Room>();
                int x = (i % 5) * 20 + 5;
                int y = Mathf.FloorToInt(i / 5) * 20 + 5;

                room.InitRoom(i, x, y);
                roomManager.RegisterRoom(i, room);
                //Debug.Log($"Room {i} position: {x}x{y}");
            }
            else
            {
                GameObject Toilet = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);
                Toilet.name = "Toilet";
                Room toilet = Toilet.GetComponent<Room>();
                int toileti = i;
                roomManager.RegisterRoom(i, toilet);
                toilet.CreateToilet(toileti, (toileti % 5) * 20 + 5, Mathf.FloorToInt(toileti / 5) * 20 + 5);
            }

        }

            int x = (i % 5) * 20 + 5;
            int y = Mathf.FloorToInt(i / 5) * 20 + 5;

        GameObject LivingRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), Quaternion.identity);
        LivingRoom.name = "LivingRoom";
        Room livingRoom = LivingRoom.GetComponent<Room>();
        livingRoom.GenerateLivingRoom(35, -20, 30, 20);
        roomManager.RegisterRoom(-3, livingRoom);
        livingRoom.SetupDoors();


        }

        room.SetupDoors();

    }
    
    
}