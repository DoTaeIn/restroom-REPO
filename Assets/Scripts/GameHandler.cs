using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum EndingType
{
    Escape,
    Death,
    Weapon,
    Caught
}

public class EndingEvent : UnityEvent<EndingType>
{
    
}
public class GameHandler : MonoBehaviour
{
    public List<GameObject> enemies;
    ProceduralMapGeneration proceduralMapGeneration;
    public int level = 1;
    [SerializeField] CinemachineConfiner2D confiner2D;
    PlayerCtrl playerCtrl;
    public EndingType ending;
    UIManager uiManager;
    public EndingEvent endingEvent;
    

    private void Awake()
    {
        proceduralMapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
        uiManager = FindFirstObjectByType<UIManager>();
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

    public void handleGameOver(float useAmt)
    {
        float itemDamage = 0;
        float enemyDamage = 0;
        
        if(useAmt >= 4)
        {
            ending = EndingType.Weapon;
            return;
        }

        for (int i = 0; i < playerCtrl.damageSources.Keys.Count; i++)
        {
            UnityEngine.Object source = playerCtrl.damageSources.Keys.ToList()[i];
            if (source.GetType() == typeof(Item))
            {
                itemDamage += playerCtrl.damageSources[source];
            }
            else
            {
                enemyDamage += playerCtrl.damageSources[source];
            }
        }

        if(itemDamage < 100f && enemyDamage < 100f)
        {
            ending = EndingType.Escape;
            return;
        }
        else if (itemDamage > enemyDamage)
        {
            ending = EndingType.Death;
        }
        else
        {
            ending = EndingType.Caught;
        }
        
        endingEvent.Invoke(ending);
    }
}
