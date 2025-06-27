using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomEX : MonoBehaviour
{
    public string roomID; // 예: "Room_1", "Room_A2"
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public Tilemap doorTilemap; // 문 타일맵
    public TileBase wallTile;
    public TileBase floorTile;
    public TileBase doorTile; // 문 타일

    public Transform[] doors; // 0: up, 1: down, 2: left, 3: right

    private int width;
    private int height;

    private void Awake()
    {
        GameObject floorObj = GameObject.FindWithTag("FloorTilemap");
        floorTilemap = floorObj.GetComponent<Tilemap>();
        GameObject wallObj = GameObject.FindWithTag("WallTilemap");
        wallTilemap = wallObj.GetComponent<Tilemap>();
        GameObject doorObj = GameObject.FindWithTag("DoorTilemap");
        doorTilemap = doorObj.GetComponent<Tilemap>();
        //floorTile = Resources.Load<TileBase>("Tiles/FloorTile"); // Resources/Tiles/FloorTile.asset
        //wallTile = Resources.Load<TileBase>("Tiles/WallTile"); // Resources/Tiles/WallTile.asset
        //doorTile = Resources.Load<TileBase>("Tiles/DoorTile"); // Resources/Tiles/DoorTile.asset
    }

    // 향후 확장을 위한 초기화 함수
    public void Initialize(string id, int startx, int starty)
    {
        width = Random.Range(10, 15);
        height = Random.Range(10, 15);

        GenerateWalls(startx, starty);
        GenerateDoors(startx, starty);
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
                else
                {
                    // 바닥 타일 생성
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }
    }

    public void GenerateDoors(int startx, int starty)
    {
        wallTilemap.SetTile(new Vector3Int(startx + Random.Range(1, width - 1), starty, 0), null); // 아래쪽 문
        wallTilemap.SetTile(new Vector3Int(startx + Random.Range(1, width - 1), starty + height - 1, 0), null); // 위쪽 문
        wallTilemap.SetTile(new Vector3Int(startx, starty + Random.Range(1, height - 1), 0), null); // 왼쪽 문
        wallTilemap.SetTile(new Vector3Int(startx + width - 1, starty + Random.Range(1, height - 1), 0), null); // 오른쪽 문

        doorTilemap.SetTile(new Vector3Int(startx + Random.Range(1, width - 1), starty, 0), doorTile); // 아래쪽 문
        doorTilemap.SetTile(new Vector3Int(startx + Random.Range(1, width - 1), starty + height - 1, 0), doorTile); // 위쪽 문
        doorTilemap.SetTile(new Vector3Int(startx, starty + Random.Range(1, height - 1), 0), doorTile); // 왼쪽 문
        doorTilemap.SetTile(new Vector3Int(startx + width - 1, starty + Random.Range(1, height - 1), 0), doorTile); // 오른쪽 문
    
        Instantiate(this, new Vector3Int(startx + Random.Range(1,width-1), starty, 0), Quaternion.identity); // 아래쪽 문
        Instantiate(this, new Vector3Int(startx + Random.Range(1,width-1), starty + height - 1, 0), Quaternion.identity); // 위쪽 문
        Instantiate(this, new Vector3Int(startx, starty + Random.Range(1,height-1), 0), Quaternion.identity); // 왼쪽 문
        Instantiate(this, new Vector3Int(startx + width - 1, starty + Random.Range(1,height-1), 0), Quaternion.identity); // 오른쪽 문
    }
}