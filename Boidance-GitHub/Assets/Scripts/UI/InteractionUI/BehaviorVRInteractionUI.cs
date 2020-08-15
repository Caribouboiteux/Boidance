using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BehaviorVRInteractionUI : InteractionUI<BoidInteraction>
{

    [System.Serializable]
    public class Data
    {

        public bool init = false;
        public int vrIndex;
        public int flockTargetIndex;
        public float min;
        public float max;

        public static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }


    public TMP_Dropdown vrInputDropdown;
    public TMP_Dropdown flockTargetDropdown;
    public TMP_InputField min;
    public TMP_InputField max;
    public Data data;
    private int saveVRTarget;


    private void Start()
    {
        deleteButton.onClick.AddListener(DeleteInteraction);
        saveVRTarget = data.vrIndex;

        // load or create data
        if (data.init == false) // if new interaction
        {
            data.vrIndex = 0;
            data.flockTargetIndex = 0;
            data.min = 0f;
            data.max = 1f;
            addInteractionParentUI.parent.saveSystem.CurrentData.AddBehaviorDataInteraction(data);
            data.init = true;
        }

        // Possible VR Input
        OnVRInputChanged(addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions);
        vrInputDropdown.onValueChanged.AddListener(delegate { OnVRInteractionChanged(vrInputDropdown); });
        addInteractionParentUI.parent.saveSystem.CurrentData.OnVRInteractionsChange += OnVRInputChanged;
        vrInputDropdown.SetValueWithoutNotify(data.vrIndex);
        OnVRInteractionChanged(vrInputDropdown);

        // Possible flockTarget
        OnBehaviorChange(addInteractionParentUI.parent.saveSystem.CurrentData.CompositeBehavior.behaviors);
        flockTargetDropdown.onValueChanged.AddListener(delegate { OnFlockTargetChanged(flockTargetDropdown); });
        addInteractionParentUI.parent.saveSystem.CurrentData.OnCompositeBehaviorsChange += OnBehaviorsChanged;
        flockTargetDropdown.SetValueWithoutNotify(data.flockTargetIndex);


        min.text = (data.min).ToString();
        max.text = (data.max).ToString();
        min.onValueChanged.AddListener(delegate { OnMinMaxChanged(min, max); });
        max.onValueChanged.AddListener(delegate { OnMinMaxChanged(min, max); });
    }

    // display updater
    private void OnVRInputChanged(List<BoidInteraction> newVal)
    {
        vrInputDropdown.options.Clear();
        for (int i = 0; i < newVal.Count; i++)
        {
            if (newVal[i] != null)
            {
                vrInputDropdown.options.Add(new TMP_Dropdown.OptionData() { text = newVal[i].data.interactionName });
            }
        }
        if (saveVRTarget >= newVal.Count)
        {
            data.vrIndex = 0;
            vrInputDropdown.SetValueWithoutNotify(data.vrIndex);
            if (addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions.Count > 0)
            {
                addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions[data.vrIndex].OnVariableChange += VariableChangeHandler;
            }
        }
        else
        {
            vrInputDropdown.SetValueWithoutNotify(saveVRTarget);
            OnVRInteractionChanged(vrInputDropdown);
        }
        vrInputDropdown.RefreshShownValue();
    }

    private void OnBehaviorChange(FlockBehavior[] newVal)
    {
        flockTargetDropdown.options.Clear();
        for (int i = 0; i < newVal.Length; i++)
        {
            if (newVal[i] != null)
            {
                flockTargetDropdown.options.Add(new TMP_Dropdown.OptionData() { text = newVal[i].name });
            }
        }
        flockTargetDropdown.options.Add(new TMP_Dropdown.OptionData() {text = "None"});
    }
    // ---------
    private void OnBehaviorsChanged(CompositeBehavior newVal)
    {
        flockTargetDropdown.options.Clear();
        for (int i = 0; i < newVal.behaviors.Length; i++)
        {
            if (newVal.behaviors[i] != null)
            {
                flockTargetDropdown.options.Add(new TMP_Dropdown.OptionData() { text = newVal.behaviors[i].name });
            }
        }

        flockTargetDropdown.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
        if (flockTargetDropdown.options.Count - 1 < flockTargetDropdown.value)
        {
            flockTargetDropdown.SetValueWithoutNotify(flockTargetDropdown.options.Count - 1);
        }
        flockTargetDropdown.RefreshShownValue();

    }
    public override void DeleteInteraction()
    {
        if (addInteractionParentUI != null)
        {
            addInteractionParentUI.parent.saveSystem.CurrentData.RemoveBehaviorDataInteraction(data);
            addInteractionParentUI.prefabUIList.Remove(this);
        }
        Destroy(this.gameObject);
    }

    private void OnVRInteractionChanged(TMP_Dropdown change)
    {

        addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions[data.vrIndex].OnVariableChange -= VariableChangeHandler;
        addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions[change.value].OnVariableChange += VariableChangeHandler;
        data.vrIndex = change.value;
        saveVRTarget = data.vrIndex;
        change.RefreshShownValue();
    }

    private void VariableChangeHandler(Vector4 newVal)
    {
        if (flockTargetDropdown.value < flockTargetDropdown.options.Count - 1)
        {
            Debug.Log(newVal);
            float value = Mathf.Lerp(float.Parse(min.text, CultureInfo.InvariantCulture), float.Parse(max.text, CultureInfo.InvariantCulture), newVal.sqrMagnitude);
            addInteractionParentUI.parent.saveSystem.CurrentData.CompositeBehavior.weights[flockTargetDropdown.value] = value;
        }
    }
    private void OnFlockTargetChanged(TMP_Dropdown change)
    {
        data.flockTargetIndex = change.value;
    }

    private void OnMinMaxChanged(TMP_InputField min, TMP_InputField max)
    {
        data.min = float.Parse(min.text, CultureInfo.InvariantCulture);
        data.max = float.Parse(max.text, CultureInfo.InvariantCulture);
    }
}