using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;
using System.Linq;
using System;

[CustomEditor(typeof(QuaternionVRInteraction))]

public class QuaternionVRInteractionEditor : Editor
{

    public override void OnInspectorGUI()
    {
        QuaternionVRInteraction vi = (QuaternionVRInteraction)target;
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        var properties = typeof(CommonUsages).GetFields().Where(field => field.FieldType.ToString() == "UnityEngine.XR.InputFeatureUsage`1[" + vi.regexName + "]");
        vi.valueFields = (List<string>)properties.Select(prop => prop.Name).ToList();
        vi.data.propIndex = EditorGUILayout.Popup(vi.data.propIndex, vi.valueFields.ToArray<string>());
        string name = vi.valueFields[vi.data.propIndex];
        vi.featureValue = (InputFeatureUsage<UnityEngine.Quaternion>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(vi);
        }
    }
}
