using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Preference Data", menuName = "Create Preference Data", order = 51)]
public class PreferencesData : ScriptableObject
{

    public string dataName;

    // -------------------------------------

    public CompositeBehavior compositeBehavior;
    public List<string> behaviorsName;
    public List<float> weigth;

    public void UpdateScriptableObjectName()
    {
        behaviorsName = new List<string>();
        weigth = new List<float>();
        for (int i = 0; i < compositeBehavior.behaviors.Length; i++)
        {
            if (compositeBehavior.behaviors[i] != null)
            {
                behaviorsName.Add(compositeBehavior.behaviors[i].name);
                weigth.Add(compositeBehavior.weights[i]);
            }
        }
        genesName = new List<string>();
        foreach (var gene in compositeGene.genes)
        {
            genesName.Add(gene.name);
        }
    }
    public void AddBehavior()
    {
        compositeBehavior.AddBehavior();
        UpdateScriptableObjectName();
        OnCompositeBehaviorsChange?.Invoke(compositeBehavior);
    }

    public void RemoveBehavior(int index)
    {
        compositeBehavior.RemoveBehavior(index);
        UpdateScriptableObjectName();
        OnCompositeBehaviorsChange?.Invoke(compositeBehavior);
    }

    public void OnBehaviorChange()
    {
        OnCompositeBehaviorsChange?.Invoke(compositeBehavior);
    }
    public CompositeBehavior CompositeBehavior
    {
        get { return compositeBehavior; }
        set
        {
            if (compositeBehavior == value) return;
            compositeBehavior = value;
            OnCompositeBehaviorsChange?.Invoke(compositeBehavior);
        }
    }
    public delegate void OnCompositeBehaviorsChangeDelegate(CompositeBehavior newVal);
    public event OnCompositeBehaviorsChangeDelegate OnCompositeBehaviorsChange;

    // -------------------------------------

    public List<BehaviorVRInteractionUI.Data> behaviorInteractionsData;
    public void AddBehaviorDataInteraction(BehaviorVRInteractionUI.Data interaction)
    {
        behaviorInteractionsData.Add(interaction);
        OnBehaviorInteractionsDataChange?.Invoke(behaviorInteractionsData);
    }
    public void RemoveBehaviorDataInteraction(BehaviorVRInteractionUI.Data interaction)
    {
        behaviorInteractionsData.Remove(interaction);
        OnBehaviorInteractionsDataChange?.Invoke(behaviorInteractionsData);
    }
    public List<BehaviorVRInteractionUI.Data> BehaviorInteractionsData
    {
        get { return behaviorInteractionsData; }
        set
        {
            if (behaviorInteractionsData == value) return;
            behaviorInteractionsData = value;
            OnBehaviorInteractionsDataChange?.Invoke(behaviorInteractionsData);
        }
    }
    public delegate void OnBehaviorInteractionsDataChangeDelegate(List<BehaviorVRInteractionUI.Data> newVal);
    public event OnBehaviorInteractionsDataChangeDelegate OnBehaviorInteractionsDataChange;

    // -------------------------------------
    public CompositeGenetic compositeGene;
    public List<string> genesName;
    public void RemoveGene(int index)
    {
        compositeGene.RemoveGene(index);
        UpdateScriptableObjectName();
        OnGenesChange?.Invoke(compositeGene);
    }

    public void AddGene(GeneticBehavior gene)
    {
        compositeGene.AddGene(gene);
        UpdateScriptableObjectName();
        OnGenesChange?.Invoke(compositeGene);
    }

    public void UpdateGene()
    {
        OnGenesChange?.Invoke(compositeGene);
    }
    public CompositeGenetic Genes
    {
        get { return compositeGene; }
        set
        {
            //if (genes == value) return;
            compositeGene = value;
            OnGenesChange?.Invoke(compositeGene);
        }
    }
    public delegate void OnGenesChangeDelegate(CompositeGenetic newVal);
    public event OnGenesChangeDelegate OnGenesChange;

    // -------------------------------------
    public List<GeneVRInteractionUI.Data> genesInteractions;

    public void AddGeneInteraction(GeneVRInteractionUI.Data interaction)
    {
        genesInteractions.Add(interaction);
        OnGenesInteractionsChange?.Invoke(genesInteractions);
    }

