using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<Slot> slots;
    private List<Item> items;
    private int limit = 5;
    

    public void AddItem(Item item)
    {
        if(items.Count <= limit)
        {
            items.Add(item);
        }
        
        UpdateUI();
    }

    public void UseItem(Item item)
    {
        if(items.Contains(item))
            items.Remove(item);

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Slot slot in slots)
        {
            if (slots.IndexOf(slot) < items.Count)
            {
                slot.image.sprite = items[slots.IndexOf(slot)].icon;
                slot.image.color = Color.white; // Ensure the image is visible
            }
            else
            {
                slot.image.sprite = null;
                slot.image.color = new Color(0, 0, 0, 0); // Make the image invisible
            }
        }
    }
}