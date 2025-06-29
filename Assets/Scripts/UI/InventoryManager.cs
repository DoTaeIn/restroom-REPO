using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    GameHandler gameHandler;
    [SerializeField] private List<Slot> slots;
    [SerializeField] private int limit = 5;

    [SerializeField] private List<Item> items = new List<Item>();

    private int itemUseAmt = 0;

    private void Awake()
    {
        gameHandler = FindFirstObjectByType<GameHandler>();
    }

    private void Start()
    {
        itemUseAmt = 0;
        
    }

    public void AddItem(Item item)
    {
        if (items.Count < limit)
        {
            items.Add(item);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    public void UseItem(Item item)
    {
        if(itemUseAmt >= 4)
            gameHandler.handleGameOver(itemUseAmt);
        
        if(item.type == ItemType.Weapon)
            itemUseAmt++;
        
        
        if (items.Remove(item))
        {
            
            UpdateUI();
        }
    }

    public Item GetItem(int index)
    {
        Item item = slots[index].item;
        UseItem(item);
        return item;
    }

    private void UpdateUI()
    {
        int itemCount = items.Count;
        int slotCount = slots.Count;

        for (int i = 0; i < slotCount; i++)
        {
            if (i < itemCount)
            {
                slots[i].image.sprite = items[i].icon;
                slots[i].item = items[i];
                slots[i].image.color  = Color.white;
            }
            else
            {
                slots[i].image.sprite = null;
                slots[i].item = null;
                slots[i].image.color  = new Color(0, 0, 0, 0);
            }
        }
    }
}