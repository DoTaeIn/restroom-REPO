using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using rigidbody2D = UnityEngine.Rigidbody2D;

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
    public TileBase floorTile2;
    public Tilemap wallTilemap;
    public TileBase wallTile;
    public TileBase wallTile2;
    public GameObject doorPrefab;
    public Vector2Int gridPosition;
    PolygonCollider2D polygon;
    private ProceduralMapGeneration mapGeneration;

    private void Awake()
    {
        roomSize = new SizeSettings();
        wallTilemap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
        floorTilemap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        polygon = GetComponent<PolygonCollider2D>();
        _furnitureManager = GetComponent<FurnitureManager>();
        mapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
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
        roomSize.XSize = Random.Range(10, 12);
        roomSize.YSize = Random.Range(8, 10);
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

            while (!placed && attempts < 20)
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

    public void GenerateLivingRoom(int startx, int starty, int width, int height)
    {
        for (int x = startx; x < startx + width; x++)
        {
            for (int y = starty; y < starty + height; y++)
            {
                // 외곽선만 벽 생성
                if (x == startx || y == starty || x == startx + width - 1 || y == starty + height - 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
                // 바닥 타일 생성
                else
                {
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }

        Vector3 TopDoormidliv = new Vector3(startx + width / 2 + 0.5f, starty + height - 0.5f, 0);
        _id = -3;

        CreateDoorAt(new Vector3Int(startx + width / 2, starty + height - 1, 0)); // 위쪽 중앙에 문 생성
        d = CreateDoorObject("TopDoor", TopDoormidliv, this);
        d.parentRoom = this;
        d.direction = "Top";
        d.doorpos = TopDoormidliv;
        doors.Add(d);
        
        
        Vector3 BottomDoormidliv = new Vector3(startx + width / 2 + 0.5f, starty + 0.5f, 0);
        CreateDoorAt(new Vector3Int(startx + width / 2, starty, 0)); // 아래쪽 중앙에 문 생성
        d = CreateDoorObject("BottomDoor", BottomDoormidliv, this);
        d.parentRoom = this;
        d.direction = "Bottom";
        d.doorpos = BottomDoormidliv;
        doors.Add(d);
    }


    public void GenerateWalls(int startx, int starty)
    {
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


    public void CreateToilet(int id, int startx, int starty)
    {
        roomSize.XSize = 7;
        roomSize.YSize = 7;
        roomSize.Position = new Vector2(startx, starty);
        roomSize.Rotation = Quaternion.identity;

        for (int x = startx; x < startx + roomSize.XSize; x++)
        {
            for (int y = starty; y < starty + roomSize.YSize; y++)
            {
                // 외곽선만 벽 생성
                if (x == startx || y == starty || x == startx + roomSize.XSize - 1 || y == starty + roomSize.YSize - 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile2);
                }
                else
                {
                    // 바닥 타일 생성
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile2);
                }
            }
        }

        this._id = id;
        Vector3Int asdasd = new Vector3Int(startx, starty, 0);
        
        GenerateDoors(startx, starty, Mathf.RoundToInt(roomSize.XSize), Mathf.RoundToInt(roomSize.YSize));

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
    public List<Door> doors = new List<Door>();
    private Door d;
    private void GenerateDoors(int startX, int startY, int width, int height)
    {
        if ((startX - 5) / 20 != 0)
        {
            // 왼쪽(Left) 벽에서 랜덤 y
            int leftY = Random.Range(startY + 1, startY + height - 1);
            Vector3Int leftDoor = new Vector3Int(startX, leftY, 0);
            Vector3 leftDoormid = new Vector3(startX + 0.5f, leftY + 0.5f, 0);
            d = CreateDoorObject("LeftDoor", leftDoormid, this);
            d.parentRoom = this;
            d.direction = "Left";
            d.doorpos = leftDoormid;
            doors.Add(d);

            CreateDoorAt(leftDoor);
        }
        if ((startX - 5) / 20 != 4)
        {

            // 오른쪽(Right) 벽에서 랜덤 y
            int rightY = Random.Range(startY + 1, startY + height - 1);
            Vector3Int rightDoor = new Vector3Int(startX + width - 1, rightY, 0);
            Vector3 rightDoormid = new Vector3(startX + width - 0.5f, rightY + 0.5f, 0);
            d = CreateDoorObject("RightDoor", rightDoormid, this);
            d.parentRoom = this;
            d.direction = "Right";
            doors.Add(d);
            d.doorpos = rightDoormid;
            CreateDoorAt(rightDoor);
        }
        if (((startY - 5) / 20 != 0) || ((startX -5)/20 == 2))
        {
            // 아래쪽(Bottom) 벽에서 랜덤 x
            int bottomX = Random.Range(startX + 1, startX + width - 1);
            Vector3Int bottomDoor = new Vector3Int(bottomX, startY, 0);
            Vector3 bottomDoormid = new Vector3(bottomX + 0.5f, startY + 0.5f, 0);
            d = CreateDoorObject("BottomDoor", bottomDoormid, this);
            d.parentRoom = this;
            d.direction = "Bottom";
            doors.Add(d);
            d.doorpos = bottomDoormid;
            CreateDoorAt(bottomDoor);

        }
        if ((startY - 5) / 20 != mapGeneration.numberOfRooms / 5 - 1)
        {
            // 위쪽(Top) 벽에서 랜덤 x
            int topX = Random.Range(startX + 1, startX + width - 1);
            Vector3Int topDoor = new Vector3Int(topX, startY + height - 1, 0);
            Vector3 topDoormid = new Vector3(topX + 0.5f, startY + height - 0.5f, 0);
            d = CreateDoorObject("TopDoor", topDoormid, this);
            d.parentRoom = this;
            d.direction = "Top";
            doors.Add(d);
            d.doorpos = topDoormid;
            CreateDoorAt(topDoor);
        }
    }

    void CreateDoorAt(Vector3Int Door)
    {
        wallTilemap.SetTile(Door, null);
        floorTilemap.SetTile(Door, floorTile);
    }

    public Door CreateDoorObject(string name, Vector3 worldPosition, Room parent)
    {
        GameObject door = new GameObject(name);
        door.transform.position = worldPosition;
        door.transform.parent = this.transform; // 방 내부 구조로 귀속
        if (name == "LeftDoor" || name == "RightDoor")
            door.transform.eulerAngles = new Vector3(0, 0, 0); // Y축 기준 0도 회전
        else
            door.transform.eulerAngles = new Vector3(0, 0, 90f); // Z축 기준 90도 회전\
        Door doorComponent = door.AddComponent<Door>();

        doorComponent.roomID = _id;
        doorComponent.parentRoom = parent;

        // 스프라이트 렌더러 추가
        SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
        sr.sprite = doorSprite;
        sr.sortingOrder = 10; // 배경 위에 뜨게 하고 싶으면 정렬 순서 설정
        door.transform.localScale = new Vector3(3f, 3f, 1f); // 절반 크기

        // 충돌 박스 추가
        BoxCollider2D col = door.AddComponent<BoxCollider2D>();
        col.isTrigger = false;

        // rigidbody2D 추가
        Rigidbody2D rb = door.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        // 태그 지정
        door.tag = "Door";
        return doorComponent;
    }


    public void SetupDoors()
    {
        Dictionary<Vector2Int, Room> allRooms;
        allRooms = RoomManager.Instance.allRoomsV;
        foreach (var pair in allRooms)
        {
            Debug.Log($"Setting up doors for room at position {pair.Key}");
            
            Room room = pair.Value;
            foreach (Door door in room.doors)
            {
                Vector2Int targetPos = room.gridPosition;
                switch (door.direction)
                {
                    case "Top": targetPos += Vector2Int.up; break;
                    case "Bottom": targetPos += Vector2Int.down; break;
                    case "Left": targetPos += Vector2Int.left; break;
                    case "Right": targetPos += Vector2Int.right; break;
                }

                if (allRooms.TryGetValue(targetPos, out Room neighborRoom))
                {
                    // 연결 대상 도어를 direction 반대방향으로 찾음
                    string opposite = GetOppositeDirection(door.direction);
                    Door targetDoor = neighborRoom.doors.Find(d => d.direction == opposite);
                    if (targetDoor != null)
                    {
                        door.connectedDoor = targetDoor;
                    }
                }
            }
        }
    }
            string GetOppositeDirection(string dir)
        {
            return dir switch
            {
                "Top" => "Bottom",
                "Bottom" => "Top",
                "Left" => "Right",
                "Right" => "Left",
                _ => ""
            };
        }






}