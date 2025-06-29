using System.ComponentModel;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int destinationRoomID;
    public Vector3 destinationPosition;
    public int roomID;
    public Room parentRoom;
    public string direction; // "Top", "Bottom", "Left", "Right"
    public Door connectedDoor; // 워프할 대상 문
    public Vector3 doorpos;
    public PlayerCtrl player;
    private MiniMapManager minimapmanager;
    private ProceduralMapGeneration proceduralMapGeneration;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerCtrl>();
        minimapmanager = FindFirstObjectByType<MiniMapManager>();
        proceduralMapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && connectedDoor != null)
        {
            Vector3 offset = Vector3.zero;

            switch (direction)
            {
                case "Top":
                    offset = Vector3.up * 1.5f;
                    break;
                case "Bottom":
                    offset = Vector3.down * 3f;
                    break;
                case "Left":
                    offset = Vector3.left * 1.5f;
                    break;
                case "Right":
                    offset = Vector3.right * 1.5f;
                    break;
            }

            player.currentRoom = connectedDoor.parentRoom;

            collision.transform.position = connectedDoor.transform.position + offset;
            minimapmanager.HighlightRoom(connectedDoor.parentRoom.gridPosition);
            minimapmanager.FocusOnRoom(connectedDoor.parentRoom.gridPosition);
            if (connectedDoor.parentRoom._id == proceduralMapGeneration.toiletSeed)
            {
                minimapmanager.ReplaceRoomIcon(new Vector2Int(proceduralMapGeneration.toiletSeed % 5 + 1, proceduralMapGeneration.toiletSeed/5 + 1));
            }
        }
    }
}