using System;
using System.Collections.Generic;
using UnityEngine;



public class FurnitureManager : MonoBehaviour
{
    public List<Furniture> furnitures;
    public List<Furniture> placed = new List<Furniture>();
    
    
    
    
    public void AddFurniture(Furniture furniture)
    {
        placed.Add(furniture);
    }

    

}
