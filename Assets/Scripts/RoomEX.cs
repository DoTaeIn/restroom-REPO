using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomEX : MonoBehaviour
{
    public string roomID; // 예: "Room_1", "Room_A2"
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public TileBase wallTile;

    public Transform[] doors; // 0: up, 1: down, 2: left, 3: right

    private int width;
    private int height;


    // 향후 확장을 위한 초기화 함수
    public void Initialize(string id, int startx, int starty)
    {
        width = Random.Range(10, 15);
        height = Random.Range(10, 15);

        GenerateWalls(startx, starty);
        this.roomID = id;
    }

     public void GenerateWalls(int startx, int starty)
    {
        Debug.Log($"Generating walls for room {roomID} at position ({startx}, {starty}) with size ({width}x{height})");
        for (int x = startx; x < startx + width; x++)
        {
            for (int y = starty; y < starty + height; y++)
            {
                // 외곽선만 벽 생성
                if (x == startx || y == starty || x == startx + width - 1 || y == starty + height - 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }
}