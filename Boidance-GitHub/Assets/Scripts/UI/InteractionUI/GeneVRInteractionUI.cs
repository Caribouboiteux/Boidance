using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TMPro;
using UnityEngine;

[System.Serializable]

public class GeneVRInteractionUI : InteractionUI<BoidInteraction>
{
    [System.Serializable]
    public class  Data
    {
        public bool init = false;
        public int vrIndex;
        public int geneIndex;
        public int variableIndex;
        public float min;
        public float max;
    }

    public TMP_Dropdown vrInputDropdown;
    public TMP_Dropdown geneDropdown;
    public TMP_Dropdown variableDropdown;
    public TMP_InputField min;
    public TMP_InputField max;
    public string[] names;
    public Data data;
    public FieldInfo propInfo;

    private int saveVRTarget;



    // Start is called before the first frame update
    void Start()
    {
        deleteButton.onClick.AddListener(DeleteInteraction);
        saveVRTarget = data.vrIndex;
        // Possible VR Input
        OnVRInputChanged(addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions);
        vrInputDropdown.onValueChanged.AddListener(delegate { OnVRInteractionChanged(vrInputDropdown); });
        addInteractionParentUI.parent.saveSystem.CurrentData.OnVRInteractionsChange += OnVRInputChanged;

        // Possible Gene 
        OnGenesChanged(addInteractionParentUI.parent.saveSystem.CurrentData.Genes);
        geneDropdown.onValueChanged.AddListener(delegate { OnGeneInputChange(geneDropdown); });
        addInteractionParentUI.parent.saveSystem.CurrentData.OnGenesChange += OnGenesChanged;

        // Possible Variable
        names = new string[typeof(GeneticBehavior).GetFields().Length];
        variableDropdown.options.Clear();
        for (int i = 0; i < typeof(GeneticBehavior).GetFields().Length; i++)
        {
            variableDropdown.options.Add(new TMP_Dropdown.OptionData() { text = typeof(GeneticBehavior).GetFields()[i].Name });
            names[i] = typeof(GeneticBehavior).GetFields()[i].Name;
        }
        variableDropdown.onValueChanged.AddListener(delegate { OnVariableChanged(variableDropdown); });

        // load or create data
        if (data.init == false) // if new interaction
        {
            data.vrIndex = 0;
            data.geneIndex = 0;
            data.variableIndex = 0;
            data.min = 0f;
            data.max = 1f;
            addInteractionParentUI.parent.saveSystem.CurrentData.AddGeneInteraction(data);
            data.init = true;
        }
        vrInputDropdown.SetValueWithoutNotify(data.vrIndex);
        OnVRInteractionChanged(vrInputDropdown);

        propInfo = typeof(GeneticBehavior).GetField(names[data.variableIndex]);
        variableDropdown.SetValueWithoutNotify(data.variableIndex);
        geneDropdown.SetValueWithoutNotify(data.geneIndex);

        min.text = (data.min).ToString();
        max.text = (data.max).ToString();
        min.onValueChanged.AddListener(delegate { OnMinMaxChange(min); });
        max.onValueChanged.AddListener(delegate { OnMinMaxChange(max); });
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

    private void OnGenesChanged(CompositeGenetic genes)
    {
        geneDropdown.options.Clear();
        for (int i = 0; i < genes.genes.Length; i++)
        {
            geneDropdown.options.Add(new TMP_Dropdown.OptionData() { text = genes.genes[i].name });
        }
    }

    public void OnVariableChanged(TMP_Dropdown change)
    {
        propInfo = typeof(GeneticBehavior).GetField(names[change.value]);
        VariableChangeHandler(Vector4.zero);
        data.variableIndex = change.value;
        UpdatePreferenceData();
    }

    public void UpdatePreferenceData()
    {
        if (addInteractionParentUI.parent.saveSystem.CurrentData.genesInteractions.Count > addInteractionParentUI.prefabUIList.IndexOf(this) && addInteractionParentUI.prefabUIList.IndexOf(this) !=-1) {
            addInteractionParentUI.parent.saveSystem.CurrentData.genesInteractions
                [addInteractionParentUI.prefabUIList.IndexOf(this)] = data;
        }
    }

    public override void DeleteInteraction()
    {
        if (addInteractionParentUI != null)
        {
            addInteractionParentUI.parent.saveSystem.CurrentData.RemoveGeneInteraction(data);
            addInteractionParentUI.prefabUIList.Remove(this);
        }
        Destroy(this.gameObject);
    }

    private void OnVRInteractionChanged(TMP_Dropdown change)
    {
        addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions[data.vrIndex].OnVariableChange -= VariableChangeHandler;
        addInteractionParentUI.parent.saveSystem.CurrentData.VRInteractions[change.value].OnVariableChange += VariableChangeHandler;
        data.vrIndex = change.value;
        UpdatePreferenceData();
        saveVRTarget = data.vrIndex;
        change.RefreshShownValue();
    }

    private void OnGeneInputChange(TMP_Dropdown change)
    {
        data.geneIndex = change.value;
        UpdatePreferenceData();
    }

    private void OnMinMaxChange(TMP_InputField change)
    {
        data.min = float.Parse(min.text, CultureInfo.InvariantCulture);
        data.max = float.Parse(max.text, CultureInfo.InvariantCulture);
        UpdatePreferenceData();
    }

    private void VariableChangeHandler(Vector4 newVal)
    {
        if (propInfo != null)
        {
            float value = Mathf.Lerp(float.Parse(min.text, CultureInfo.InvariantCulture), float.Parse(max.text, CultureInfo.InvariantCulture), newVal.sqrMagnitude);
            propInfo.SetValue(addInteractionParentUI.parent.saveSystem.CurrentData.Genes.genes[geneDropdown.value], value);
            Debug.Log(newVal);
        }
    }
}
