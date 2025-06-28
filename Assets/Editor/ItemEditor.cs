using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    SerializedProperty iconSwapProp;
    SerializedProperty namesProp;
    void OnEnable()
    {
        iconSwapProp = serializedObject.FindProperty("iconSwap");
        namesProp = serializedObject.FindProperty("itemNames");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
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
            item.icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.icon, typeof(Sprite), false);
            EditorGUILayout.PropertyField(iconSwapProp, new GUIContent("Icon Swap"), true);
            item.healAmount = EditorGUILayout.FloatField("Heal Amount", item.healAmount);
            EditorGUILayout.PropertyField(namesProp, new GUIContent("Item Names"), true);

        }
        else if (item.type == ItemType.Weapon)
        {
            item.damage = EditorGUILayout.FloatField("Damage", item.damage);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
