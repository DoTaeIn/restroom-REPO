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

    private void Awake()
    {
        proceduralMapGeneration = FindFirstObjectByType<ProceduralMapGeneration>();
    }

    private void OnEnable()
    {
        proceduralMapGeneration.OnMapGenerated += AddFurnitureManager;
    }
    
    public void SetPassword(int pass)
    {
        password = pass.ToString();
    }

    void AddFurnitureManager()
    {
        totalFurnitures = FindObjectsByType<Furniture>(
            FindObjectsInactive.Exclude, 
            FindObjectsSortMode.None
        ).OrderBy(_ => UnityEngine.Random.value).ToList();

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
            }
            else
            {
                var prototype = items[UnityEngine.Random.Range(0, items.Count)];
                newItem = Instantiate(prototype);
                newItem.name = "Item " + i;
            }
            furniture.item = newItem;
        }
    }
}
