using System;
using System.Collections;
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

    public bool isThrown;

    public bool isAquired;
    //public GameObject furniturePrefab;
    public SizeSettings FurnitureSize;
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private ParticleSystem particle;
    public Item item;
    BoxCollider2D _collider;
    //public Transform parent;

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
    
    public Item GetItem()
    {
        isAquired = true;
        if (item != null)
        {
            return item;
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") && isThrown)
        {
            particle.Play();
            Invoke("Remove", 0.2f);
        }
    }

    void Remove()
    {
        Destroy(this.gameObject);
    }
}