    public void RemoveGeneInteraction(GeneVRInteractionUI.Data interaction)
    {
        genesInteractions.Remove(interaction);
        OnGenesInteractionsChange?.Invoke(genesInteractions);
    }
    public List<GeneVRInteractionUI.Data> GenesInteractions
    {
        get { return genesInteractions; }
        set
        {
            if (genesInteractions == value) return;
            genesInteractions = value;
            OnGenesInteractionsChange?.Invoke(genesInteractions);
        }
    }
    public delegate void OnGenesInteractionsChangeDelegate(List<GeneVRInteractionUI.Data> newVal);
    public event OnGenesInteractionsChangeDelegate OnGenesInteractionsChange;

    // -------------------------------------
    public List<BoidInteraction.Data> vRInteractionsData;

    public void addVrInteractionData(BoidInteraction.Data interaction)
    {
        vRInteractionsData.Add(interaction);
        OnVRInteractionsChangeData?.Invoke(vRInteractionsData);
    }
    public void RemoveVrInteractionData(BoidInteraction.Data i)
    {
        vRInteractionsData.Remove(i);
        OnVRInteractionsChangeData?.Invoke(vRInteractionsData);
    }
    public List<BoidInteraction.Data> VRInteractionsData
    {
        get { return vRInteractionsData; }
        set
        {
            if (vRInteractionsData == value) return;
            vRInteractionsData = value;
            OnVRInteractionsChangeData?.Invoke(vRInteractionsData);
        }
    }
    public delegate void OnVRInteractionsChangeDelegateData(List<BoidInteraction.Data> newVal);
    public event OnVRInteractionsChangeDelegateData OnVRInteractionsChangeData;

    // -------------------------------------
    public List<BoidInteraction> vRInteractions;

    public void addVrInteraction(BoidInteraction interaction, bool newData)
    {
        if (newData)
        {
            if (interaction != null)
            {
                addVrInteractionData(interaction.data);
            }
            else
            {
                addVrInteractionData(null);
            }
        }
        vRInteractions.Add(interaction);
        if (interaction != null)
        {
            OnVRInteractionsChange?.Invoke(vRInteractions);
        }
    }
    public void RemoveVrInteraction(BoidInteraction i)
    {
        if (i != null)
        {
            RemoveVrInteractionData(i.data);
            vRInteractions.Remove(i);
            OnVRInteractionsChange?.Invoke(vRInteractions);
        }
        else
        {
            RemoveVrInteractionData(null);
            vRInteractions.Remove(i);
            OnVRInteractionsChange?.Invoke(vRInteractions);
        }
    }

    public void ChangeVrName()
    {
        OnVRInteractionsChange?.Invoke(vRInteractions);
    }
    public List<BoidInteraction> VRInteractions
    {
        get { return vRInteractions; }
        set
        {
            if (vRInteractions == value) return;
            vRInteractions = value;
            OnVRInteractionsChange?.Invoke(vRInteractions);
        }
    }
    public delegate void OnVRInteractionsChangeDelegate(List<BoidInteraction> newVal);
    public event OnVRInteractionsChangeDelegate OnVRInteractionsChange;

    // -------------------------------------
    // Flock parameters
    public float scale = 0.045f;
    public float maxSpeed = 1.5f;
    public float sightRadius = 0.03f;
    public float sqrtAvoidanceRadius = 0.03f;
    public bool followRight;
    public bool followLeft;
    public bool followHead;
    public Color meanColor;
    public bool matchWheel;
    public bool matchShape;
    public bool matchSize;
    public bool triggerRight;
    public bool triggerLeft;
    public float Scale { get => scale; set { scale = value; OnFlockParametersChange?.Invoke(); } }
    public float MaxSpeed { get => maxSpeed; set { maxSpeed = value; OnFlockParametersChange?.Invoke(); } }
    public float SightRadius { get => sightRadius; set { sightRadius = value; OnFlockParametersChange?.Invoke(); } }
    public Color MeanColor { get => meanColor; set { meanColor = value; OnFlockParametersChange?.Invoke(); } }


    public delegate void OnFlockParametersChangeDelegate();
    public event OnFlockParametersChangeDelegate OnFlockParametersChange;
}
