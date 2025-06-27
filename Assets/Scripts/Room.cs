using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public int _id;
    private bool _hasKey;
    public SizeSettings roomSize;
    //SizeSettings _furnitureSize;
    [SerializeField] FurnitureManager _furnitureManager;

    [Header("Tilemap Settings")]
    public Tilemap floorTilemap;
    public TileBase floorTile;
    public Tilemap wallTilemap;
    public TileBase wallTile;
    public GameObject[] doorPrefab;

    PolygonCollider2D polygon;

    private void Awake()
    {
        roomSize = new SizeSettings();
        wallTilemap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
        floorTilemap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        polygon = GetComponent<PolygonCollider2D>();
        _furnitureManager = GetComponent<FurnitureManager>();
    }

    private Vector2 GetRandomPos()
    {
        float minX = roomSize.Position.x + 2;  // 방 내부 1칸 여유
        float maxX = roomSize.Position.x + roomSize.XSize - 2;
        float minY = roomSize.Position.y + 2;
        float maxY = roomSize.Position.y + roomSize.YSize - 2;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        return new Vector2(x, y);
    }


    private Quaternion GetRandomRotation()
    {
        float ranValue = Random.Range(0f, 4);
        switch (ranValue)
        {
            case 0:
                return Quaternion.Euler(0f, 0f, 0f);
            case 1:
                return Quaternion.Euler(0f, 90f, 0f);
            case 2:
                return Quaternion.Euler(0f, 180f, 0f);
            case 3:
                return Quaternion.Euler(0f, 270f, 0f);
            default:
                return Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void InitRoom(int id, int startX, int startY)
    {
        roomSize.XSize = Random.Range(10, 15);
        roomSize.YSize = Random.Range(10, 15);
        roomSize.Position = new Vector2(startX, startY);
        roomSize.Rotation = Quaternion.identity;

        GenerateWalls(startX, startY);
        this._id = id;
        Vector3Int asdasd = new Vector3Int(startX, startY, 0);
        GenerateDoors(startX, startY, Mathf.RoundToInt(roomSize.XSize), Mathf.RoundToInt(roomSize.YSize));

        foreach (Furniture furniturePrefab in _furnitureManager.furnitures)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < 10)
            {
                Vector2 randomPos = GetRandomPos();
                Quaternion randomRot = GetRandomRotation();

                SizeSettings proposed = new SizeSettings
                {
                    XSize = furniturePrefab.FurnitureSize?.XSize ?? 1,
                    YSize = furniturePrefab.FurnitureSize?.YSize ?? 1,
                    Position = randomPos,
                    Rotation = randomRot
                };


                bool interferes = false;
                foreach (Furniture obj in _furnitureManager.placed)
                {
                    if (obj.IsInterfering(proposed))
                    {
                        interferes = true;
                        break;
                    }
                }


                if (!interferes)
                {
                    GameObject gm = Instantiate(furniturePrefab.gameObject, randomPos, randomRot);
                    gm.transform.SetParent(transform);
                    _furnitureManager.AddFurniture(gm.GetComponent<Furniture>());
                    placed = true;
                }

                attempts++;
            }
        }

    }


    public void GenerateWalls(int startx, int starty)
    {
        Debug.Log($"Generating walls for room {_id} at position ({startx}, {starty}) with size ({roomSize.XSize}x{roomSize.YSize})");
        for (int x = startx; x < startx + roomSize.XSize; x++)
        {
            for (int y = starty; y < starty + roomSize.YSize; y++)
            {
                // 외곽선만 벽 생성
                if (x == startx || y == starty || x == startx + roomSize.XSize - 1 || y == starty + roomSize.YSize - 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }

        Vector2[] points = new Vector2[5];

        float minX = roomSize.Position.x;
        float minY = roomSize.Position.y;
        float maxX = roomSize.Position.x + roomSize.XSize;
        float maxY = roomSize.Position.y + roomSize.YSize;

        points[0] = new Vector2(minX, minY);
        points[1] = new Vector2(minX, maxY);
        points[2] = new Vector2(maxX, maxY);
        points[3] = new Vector2(maxX, minY);
        points[4] = points[0]; // Close the loop

        polygon.pathCount = 1;
        polygon.SetPath(0, points);

        GenerateFloors();
    }


    private void GenerateFloors()
    {
        for (int x = 0; x < roomSize.XSize; x++)
        {
            for (int y = 0; y < roomSize.YSize; y++)
            {
                // 바닥 타일 생성
                floorTilemap.SetTile(new Vector3Int((int)roomSize.Position.x + x, (int)roomSize.Position.y + y, 0), floorTile);
            }
        }
    }
    public Sprite doorSprite;
    private void GenerateDoors(int startX, int startY, int width, int height)
    {
        // 아래쪽(Bottom) 벽에서 랜덤 x
        int bottomX = Random.Range(startX + 1, startX + width - 1);
        Vector3Int bottomDoor = new Vector3Int(bottomX, startY, 0);
        CreateDoorObject("LeftDoor", bottomDoor);

        // 위쪽(Top) 벽에서 랜덤 x
        int topX = Random.Range(startX + 1, startX + width - 1);
        Vector3Int topDoor = new Vector3Int(topX, startY + height - 1, 0);
        CreateDoorObject("TopDoor", topDoor);

        // 왼쪽(Left) 벽에서 랜덤 y
        int leftY = Random.Range(startY + 1, startY + height - 1);
        Vector3Int leftDoor = new Vector3Int(startX, leftY, 0);
        CreateDoorObject("LeftDoor", leftDoor);

        // 오른쪽(Right) 벽에서 랜덤 y
        int rightY = Random.Range(startY + 1, startY + height - 1);
        Vector3Int rightDoor = new Vector3Int(startX + width - 1, rightY, 0);
        CreateDoorObject("RightDoor", rightDoor);

        CreateDoorAt(bottomDoor);
        CreateDoorAt(topDoor);
        CreateDoorAt(leftDoor);
        CreateDoorAt(rightDoor);

    }

    void CreateDoorAt(Vector3Int Door)
    {
        wallTilemap.SetTile(Door, null);
        floorTilemap.SetTile(Door, floorTile);
    }

    public void CreateDoorObject(string name, Vector3 worldPosition)
    {
        GameObject door = new GameObject(name);
        door.transform.position = worldPosition;
        door.transform.parent = this.transform; // 방 내부 구조로 귀속

        // 스프라이트 렌더러 추가
        SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
        sr.sprite = doorSprite;
        sr.sortingOrder = 10; // 배경 위에 뜨게 하고 싶으면 정렬 순서 설정

        // 충돌 박스 추가
        BoxCollider2D col = door.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        // 태그 지정
        door.tag = "Door";
    }


}
