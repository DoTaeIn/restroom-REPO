using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomEX : MonoBehaviour
{
    public string roomID; // 예: "Room_1", "Room_A2"
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Transform[] doors; // 0: up, 1: down, 2: left, 3: right

    // 향후 확장을 위한 초기화 함수
    public void Initialize(string id)
    {
        this.roomID = id;
        // 타일 배치 등 초기화 가능
    }
}