using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehavior))]

public class CompositeBehaviorEditor : Editor
{


    public override void OnInspectorGUI()
    {
        CompositeBehavior cb = (CompositeBehavior)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("Composite Behavior Inspector", EditorStyles.boldLabel);
        if (cb.behaviors == null || cb.behaviors.Length == 0)
        {
            GUILayout.Space(10f);
            EditorGUILayout.HelpBox("No Behaviors in array.", MessageType.Warning);
        }
        else
        {
            GUILayout.Space(10f);
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behaviors.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                cb.behaviors[i] = (FlockBehavior)EditorGUILayout.ObjectField(cb.behaviors[i], typeof(FlockBehavior), false);
                cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i]);
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            cb.AddBehavior();
            EditorUtility.SetDirty(cb);
        }
        GUI.enabled = !(cb.behaviors == null || cb.behaviors.Length == 0);
        if (GUILayout.Button("Remove"))
        {
            RemoveBehavior(cb);
            EditorUtility.SetDirty(cb);
        }
        GUI.enabled = true;

        GUILayout.EndHorizontal();

    }


    void RemoveBehavior(CompositeBehavior cb)
    {
        int oldCount = cb.behaviors.Length;
        if (oldCount == 1)
        {
            cb.behaviors = null;
            cb.weights = null;
            return;
        }

        FlockBehavior[] newBehaviors = new FlockBehavior[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }
        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }
}
