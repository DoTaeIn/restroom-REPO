using System;
using System.Collections.Generic;
using UnityEngine;



public class FurnitureManager : MonoBehaviour
{
    public List<Furniture> furnitures;
    public List<Item> items;
    public Item keyItem;
    public List<Furniture> placed = new List<Furniture>();
    [Range(6, 100)]
    public int distributeAmt = 10;
    private string password;
    
    public void SetPassword(int pass)
    {
        password = pass.ToString();
    }
    
    public void AddFurniture(Furniture furniture)
    {
        placed.Add(furniture);
    }

    public void Distribute()
    {
        List<char> passChar = new List<char>(password.ToCharArray());
        foreach (Furniture furniture in placed)
        {
            for (int i = 0; i < distributeAmt; i++)
            {
                if (i < 6)
                {
                    if (furniture.item == null)
                    {
                        if(i < 4)
                        {
                            Item item = Instantiate(keyItem);
                            item.type = ItemType.Key;
                            item.keyId = i;
                            item.keyPos = passChar[i] - '0'; // Convert char to int
                            item.name = "Key " + i;
                        }
                        else
                        {
                            Item item = Instantiate(keyItem);
                            int randomIndex = UnityEngine.Random.Range(0, 4);
                            item.type = ItemType.Key;
                            item.keyId = randomIndex;
                            item.keyPos = passChar[randomIndex] - '0'; // Convert char to int
                            item.name = "Key " + randomIndex;
                        }
                    }
                }
                else
                {
                    if (furniture.item == null)
                    {
                        furniture.item = items[UnityEngine.Random.Range(0, items.Count)];
                        furniture.item.name = "Item " + i;
                    }
                }
            }
        }
    }

}
