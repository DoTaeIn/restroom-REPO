using UnityEngine;

public class Door : MonoBehaviour
{
    public string destinationRoomID;
    public Vector3 destinationPosition;
    

    public void Doorwarp()
    {
        RoomManager.Instance.WarpToRoom(destinationRoomID, destinationPosition);
    }
}