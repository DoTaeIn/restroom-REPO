using UnityEngine;

public class Room : MonoBehaviour
{
    private int _id;
    private bool _hasKey;
    SizeSettings _furnitureSize;
    FurnitureManager _furnitureManager;

    private Vector2 GetRandomPos()
    {
        Vector2 randomPos;
        randomPos.x = Random.Range(_furnitureSize.XSize, _furnitureSize.Position.x + _furnitureSize.XSize);
        randomPos.y = Random.Range(_furnitureSize.YSize, _furnitureSize.Position.y + _furnitureSize.YSize);
        return randomPos;
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

    private void InitRoom()
    {
        foreach (Furniture furniture in _furnitureManager.furnitures)
        {
            //if(_furnitureManager.AddFurniture(furniture)){}
            
        }
    }
}
