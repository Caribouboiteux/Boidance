using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGeneInteractionUI : AddInteractionUI<GeneticBehavior>
{
    public override void PopulatePrefabUI()
    {
        for (int i = 0; i < targetPrefabObject.Length; i++)
        {
            targetPrefabs[i] = (GeneticBehavior)targetPrefabObject[i];
        }

        GeneticBehavior[] genes = parent.saveSystem.CurrentData.Genes.genes;
        for (int i = 0; i < genes.Length; i++)
        {
            int targetIndex = 0;
            for (int j = 0; j < targetPrefabs.Length; j++)
            {
                if (genes[i] == targetPrefabs[j])
                {
                    targetIndex = j;
                }
            }
            OnAddButtonClicked(targetIndex, genes[i]);
        }
    }

    public override void UpdateInteractionHandler(InteractionUI<GeneticBehavior> interactionUI)
    {
        parent.saveSystem.CurrentData.AddGene(targetPrefabs[0]);
    }

    public void OnAddButtonClicked(int targetBehaviorIndex, GeneticBehavior gene)
    {
        GameObject newBehavior = Instantiate<GameObject>(displayUIPrefab);
        newBehavior.transform.SetParent(contentDisplay.transform);
        newBehavior.transform.localScale = Vector3.one;
        newBehavior.SetActive(true);
        GeneInteractionUI geneUI = newBehavior.GetComponent<GeneInteractionUI>();
        geneUI.addInteractionParentUI = this;
        geneUI.gene = gene;
        prefabUIList.Add(geneUI);
    }


    public override void UpdatePreferences()
    {

    }
}
