using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddVRGeneInteractionUI : AddInteractionUI<BoidInteraction>
{
    public override void PopulatePrefabUI()
    {
        for (int i = 0; i < targetPrefabObject.Length; i++)
        {
            targetPrefabs[i] = (BoidInteraction)targetPrefabObject[i];
        }

        for (int i = 0; i < parent.saveSystem.CurrentData.genesInteractions.Count; i++)
        {
            OnAddButtonClicked(parent.saveSystem.CurrentData.genesInteractions[i]);
        }
    }

    public void OnAddButtonClicked(GeneVRInteractionUI.Data data)
    {
        GameObject newInput = Instantiate<GameObject>(displayUIPrefab);
        newInput.transform.SetParent(contentDisplay.transform);
        newInput.transform.localScale = Vector3.one;
        newInput.SetActive(true);
        GeneVRInteractionUI interaction = newInput.GetComponent<GeneVRInteractionUI>();
        interaction.addInteractionParentUI = this;
        interaction.data = data;
        prefabUIList.Add(interaction);
    }
    public override void UpdateInteractionHandler(InteractionUI<BoidInteraction> interactionUI)
    {

    }


    public override void UpdatePreferences()
    {

    }
}
