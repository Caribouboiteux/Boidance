using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SaveSystem : MonoBehaviour
{
    public List<PreferencesData> data;
    public PreferencesData currentData;
    public PreferencesData defaultData;
    public PreferencesData defaultData1;
    public PreferencesData defaultData2;
    public PreferencesData defaultData3;

    public PreferencesData CurrentData
    {
        get { return currentData; }
        set
        {
            if (currentData == value) return;
            currentData = value;
            OnPreferenceChange();
            OnPreferencesChange?.Invoke(currentData);
        }
    }

    public delegate void OnPreferencesChangeDelegate(PreferencesData newVal);
    public event OnPreferencesChangeDelegate OnPreferencesChange;

    [Header("UI", order = 1)]
    public TMP_InputField title;
    public TMP_Text nbText;
    public Button saveButton;
    public Button addButton;
    public Button removeButton;
    public Button upButton;
    public Button downButton;
    private void Start()
    {
        data = new List<PreferencesData>();
        data.Add(defaultData);
        data.Add(defaultData1);
        data.Add(defaultData2);
        data.Add(defaultData3);

        title.onValueChanged.AddListener(delegate { OnNameChanged(title); });
        upButton.onClick.AddListener(OnUp);
        downButton.onClick.AddListener(OnDown);
        saveButton.onClick.AddListener(delegate {SaveData();});
        data[0]= LoadData("0");
        data[1]= LoadData("1");
        data[2] = LoadData("2");
        data[3] = LoadData("3");
        CurrentData = data[0];
    }

    private void OnPreferenceChange()
    {
        CheckNullPreferences(currentData);
        nbText.SetText((data.IndexOf(currentData)+1).ToString()+"/4");
        title.text = currentData.dataName;
    }

    public void OnUp()
    {
        if (data.IndexOf(currentData) + 1 > 3)
        {
            CurrentData = data[0];
        }
        else
        {
            CurrentData = data[data.IndexOf(CurrentData) + 1];
        }
    }
    public void OnDown()
    {
        if (data.IndexOf(currentData) - 1 < 0)
        {
            CurrentData = data[data.Count - 1];
        }
        else
        {
            CurrentData = data[data.IndexOf(CurrentData) - 1];
        }
    }

    public void OnAddButtonPressed()
    {
        CurrentData = data[data.Count - 1];
        removeButton.interactable = false;
        title.text = currentData.dataName;
        title.interactable = true;
    }

    private void OnNameChanged(TMP_InputField change)
    {
        currentData.dataName = change.text;
    }
    void CheckNullPreferences(PreferencesData data)
    {
        if (data.CompositeBehavior == null) { data.CompositeBehavior = ScriptableObject.CreateInstance<CompositeBehavior>(); }
        if (data.compositeGene == null) { data.compositeGene = ScriptableObject.CreateInstance<CompositeGenetic>(); }
        data.VRInteractions = new System.Collections.Generic.List<BoidInteraction>();
        if (data.VRInteractionsData != null)
        {
            foreach (BoidInteraction.Data item in data.VRInteractionsData)
            {
                BoidInteraction interaction;
                switch (item.regexName)
                {
                    case "System.Boolean":
                        interaction = ScriptableObject.CreateInstance<BoolVRInteraction>();
                        break;
                    case "System.Single":
                        interaction = ScriptableObject.CreateInstance<FloatVRInteraction>();
                        break;
                    case "UnityEngine.Vector2":
                        interaction = ScriptableObject.CreateInstance<Vector2VRInteraction>();
                        break;
                    case "UnityEngine.Vector3":
                        interaction = ScriptableObject.CreateInstance<Vector3VRInteraction>();
                        break;
                    default:
                        interaction = ScriptableObject.CreateInstance<QuaternionVRInteraction>();
                        break;
                }
                interaction.data = item;
                data.VRInteractions.Add(interaction);
            }
        }
        if (data.VRInteractionsData == null) { data.VRInteractionsData = new System.Collections.Generic.List<BoidInteraction.Data>(); }
        if (data.GenesInteractions == null) { data.GenesInteractions = new System.Collections.Generic.List<GeneVRInteractionUI.Data>(); }
        if (data.behaviorInteractionsData == null) { data.behaviorInteractionsData = new System.Collections.Generic.List<BehaviorVRInteractionUI.Data>(); }
        data.UpdateScriptableObjectName();
    }

    void SaveData()
    {
        string temp = data.IndexOf(currentData).ToString();
        SaveData(temp);
    }

    void SaveData(string name)
    {
        Debug.Log("Preferences Saved as " + name);
        // save Preferences Data
        string json = JsonUtility.ToJson(currentData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + ".txt", json);
        // save Behaviors
        json = JsonUtility.ToJson(currentData.CompositeBehavior);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + "Behaviors.txt", json);
        // save Genes
        json = JsonUtility.ToJson(currentData.compositeGene);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + "Gene.txt", json);
    }

    private void LoadScriptableObjectByName(PreferencesData data)
    {
        Debug.Log("NewSession, load from name");
        FlockBehavior[] behaviors = new FlockBehavior[data.behaviorsName.Count];
        for (int i = 0; i < data.behaviorsName.Count; i++)
        {
            behaviors[i] = Resources.Load<FlockBehavior>("UIPrefab" + Path.DirectorySeparatorChar
                + "BoidsBehaviors" + Path.DirectorySeparatorChar + data.behaviorsName[i]);

        }
        data.CompositeBehavior.behaviors = behaviors;
        data.CompositeBehavior.weights = data.weigth.ToArray();

        GeneticBehavior[] genes = new GeneticBehavior[data.genesName.Count];
        for (int i = 0; i < data.genesName.Count; i++)
        {
            genes[i] = Resources.Load<GeneticBehavior>("UIPrefab" + Path.DirectorySeparatorChar
                + "Genetic" + Path.DirectorySeparatorChar + data.genesName[i]);
        }
        data.compositeGene.genes = genes;
    }
    PreferencesData LoadData(string name)
    {
        PreferencesData data;
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + ".txt"))
        {
            data = ScriptableObject.CreateInstance<PreferencesData>();
            string json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + ".txt");
            JsonUtility.FromJsonOverwrite(json, data);
            if (data.CompositeBehavior == null)
            {
                switch (name)
                {
                    case "3":
                        data.CompositeBehavior = defaultData3.CompositeBehavior;
                        data.compositeGene = defaultData3.compositeGene;
                        break;
                    case "2":
                        data.CompositeBehavior = defaultData2.CompositeBehavior;
                        data.compositeGene = defaultData2.compositeGene;
                        break;
                    case "1":
                        data.CompositeBehavior = defaultData1.CompositeBehavior;
                        data.compositeGene = defaultData1.compositeGene;
                        break;
                    default:
                        data.CompositeBehavior = defaultData.CompositeBehavior;
                        data.compositeGene = defaultData.compositeGene;
                        break;
                }
                LoadScriptableObjectByName(data);
            }
            else
            {
                json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + "Behaviors.txt");
                JsonUtility.FromJsonOverwrite(json, data.CompositeBehavior);
                json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "PreferencesData" + name + "Gene.txt");
                JsonUtility.FromJsonOverwrite(json, data.compositeGene);
            }
        }
        else
        {
            switch (name)
            {
                case "3":
                    data = defaultData3;
                    break;
                case "2":
                    data = defaultData2;
                    break;
                case "1":
                    data = defaultData1;
                    break;
                default:
                    data = defaultData;
                    break;
            }
        }
        return data;
    }
}
