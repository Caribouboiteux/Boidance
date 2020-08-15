using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AddBoidBehaviorUI : AddInteractionUI<FlockBehavior>
{


    public override void PopulatePrefabUI()
    {
        for (int i = 0; i < targetPrefabObject.Length; i++)
        {
            targetPrefabs[i] = (FlockBehavior)targetPrefabObject[i];
        }

        FlockBehavior[] behaviors = parent.saveSystem.CurrentData.CompositeBehavior.behaviors;
        float[] weights = parent.saveSystem.CurrentData.CompositeBehavior.weights;
        for (int i = 0; i < behaviors.Length; i++)
        {
            int targetIndex = 0;
            for (int j = 0; j < targetPrefabs.Length; j++)
            {
                if (behaviors[i] == targetPrefabs[j])
                {
                    targetIndex = j;
                }
            }
            OnAddButtonClicked(targetIndex, weights[i]);
        }
    }

    public override void UpdateInteractionHandler(InteractionUI<FlockBehavior> interactionUI)
    {
        parent.saveSystem.CurrentData.AddBehavior();
    }
    public void OnAddButtonClicked(int targetBehaviorIndex, float weightTarget)
    {
        GameObject newBehavior = Instantiate<GameObject>(displayUIPrefab);
        newBehavior.transform.SetParent(contentDisplay.transform);
        newBehavior.transform.localScale = Vector3.one;
        newBehavior.SetActive(true);
        BoidBehaviorUI behavior = newBehavior.GetComponent<BoidBehaviorUI>();
        behavior.addInteractionParentUI = this;
        behavior.behaviorIndex = targetBehaviorIndex ;
        behavior.weightTarget = weightTarget;
        behavior.boidBehaviorDropdown.SetValueWithoutNotify(targetBehaviorIndex);
        prefabUIList.Add(behavior);
        UpdatePreferences();
    }

    public override void UpdatePreferences()
    {
    }
}
