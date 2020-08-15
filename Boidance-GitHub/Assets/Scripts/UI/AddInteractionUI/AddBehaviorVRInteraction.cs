using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddBehaviorVRInteraction : AddInteractionUI<BoidInteraction>
{

    public override void PopulatePrefabUI()
    {
        for (int i = 0; i < targetPrefabObject.Length; i++)
        {
            targetPrefabs[i] = (BoidInteraction)targetPrefabObject[i];
        }

        for (int i = 0; i < parent.saveSystem.CurrentData.BehaviorInteractionsData.Count; i++)
        {
            OnAddButtonClicked(parent.saveSystem.CurrentData.BehaviorInteractionsData[i]);
        }
    }
    public void OnAddButtonClicked(BehaviorVRInteractionUI.Data data)
    {
        GameObject newInput = Instantiate<GameObject>(displayUIPrefab);
        newInput.transform.SetParent(contentDisplay.transform);
        newInput.transform.localScale = Vector3.one;
        newInput.SetActive(true);
        BehaviorVRInteractionUI interaction = newInput.GetComponent<BehaviorVRInteractionUI>();
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
