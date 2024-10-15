using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GearGeneration), true)]
[CanEditMultipleObjects]
public class GearGenerationEditor : Editor
{
    SerializedProperty noTeeth, cogBody, cogTeeth, outerRingRatio;
    private void OnEnable()
    {
        noTeeth = serializedObject.FindProperty("n_teeth");
        cogBody = serializedObject.FindProperty("t_cogBody");
        cogTeeth = serializedObject.FindProperty("t_cogTeeth");
    }
    public override void OnInspectorGUI()
    {
        GearGeneration gearGeneration = (GearGeneration)target;
        EditorGUILayout.PropertyField(noTeeth, new GUIContent("Number of Teeth"));
        EditorGUILayout.PropertyField( cogBody, new GUIContent("Gear's Body Template"));
        EditorGUILayout.PropertyField( cogTeeth, new GUIContent("Gear's Teeth Template"));
        if (gearGeneration is RingGearGeneration)
        {
            outerRingRatio = serializedObject.FindProperty("outerRingRatio");
            EditorGUILayout.PropertyField(outerRingRatio, new GUIContent("Ring Gear Outer Thiccness"));
        }

        if (GUILayout.Button("Generate Cog"))
        {
            if (AreInputsValid())
            {
                gearGeneration.Generate();
            }
        }
        serializedObject.ApplyModifiedProperties();
    } 
    private bool AreInputsValid()
    {
        if ((GameObject)cogBody.objectReferenceValue == null)
        {
            EditorUtility.DisplayDialog("Error!", "Please assign a Prefab or Scene Template for the Cog's Body. ", "Gotcha.");
            return false;
        }
        if ((GameObject)cogTeeth.objectReferenceValue == null)
        {
            EditorUtility.DisplayDialog("Error!", "Please assign a Prefab or Scene Template for the Cog's Teeth. ", "Gotcha.");
            return false;
        }
        return true;

    }
}
