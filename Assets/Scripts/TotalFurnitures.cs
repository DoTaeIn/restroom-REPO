using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Linq;
public class TotalFurnitures : MonoBehaviour
{
    List<Furniture> totalFurnitures = new List<Furniture>();
    private ProceduralMapGeneration proceduralMapGeneration;
    public List<Item> items;
    public Item keyItem;
    
    public int distributeAmt = 10;
    private string password;
    private int weaponCount = 0;
    int totalWeaponCount = 5;

    private void Awake()
    {
        proceduralMapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
    }

    private void OnEnable()
    {
        proceduralMapGeneration.OnMapGenerated += AddFurnitureManager;
    }
    
    public void SetPassword(int[] pass)
    {
        password = pass.ToString();
    }

    void AddFurnitureManager()
    {
        totalFurnitures = FindObjectsByType<Furniture>(
                FindObjectsInactive.Exclude, 
                FindObjectsSortMode.None
            )
            // Exclude all Furniture whose GameObject has the “Grabbable” tag
            .Where(f => !f.gameObject.CompareTag("Grabable") || !f.gameObject.CompareTag("Toilet"))
            .OrderBy(_ => UnityEngine.Random.value)
            .ToList();

        List<char> passChar = password.ToCharArray().ToList();

        for (int i = 0; i < totalFurnitures.Count; i++)
        {
            Furniture furniture = totalFurnitures[i];
            
            if (furniture.item != null) 
                continue;
            
            Item newItem;
            if (i < 6)
            {
                newItem = Instantiate(keyItem);
                newItem.type  = ItemType.Key;
                    
                int idx = i < 4 ? i : UnityEngine.Random.Range(0, 4);
                newItem.keyId  = passChar[idx] - '0';
                newItem.keyPos = idx;
                newItem.name   = "Key " + idx;
                Debug.Log($"Key is in: {furniture.name}");
            }
            else
            {
                // If we've hit the weapon cap, only pick from non-weapons
                List<Item> validItems;
                if (weaponCount >= totalWeaponCount)
                    validItems = items.Where(p => !p.gameObject.CompareTag("Weapon")).ToList();
                else
                    validItems = items;

                // If for some reason all remaining are weapons, bail out
                if (validItems.Count == 0)
                    continue;

                // Then pick from that filtered list
                var prototype = validItems[UnityEngine.Random.Range(0, validItems.Count)];

                // Now if it is actually a weapon, bump the count
                if (prototype.gameObject.CompareTag("Weapon"))
                    weaponCount++;

                newItem = Instantiate(prototype);
                newItem.name = "Item " + i;
            }
            furniture.item = newItem;
        }
    }
}
