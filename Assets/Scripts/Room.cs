using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.AI.Navigation;
using Unity.VisualScripting;
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
    public TileBase wallLeftCornerTile;
    public TileBase WallRightCornerTile;
    public TileBase topWallTile;
    public TileBase leftWallTile;
    public TileBase leftnoWallTile;
    public TileBase rightWallTile;
    public TileBase leftTopWallTile;
    public TileBase rightTopWallTile;
    public TileBase rightnoWallTile;
    public TileBase bottomWallTile;
    public TileBase rightBottomWallTile;
    public TileBase leftBottomWallTile;
    public TileBase wallBottomTile;
    public GameObject doorPrefab;
    public Vector2Int gridPosition;
    PolygonCollider2D polygon;
    private ProceduralMapGeneration mapGeneration;
    NavMeshSurface surface;

    private void Awake()
    {
        roomSize = new SizeSettings();
        wallTilemap = GameObject.FindGameObjectWithTag("Wall").GetComponent<Tilemap>();
        floorTilemap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        polygon = GetComponent<PolygonCollider2D>();
        _furnitureManager = GetComponent<FurnitureManager>();
        mapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
        surface = GameObject.FindGameObjectWithTag("Floor").GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        //surface.BuildNavMesh();
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
        roomSize.XSize = Random.Range(16, 20);
        roomSize.YSize = Random.Range(12, 16);
        roomSize.Position = new Vector2(startX, startY);
        roomSize.Rotation = Quaternion.identity;

        GenerateWalls(startX, startY);
        this._id = id;
        Vector3Int asdasd = new Vector3Int(startX, startY, 0);
        GenerateDoors(startX, startY, Mathf.RoundToInt(roomSize.XSize), Mathf.RoundToInt(roomSize.YSize));
        PlaceFurnitureRandomly();

    }

    public void PlaceFurnitureRandomly()
    {
        int maxFurnitureCount = 4; // 최대 배치할 가구 수
        int placedFurnitureCount = 0;
        var shuffledFurnitures = new List<Furniture>(_furnitureManager.furnitures);
        for (int i = 0; i < shuffledFurnitures.Count; i++)
        {
            int rnd = Random.Range(i, shuffledFurnitures.Count);
            (shuffledFurnitures[i], shuffledFurnitures[rnd]) = (shuffledFurnitures[rnd], shuffledFurnitures[i]);
        }

        foreach (Furniture furniturePrefab in shuffledFurnitures)
        {

            if (placedFurnitureCount >= maxFurnitureCount)
                break;

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
                    placedFurnitureCount++;
                }

                attempts++;
            }
        }
    }

    public void GenerateLivingRoom(int startx, int starty, int width, int height)
    {
        roomSize.XSize = width;
        roomSize.YSize = height;
        GenerateWalls(startx, starty);
        Debug.Log(startx.ToString()+starty+ToString()+width.ToString()+height.ToString());


        Vector3 TopDoormidliv = new Vector3(startx + width / 2 , starty + height , 0);
        _id = -3;

        CreateDoorAt(new Vector3Int(startx + width / 2, starty + height , 0),"Top"); // 위쪽 중앙에 문 생성
        d = CreateDoorObject("TopDoor", TopDoormidliv, this);
        d.parentRoom = this;
        d.direction = "Top";
        d.doorpos = TopDoormidliv;
        doors.Add(d);
        
        
        Vector3 BottomDoormidliv = new Vector3(startx + width / 2 , starty-1f, 0);
        CreateDoorAt(new Vector3Int(startx + width / 2, starty-1, 0),"Bottom"); // 아래쪽 중앙에 문 생성
        d = CreateDoorObject("BottomDoor", BottomDoormidliv, this);
        d.parentRoom = this;
        d.direction = "Bottom";
        d.doorpos = BottomDoormidliv;
        doors.Add(d);
        // PlaceFurnitureRandomly();
    }


    public void GenerateWalls(int startx, int starty)
    {
        int x, y;
        for (x = startx-1; x < startx + roomSize.XSize+1; x++) //위아래 벽타일 생성
        {
            y = starty + (int)roomSize.YSize+3;  //top 채우기
            if (x == startx-1)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), leftTopWallTile);
            }
            else if (x == startx + roomSize.XSize)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), rightTopWallTile);
            }
            else
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), topWallTile);
            }


            y = starty-1; //bottom 채우기
            if (x == startx-1)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), leftBottomWallTile);
            }
            else if (x == startx + roomSize.XSize)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), rightBottomWallTile);
            }
            else
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), bottomWallTile);
            }
        }

        for (y = starty; y < starty + roomSize.YSize+1; y++) //좌우 벽타일 생성
        {
            x = startx-1;
            wallTilemap.SetTile(new Vector3Int(x, y, 0), leftWallTile);
            x = startx + (int)roomSize.XSize;
            wallTilemap.SetTile(new Vector3Int(x, y, 0), rightWallTile);
        }

        y = starty + (int)roomSize.YSize;
        x = startx - 1;
        wallTilemap.SetTile(new Vector3Int(x, y, 0), wallLeftCornerTile);
        x = startx + (int)roomSize.XSize;
        wallTilemap.SetTile(new Vector3Int(x, y, 0), WallRightCornerTile);


        for (y = starty+(int)roomSize.YSize+1; y < starty + roomSize.YSize + 3; y++) //좌우 벽타일 생성 (위쪽 줄 없는 벽)
        {
            x = startx - 1;
            wallTilemap.SetTile(new Vector3Int(x, y, 0), leftnoWallTile);
            x = startx + (int)roomSize.XSize;
            wallTilemap.SetTile(new Vector3Int(x, y, 0), rightnoWallTile);
        }
        for (x = startx; x < startx + roomSize.XSize; x++) //벽 전면 타일 생성
        {
            y = starty + (int)roomSize.YSize;
            wallTilemap.SetTile(new Vector3Int(x, y, 0), wallBottomTile);
            for (y = starty+(int)roomSize.YSize+1; y < starty+(int)roomSize.YSize+3; y++)
            {

                wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);

            }
        }
        
        for (x = startx; x < startx+roomSize.XSize; x++)
        {   
            for (y = starty; y < starty+roomSize.YSize; y++)
            {
                // 바닥 타일 생성
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
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

        
    }


    public void CreateToilet(int id, int startx, int starty)
    {
        roomSize.XSize = 10;
        roomSize.YSize = 10;
        roomSize.Position = new Vector2(startx, starty);
        roomSize.Rotation = Quaternion.identity;
        GenerateWalls(startx, starty);  
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

        this._id = id;
        Vector3Int asdasd = new Vector3Int(startx, starty, 0);
        GenerateDoors(startx, starty, Mathf.RoundToInt(roomSize.XSize), Mathf.RoundToInt(roomSize.YSize));

        Furniture toiletFurniture = _furnitureManager.furnitures[0]; // 변기만 들어있으니까

        Vector3 centerPos = new Vector3( //중간에 변기 설치
            roomSize.Position.x + roomSize.XSize / 2f,
            roomSize.Position.y + roomSize.YSize / 2f,
            0
        );

        Quaternion rot = Quaternion.identity;

        GameObject toiletObj = Instantiate(toiletFurniture.gameObject, centerPos, rot);
        toiletObj.transform.SetParent(transform);
        _furnitureManager.AddFurniture(toiletObj.GetComponent<Furniture>());


    }

    public Sprite doorSprite,sideDoorSprite;
    public List<Door> doors = new List<Door>();
    private Door d;
    private void GenerateDoors(int startX, int startY, int width, int height)
    {
        if ((startX - 5) / mapGeneration.roomGap != 0)
        {
            // 왼쪽(Left) 벽에서 랜덤 y
            int leftY = Random.Range(startY + 1, startY + height - 1);
            Vector3Int leftDoor = new Vector3Int(startX-1, leftY, 0);
            Vector3 leftDoormid = new Vector3(startX - 1f, leftY, 0);
            d = CreateDoorObject("LeftDoor", leftDoormid, this);
            d.parentRoom = this;
            d.direction = "Left";
            d.doorpos = leftDoormid;
            doors.Add(d);
            CreateDoorAt(leftDoor,"Left");
        }
        if ((startX - 5) / mapGeneration.roomGap != 4)
        {

            // 오른쪽(Right) 벽에서 랜덤 y
            int rightY = Random.Range(startY + 1, startY + height - 1);
            Vector3Int rightDoor = new Vector3Int(startX + width, rightY, 0);
            Vector3 rightDoormid = new Vector3(startX + width + 0.5f, rightY, 0);
            d = CreateDoorObject("RightDoor", rightDoormid, this);
            d.parentRoom = this;
            d.direction = "Right";
            doors.Add(d);
            d.doorpos = rightDoormid;
            CreateDoorAt(rightDoor,"Right");
        }
        if (((startY - 5) / mapGeneration.roomGap != 0) || ((startX -5)/mapGeneration.roomGap == 2))
        {
            // 아래쪽(Bottom) 벽에서 랜덤 x
            int bottomX = Random.Range(startX + 1, startX + width - 1);
            Vector3Int bottomDoor = new Vector3Int(bottomX, startY-1, 0);
            Vector3 bottomDoormid = new Vector3(bottomX, startY - 1f, 0);
            d = CreateDoorObject("BottomDoor", bottomDoormid, this);
            d.parentRoom = this;
            d.direction = "Bottom";
            doors.Add(d);
            d.doorpos = bottomDoormid;
            CreateDoorAt(bottomDoor,"Bottom");

        }
        if ((startY - 5) / mapGeneration.roomGap != mapGeneration.numberOfRooms / 5 - 1)
        {
            // 위쪽(Top) 벽에서 랜덤 x
            int topX = Random.Range(startX + 1, startX + width - 1);
            Vector3Int topDoor = new Vector3Int(topX, startY + height - 1, 0);
            Vector3 topDoormid = new Vector3(topX, startY + height, 0);
            d = CreateDoorObject("TopDoor", topDoormid, this);
            d.parentRoom = this;
            d.direction = "Top";
            doors.Add(d);
            d.doorpos = topDoormid;
            CreateDoorAt(topDoor,"Top");
        }
    }

    void CreateDoorAt(Vector3Int Door, string direction)
    {
        Vector3Int offset1, offset2;
        if (direction == "Top" || direction == "Bottom") //위/ 아래일때
        {
            offset1 = new Vector3Int(1, 0, 0);
            offset2 = new Vector3Int(0, 0, 0); 
        }
        else // "Left" or "Right" 왼쪽 오른쪽일때
        {
            // 세로 2칸 제거 (↑ 방향으로)
            offset1 = new Vector3Int(0, 1, 0);
            offset2 = new Vector3Int(0, 0, 0);
        }
        Vector3Int[] positions = {Door + offset2, Door + offset1 }; //위치 저장

        foreach (var pos in positions)   
        {
            wallTilemap.SetTile(pos, null);  //지우고
            floorTilemap.SetTile(pos, floorTile);  //대체하고
        }
    }


    public Door CreateDoorObject(string name, Vector3 worldPosition, Room parent)
    {
        GameObject door = new GameObject(name);
        door.transform.position = worldPosition;
        door.transform.parent = this.transform; // 방 내부 구조로 귀속
        
        Door doorComponent = door.AddComponent<Door>();

        doorComponent.roomID = _id;
        doorComponent.parentRoom = parent;
        doorComponent.direction = name.Replace("Door", ""); // ex: "Top", "Left" 더 쉽게 바꾼거라는데 몰라 gpt가 바꿈ㅋㅋ굿

        // 충돌 박스 추가
        BoxCollider2D col = door.AddComponent<BoxCollider2D>();
        col.isTrigger = false;

        // rigidbody2D 추가
        Rigidbody2D rb = door.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        // 태그 지정
        door.tag = "Door";

        // 스프라이트 렌더러 추가
        SpriteRenderer sr = door.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 10; // 배경 위에 뜨게 하고 싶으면 정렬 순서 설정


        switch (doorComponent.direction)
        {
            case "Top":
                sr.sprite = doorSprite;
                door.transform.position = worldPosition + new Vector3(1f, 2f, 0); // 중심 조정
                col.size = new Vector2(2f, 4f);
                col.offset = new Vector2(0f, 0f);
                break;

            case "Bottom":
                sr.sprite = sideDoorSprite;
                door.transform.localRotation = Quaternion.Euler(0, 0, 180);
                door.transform.position = worldPosition + new Vector3(1f, 0.25f, 0);
                col.size = new Vector2(2f, 0.5f);
                col.offset = new Vector2(0f, 0f);
                break;

            case "Left":
                sr.sprite = sideDoorSprite;
                door.transform.localRotation = Quaternion.Euler(0, 0, 90);
                door.transform.position = worldPosition + new Vector3(0.25f, 1f, 0);
                col.size = new Vector2(2f, 0.5f);
                col.offset = new Vector2(0f, 0f);
                break;

            case "Right":
                sr.sprite = sideDoorSprite;
                door.transform.localRotation = Quaternion.Euler(0, 0, -90);
                door.transform.position = worldPosition + new Vector3(0.25f, 1f, 0);
                col.size = new Vector2(2f, 0.5f);
                col.offset = new Vector2(0f, 0f);
                break;
        }

        return doorComponent;
    }


    public void SetupDoors()
    {
        Dictionary<Vector2Int, Room> allRooms;
        allRooms = RoomManager.Instance.allRoomsV;
        foreach (var pair in allRooms)
        {
            Vector2Int roomkey = pair.Key;
            Room room = pair.Value;
            foreach (Door door in room.doors)
            {
                door.destinationRoomID = room._id;
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