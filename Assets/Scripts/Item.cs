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
    public Sprite icon;
    
    [Header("Settings")]
    public float damage;
    public float healAmount;
    public int keyPos = 4; //_XXX -> 1st => 0
    public int keyId; //key Num.
}
