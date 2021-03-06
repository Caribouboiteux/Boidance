﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;
using System.Linq;
using System;

[CustomEditor(typeof(Vector2VRInteraction))]

public class Vector2VRInteractionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Vector2VRInteraction vi = (Vector2VRInteraction)target;
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        var properties = typeof(CommonUsages).GetFields().Where(field => field.FieldType.ToString() == "UnityEngine.XR.InputFeatureUsage`1["+vi.regexName+"]");
        vi.valueFields = (List<string>)properties.Select(prop => prop.Name).ToList();
        vi.data.propIndex = EditorGUILayout.Popup(vi.data.propIndex, vi.valueFields.ToArray<string>());
        string name = vi.valueFields[vi.data.propIndex];
        vi.featureValue = (InputFeatureUsage<Vector2>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(vi);
        }
    }
}
