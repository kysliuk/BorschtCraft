using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BorschtCraft.Food.SlotConfig))]
public class SlotConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var slotTypeProp = serializedObject.FindProperty("SlotType");
        EditorGUILayout.PropertyField(slotTypeProp);

        if ((BorschtCraft.Food.SlotType)slotTypeProp.enumValueIndex == BorschtCraft.Food.SlotType.Cooking)
        {
            var cookingTimeProp = serializedObject.FindProperty("CookingTime");
            EditorGUILayout.PropertyField(cookingTimeProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
