using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System.IO;
using TMPro;
using System.Linq;
using System.Dynamic;
using System;

public class InputVRUI : InteractionUI<BoidInteraction>
{
    public TMP_Dropdown inputTypeDropdown;
    public int typeIndex;
    public TMP_Dropdown inputDropdown;
    public int inputIndex;
    public TMP_Dropdown deviceDropdown;
    public int deviceIndex;
    public TMP_Dropdown directionDropdown;
    public int directionIndex;
    public TMP_InputField nameField;
    List<string> inputOptions;

    public BoidInteraction interaction;

    private void Start()
    {
        // delete button
        deleteButton.onClick.AddListener(DeleteInteraction);

        // type dropdown
        inputTypeDropdown.onValueChanged.AddListener(delegate { OnVRTypeChanged(inputTypeDropdown); });
        inputTypeDropdown.options.Clear();
        for (int i = 0; i < AddVRInputUI.inputTypes.Length; i++)
        {
            inputTypeDropdown.options.Add(new TMP_Dropdown.OptionData() { text = AddVRInputUI.inputTypes[i] });
        }
        inputTypeDropdown.value = typeIndex;
        if (interaction == null)
        {
            OnVRTypeChanged(inputTypeDropdown);
        }

        // input dropdown
        inputDropdown.onValueChanged.AddListener(delegate { OnInputChanged(inputDropdown); });
        UpdateInput(interaction.data.propIndex);
        deviceDropdown.options.Clear();
        for (int i = 0; i < AddVRInputUI.deviceNames.Length; i++)
        {
            deviceDropdown.options.Add(new TMP_Dropdown.OptionData() { text = AddVRInputUI.deviceNames[i] });
        }
        deviceDropdown.value = GetDeviceNameIndex();
        deviceDropdown.onValueChanged.AddListener(delegate { OnDeviceChanged(deviceDropdown); });

        // direction dropdown
        directionDropdown.onValueChanged.AddListener(delegate { OnDirectionChange(directionDropdown); });
        directionDropdown.options.Clear();
        for (int i = 0; i < AddVRInputUI.directionNames.Length; i++)
        {
            directionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = AddVRInputUI.directionNames[i] });
        }
        directionDropdown.value = GetDirectionIndex();

        // name textField
        if (interaction.data.interactionName.Equals("Default")){
            nameField.text = GetGenericName();
            OnNameChanged();
        }
        else
        {
            nameField.text = interaction.data.interactionName;
        }
        nameField.onValueChanged.AddListener(delegate { OnNameChanged(); });
    }
    public override void DeleteInteraction()
    {
        if (addInteractionParentUI != null)
        {
            addInteractionParentUI.parent.saveSystem.CurrentData.RemoveVrInteraction(interaction);
            addInteractionParentUI.prefabUIList.Remove(this);
        }
        Destroy(this.gameObject);
    }

    public void OnDirectionChange(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 1:
                interaction.data.maskVector = Vector3.right;
                break;

            case 2:
                interaction.data.maskVector = Vector3.up;
                break;

            case 3:
                interaction.data.maskVector = Vector3.forward;
                break;
            default:
                interaction.data.maskVector = Vector4.one;
                break;
        }
    }

    private void OnVRTypeChanged(TMP_Dropdown change)
    {
        addInteractionParentUI.parent.saveSystem.CurrentData.RemoveVrInteraction(interaction);
        addInteractionParentUI.prefabUIList.Remove(this);
        switch (change.value)
        {
            case 0:
                interaction = ScriptableObject.CreateInstance<BoolVRInteraction>();
                break;
            case 1:
                interaction = ScriptableObject.CreateInstance<FloatVRInteraction>();
                break;
            case 2:
                interaction = ScriptableObject.CreateInstance<Vector2VRInteraction>();
                break;
            case 3:
                interaction = ScriptableObject.CreateInstance<Vector3VRInteraction>();
                break;
            default:
                interaction = ScriptableObject.CreateInstance<QuaternionVRInteraction>();
                break;
        }
        addInteractionParentUI.parent.saveSystem.CurrentData.addVrInteraction(interaction, true);
        addInteractionParentUI.prefabUIList.Add(this);
        UpdateInput(0);
        OnDeviceChanged(deviceDropdown);
        nameField.text = GetGenericName();
    }
    private void UpdateInput(int index)
    {
        var properties = typeof(CommonUsages).GetFields().Where(field => field.FieldType.ToString() == "UnityEngine.XR.InputFeatureUsage`1[" + interaction.getRegexName() + "]");
        inputOptions = (List<string>)properties.Select(prop => prop.Name).ToList();
        inputDropdown.options.Clear();
        for (int i = 0; i < inputOptions.Count; i++)
        {
            inputDropdown.options.Add(new TMP_Dropdown.OptionData() { text = inputOptions[i] });
        }
        inputDropdown.value = index;
    }
    private void OnDeviceChanged(TMP_Dropdown change)
    {
        switch (change.value)
        {
            case 0:
                interaction.data.deviceCharacteristics = InputDeviceCharacteristics.Controller;
                break;
            case 1:
                interaction.data.deviceCharacteristics = InputDeviceCharacteristics.Left;
                break;
            case 2:
                interaction.data.deviceCharacteristics = InputDeviceCharacteristics.Right;
                break;
            default:
                interaction.data.deviceCharacteristics = InputDeviceCharacteristics.Camera;
                break;
        }
        nameField.text = GetGenericName();
    }

    private void OnInputChanged(TMP_Dropdown change)
    {
        string name = inputOptions[change.value];
        interaction.SetInput(name, change.value);
        nameField.text = GetGenericName();
    }

    private string GetGenericName()
    {
        string name = inputOptions[inputDropdown.value] + AddVRInputUI.deviceShortNames[deviceDropdown.value];
        return name;
    }

    private void OnNameChanged()
    {
        interaction.data.interactionName = nameField.text;
        addInteractionParentUI.parent.saveSystem.CurrentData.ChangeVrName();
    }

    private int GetDeviceNameIndex()
    {
        if (interaction.data.deviceCharacteristics == InputDeviceCharacteristics.Controller)
        {
            return 0;
        }
        if (interaction.data.deviceCharacteristics == InputDeviceCharacteristics.Left)
        {
            return 1;
        }
        if (interaction.data.deviceCharacteristics == InputDeviceCharacteristics.Right)
        {
            return 2;
        }
        return 3;
    }

    private int GetDirectionIndex()
    {
        if (interaction.data.maskVector.x == 0 && interaction.data.maskVector.y == 0)
        {
            return 3;
        }
        if (interaction.data.maskVector.z == 0 && interaction.data.maskVector.y == 0)
        {
            return 1;
        }
        if (interaction.data.maskVector.z == 0 && interaction.data.maskVector.x == 0)
        {
            return 2;
        }
        return 0;
    }
}
