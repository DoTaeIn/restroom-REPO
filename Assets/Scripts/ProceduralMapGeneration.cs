using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ProceduralMapGeneration : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public int numberOfRooms = 10;

    void Start()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            Vector3 position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            GameObject roomGO = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], position, Quaternion.identity);


            RoomEX room = roomGO.GetComponent<RoomEX>();
            string roomID = $"RoomEX_{i}";
            room.Initialize(roomID);
            RoomManager.Instance.RegisterRoom(roomID, room);

            if (i > 0)
            {
                // 문 연결 설정은 DoorTrigger에서 수동으로 하거나 ScriptableObject로 설정 가능
            }

            if (i != 0)
                roomGO.SetActive(false); // 처음 방만 켜두기
        }
    }
}