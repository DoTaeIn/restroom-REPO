using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{
    public List<GameObject> enemies;
    ProceduralMapGeneration proceduralMapGeneration;
    public int level = 1;
    [SerializeField] CinemachineConfiner2D confiner2D;
    PlayerCtrl playerCtrl;

    private void Awake()
    {
        proceduralMapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
        //confiner2D = FindFirstObjectByType<CinemachineConfiner2D>();
        playerCtrl = FindFirstObjectByType<PlayerCtrl>();
    }

    private void OnEnable()
    {
        proceduralMapGeneration.OnMapGenerated += SpawnEnemies;
    }
    
    private void OnDisable()
    {
        proceduralMapGeneration.OnMapGenerated -= SpawnEnemies;
    }
    
    void SpawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            int random = Random.Range(0, RoomManager.Instance.allRoomsV.Values.Count);
            Room room = RoomManager.Instance.allRoomsV.Values.ToList()[random];

            Vector3 randomPosition = new Vector3(
                Random.Range(room.roomSize.Position.x, room.roomSize.Position.x + room.roomSize.XSize),
                Random.Range(room.roomSize.Position.y, room.roomSize.Position.y + room.roomSize.YSize),
                0
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
            {
                GameObject enemyInstance = Instantiate(enemy, hit.position, Quaternion.identity);

                NavMeshAgent agent = enemyInstance.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                    agent.enabled = true;
                    agent.Warp(hit.position);  // Ensure agent is snapped onto NavMesh
                }
            }
            else
            {
                Debug.LogWarning("Could not find valid NavMesh position for enemy.");
            }
        }
    }

    private void Update()
    {
        confiner2D.BoundingShape2D = playerCtrl.currentRoom.polygon;
    }
}
