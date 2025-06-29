using System;
using UnityEngine;

public enum ItemType
{
    Key,
    Weapon,
    Heal
}

public class Item : MonoBehaviour
{
    [Header("References")]
    private int _id;
    [SerializeField] private string name;
    public ItemType type;
    public Sprite[] iconSwap;
    public Sprite icon;
    SpriteRenderer spriteRenderer;
    
    [Header("Settings")]
    public float damage;
    public float healAmount;
    public string[] itemNames;
    public int keyPos = 4; //_XXX -> 1st => 0
    public int keyId; //key Num.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        icon = spriteRenderer.sprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyCtrl>() != null)
        {
            collision.gameObject.GetComponent<EnemyCtrl>().ChangeState(new ConcussionState(collision.gameObject.GetComponent<EnemyCtrl>()));
        }
    }

    public float Consume()
    {
        bool isBad = UnityEngine.Random.Range(0, 1) < 0.5f;
        
        if (isBad)
        {
            icon = iconSwap[0];
            name = itemNames[0];
            return healAmount;
            Debug.Log("Consumed a bad item!");
        }
        else
        {
            icon = iconSwap[1];
            name = itemNames[1];
            return -healAmount;
            Debug.Log("Consumed a good item!");
        }
    }
}
