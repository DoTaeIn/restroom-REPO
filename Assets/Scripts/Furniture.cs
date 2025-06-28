using System;
using System.Collections.Generic;
using UnityEngine;

public class SizeSettings
{
    public float XSize;
    public float YSize;
    public Vector3 Position;
    public Quaternion Rotation;
    
    public float Left => Position.x - 1;
    public float Right => Position.x + XSize + 1 ;
    public float Bottom => Position.y - 1;
    public float Top => Position.y + YSize + 1;
}

public class Furniture : MonoBehaviour
{
    private int _id;
    private string _name;
    //public GameObject furniturePrefab;
    public SizeSettings FurnitureSize;
    [SerializeField] private Sprite[] animationSprites;
    GameObject _item;
    BoxCollider2D _collider;
    public Transform parent;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        FurnitureSize = new SizeSettings
        {
            XSize = _collider.size.x,
            YSize = _collider.size.y,
            Position = this.transform.position,
            Rotation = this.transform.rotation
        };
    }

    public bool IsInterfering(SizeSettings other)
    {
        bool xOverlap = FurnitureSize.Left < other.Right && FurnitureSize.Right > other.Left;
        bool yOverlap = FurnitureSize.Bottom < other.Top && FurnitureSize.Top > other.Bottom;

        return xOverlap && yOverlap;
    }


}
