using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSettings
{
    public Vector2 pos;
    public Quaternion rotation;
    public Furniture furniture;
}

public class FurnitureManager : MonoBehaviour
{
    public List<Furniture> furnitures;
    private List<FurnitureSettings> _furnitures;

    public FurnitureSettings AddFurniture(Furniture furniture, Vector2 pos, Quaternion rotation)
    {
        foreach (FurnitureSettings f in _furnitures)
        {
            if(!f.furniture.IsInterfering(f.furniture.FurnitureSize))
            {
                FurnitureSettings furnitureSettings = new FurnitureSettings();
                furnitureSettings.pos = pos;
                furnitureSettings.rotation = rotation;
                furnitureSettings.furniture = furniture;
                _furnitures.Add(furnitureSettings);
                return furnitureSettings;
            }
        }
        return null;
    }

}
