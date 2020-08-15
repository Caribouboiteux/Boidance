using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GeneInteractionUI : InteractionUI<GeneticBehavior>
{
    public TMP_Dropdown geneDropdown;
    public int geneIndex;
    public TMP_InputField mutationRate;
    public TMP_InputField insertion;
    public TMP_InputField elitsm;
    public TMP_InputField mutationRange;
    public TMP_InputField lerpTime;

    public GeneticBehavior gene;

    // Start is called before the first frame update
    void Start()
    {
        deleteButton.onClick.AddListener(DeleteInteraction);
        geneDropdown.onValueChanged.AddListener(delegate { OnGeneChanged(geneDropdown); });
        geneDropdown.options.Clear();
        for (int i = 0; i < AddGeneInteractionUI.targetPrefabs.Length; i++)
        {
            geneDropdown.options.Add(new TMP_Dropdown.OptionData() { text = AddGeneInteractionUI.targetPrefabs[i].name });
        }
        geneDropdown.value = geneIndex;

        OnGeneChanged(geneDropdown);

        UpdateInfo();

        mutationRate.onValueChanged.AddListener(delegate { OnParamChanged(mutationRate); });
        insertion.onValueChanged.AddListener(delegate { OnParamChanged(mutationRate); });
        elitsm.onValueChanged.AddListener(delegate { OnParamChanged(mutationRate); });
        mutationRange.onValueChanged.AddListener(delegate { OnParamChanged(mutationRate); });
        lerpTime.onValueChanged.AddListener(delegate { OnParamChanged(mutationRate); });

    }
    private void UpdateInfo()
    {
        mutationRate.SetTextWithoutNotify(gene.mutationRate.ToString(CultureInfo.InvariantCulture));
        insertion.SetTextWithoutNotify(gene.insertionRate.ToString(CultureInfo.InvariantCulture));
        elitsm.SetTextWithoutNotify(gene.elitismRate.ToString(CultureInfo.InvariantCulture));
        mutationRange.SetTextWithoutNotify(gene.mutationRange.ToString(CultureInfo.InvariantCulture));
        lerpTime.SetTextWithoutNotify(gene.lerpTime.ToString(CultureInfo.InvariantCulture));
    }
    public override void DeleteInteraction()
    {
        if (addInteractionParentUI != null)
        {
            addInteractionParentUI.parent.saveSystem.CurrentData.RemoveGene(addInteractionParentUI.prefabUIList.IndexOf(this));
            addInteractionParentUI.prefabUIList.Remove(this);
        }
        Destroy(this.gameObject);
    }

    private void OnGeneChanged(TMP_Dropdown change)
    {
        gene = AddGeneInteractionUI.targetPrefabs[change.value];
        UpdateInfo();
        addInteractionParentUI.parent.saveSystem.CurrentData.Genes.genes[addInteractionParentUI.prefabUIList.IndexOf(this)] = gene;
        addInteractionParentUI.parent.saveSystem.CurrentData.UpdateGene();
    }

    private void OnParamChanged(TMP_InputField change)
    {
        try
        {
            gene.insertionRate = float.Parse(insertion.text, CultureInfo.InvariantCulture);
            gene.mutationRate = float.Parse(mutationRate.text, CultureInfo.InvariantCulture);
            gene.mutationRange = float.Parse(mutationRange.text, CultureInfo.InvariantCulture);
            gene.elitismRate = float.Parse(elitsm.text, CultureInfo.InvariantCulture);
            gene.lerpTime = float.Parse(lerpTime.text, CultureInfo.InvariantCulture);
            addInteractionParentUI.parent.saveSystem.CurrentData.Genes.genes[addInteractionParentUI.prefabUIList.IndexOf(this)] = gene;
        }
        catch (FormatException e)
        {
            Debug.Log(e.Data);
        }

    }
}
