using UnityEngine;

public class SizeSettings
{
    public float XSize;
    public float YSize;
    public Vector3 Position;
    public Quaternion Rotation;
}

public class Furniture : MonoBehaviour
{
    public SizeSettings FurnitureSize;

    public bool IsInterfering(SizeSettings position)
    {
        // Check if the furniture's position and size interfere with the given position and size
        float xDistance = Mathf.Abs(FurnitureSize.XSize - position.XSize);
        float yDistance = Mathf.Abs(FurnitureSize.YSize - position.YSize);
        
        // Assuming a simple bounding box check for interference
        return xDistance < (FurnitureSize.XSize / 2 + position.XSize / 2) &&
               yDistance < (FurnitureSize.YSize / 2 + position.YSize / 2);
    }
}
