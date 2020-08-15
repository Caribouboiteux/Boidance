using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class AddVRInputUI : AddInteractionUI<BoidInteraction>
{
    
    public enum AddTypes{ Bool=0, Float=1, Vector2 = 2, Vector3 = 3, Quaternion=4};

    public static FieldInfo[] inputs;
    public static string[] inputTypes = new string[] { "Bool", "Float", "Vector2", "Vector3", "Quaternion" };
    public static string[] directionNames = new string[] { "all", "x", "y", "z"};
    public static string[] typeNames = new string[] { "System.Boolean", "System.Single", "UnityEngine.Vector2", "UnityEngine.Vector3", "UnityEngine.Quaternion" };
    public static string[] deviceNames = new string[] { "BothControllers", "LeftController", "RightController", "Headset"};
    public static string[] deviceShortNames = new string[] { "LR", "L", "R", "H"};
    public AddTypes addTypes;
    public override void PopulatePrefabUI()
    {
        for (int i = 0; i < targetPrefabObject.Length; i++)
        {
            targetPrefabs[i] = (BoidInteraction)targetPrefabObject[i];
        }
        for (int i = 0; i < parent.saveSystem.CurrentData.VRInteractions.Count; i++)
        {
            OnAddButtonClicked(parent.saveSystem.CurrentData.VRInteractions[i]);
        }
        inputs = typeof(CommonUsages).GetFields();
    }
    public void OnAddButtonClicked(BoidInteraction targetInputInteraction)
    {
        GameObject newInput = Instantiate<GameObject>(displayUIPrefab);
        newInput.transform.SetParent(contentDisplay.transform);
        newInput.transform.localScale = Vector3.one;
        newInput.SetActive(true);
        InputVRUI input = newInput.GetComponent<InputVRUI>();
        input.addInteractionParentUI = this;
        input.interaction = targetInputInteraction;
        input.typeIndex = Array.IndexOf(typeNames, targetInputInteraction.data.regexName);
        prefabUIList.Add(input);
    }
    public override void UpdateInteractionHandler(InteractionUI<BoidInteraction> interactionUI)
    {
        parent.saveSystem.CurrentData.addVrInteraction(null, true);
    }

    public override void UpdatePreferences()
    {
    }
}
