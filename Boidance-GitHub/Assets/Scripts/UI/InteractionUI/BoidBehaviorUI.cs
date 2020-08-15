using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoidBehaviorUI : InteractionUI<FlockBehavior>
{
    public TMP_Dropdown boidBehaviorDropdown;
    public int behaviorIndex;
    public TMP_InputField weightField;
    public float weightTarget = 1;
    private void Start()
    {
        deleteButton.onClick.AddListener(DeleteInteraction);
        boidBehaviorDropdown.onValueChanged.AddListener(delegate { OnBoidsBehaviorChange(boidBehaviorDropdown); });
        boidBehaviorDropdown.options.Clear();
        for (int i = 0; i < AddBoidBehaviorUI.targetPrefabs.Length; i++)
        {
            boidBehaviorDropdown.options.Add(new TMP_Dropdown.OptionData() { text = AddBoidBehaviorUI.targetPrefabs[i].name });
        }
        boidBehaviorDropdown.value = behaviorIndex;
        weightField.text = weightTarget.ToString();

        weightField.onValueChanged.AddListener(delegate { OnWeightChanged(weightField); });
        OnBoidsBehaviorChange(boidBehaviorDropdown);
    }

    public override void DeleteInteraction()
    {
        if (addInteractionParentUI != null)
        {
            addInteractionParentUI.parent.saveSystem.CurrentData.RemoveBehavior(addInteractionParentUI.prefabUIList.IndexOf(this));
            addInteractionParentUI.prefabUIList.Remove(this);
        }
        Destroy(this.gameObject);
    }

    private void OnBoidsBehaviorChange(TMP_Dropdown change)
    {
        addInteractionParentUI.parent.saveSystem.CurrentData.CompositeBehavior.behaviors[addInteractionParentUI.prefabUIList.IndexOf(this)] =
            AddBoidBehaviorUI.targetPrefabs[change.value];
        addInteractionParentUI.parent.saveSystem.CurrentData.OnBehaviorChange();
    }

    private void OnWeightChanged(TMP_InputField change)
    {
        addInteractionParentUI.parent.saveSystem.CurrentData.CompositeBehavior.weights[addInteractionParentUI.prefabUIList.IndexOf(this)] =
            float.Parse(change.text, System.Globalization.CultureInfo.InvariantCulture);
    }
}
