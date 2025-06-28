using Unity.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string destinationRoomID;
    public Vector3 destinationPosition;
    public int roomID;
    public Room parentRoom;
    public string direction; // "Top", "Bottom", "Left", "Right"
    public Door connectedDoor; // 워프할 대상 문

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
                    offset = Vector3.down * 1.5f;
                    break;
                case "Left":
                    offset = Vector3.left * 1.5f;
                    break;
                case "Right":
                    offset = Vector3.right * 1.5f;
                    break;
            }

            Debug.Log($"Player entered door: {gameObject.name}, warping to {connectedDoor.gameObject.name}");
            collision.transform.position = connectedDoor.transform.position + offset;
        }
    }
}