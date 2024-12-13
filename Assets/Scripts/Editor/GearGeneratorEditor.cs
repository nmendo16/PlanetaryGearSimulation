using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlanetGearGenerator), true)]
[CanEditMultipleObjects]
public class GearGeneratorEditor : Editor
{
    SerializedProperty noTeeth, cogBody, cogTeeth, outerRingRatio;
    private void OnEnable()
    {
        noTeeth = serializedObject.FindProperty("n_cogs");
        cogBody = serializedObject.FindProperty("obj_gear");
        cogTeeth = serializedObject.FindProperty("obj_cog");
    }
    public override void OnInspectorGUI()
    {
        PlanetGearGenerator GearGenerator = (PlanetGearGenerator)target;
        EditorGUILayout.PropertyField(noTeeth, new GUIContent("Number of Cogs"));
        EditorGUILayout.PropertyField(cogBody, new GUIContent("Gear's Body Template"));
        EditorGUILayout.PropertyField(cogTeeth, new GUIContent("Cog Template"));
        if (GearGenerator is RingGearGenerator)
        {
            outerRingRatio = serializedObject.FindProperty("outerRingRatio");
            EditorGUILayout.PropertyField(outerRingRatio, new GUIContent("Ring Gear Outer Thickness"));
        }

        if (GUILayout.Button("Generate Cog"))
        {
            if (AreInputsValid())
            {
                GearGenerator.Generate();
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