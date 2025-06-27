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
    [SerializeField]FurnitureManager _furnitureManager;
    
    [Header("Tilemap Settings")]
    public Tilemap floorTilemap;
    public TileBase floorTile;
    public Tilemap wallTilemap;
    public TileBase wallTile;
    public Transform[] doors;
    
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
}
