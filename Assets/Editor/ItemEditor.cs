using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Item item = (Item)target;

        //example.alwaysVisible = EditorGUILayout.IntField("Always Visible", example.alwaysVisible);
        item.type = item.type = (ItemType)EditorGUILayout.EnumFlagsField("Type", item.type);
        
        if (item.type == ItemType.Key)
        {
            item.keyPos = EditorGUILayout.IntField("Key Num Pos", item.keyPos);
            item.keyId = EditorGUILayout.IntField("Key Variable", item.keyId);
        }
        else if (item.type == ItemType.Heal)
        {
            item.healAmount = EditorGUILayout.FloatField("Heal Amount", item.healAmount);
        }
        else if (item.type == ItemType.Weapon)
        {
            item.damage = EditorGUILayout.FloatField("Damage", item.damage);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
    }
}
