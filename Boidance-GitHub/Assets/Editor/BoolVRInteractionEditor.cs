using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;
using System.Linq;
using System;

[CustomEditor(typeof(BoolVRInteraction))]

public class BoolVRInteractionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        BoolVRInteraction vi = (BoolVRInteraction)target;
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        var properties = typeof(CommonUsages).GetFields().Where(field => field.FieldType.ToString() == "UnityEngine.XR.InputFeatureUsage`1["+vi.regexName+"]");
        vi.valueFields = (List<string>)properties.Select(prop => prop.Name).ToList();
        vi.data.propIndex = EditorGUILayout.Popup(vi.data.propIndex, vi.valueFields.ToArray<string>());
        string name = vi.valueFields[vi.data.propIndex];
        vi.featureValue = (InputFeatureUsage<bool>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(vi);
        }
    }
}
