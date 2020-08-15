using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInteractionManager : MonoBehaviour
{
    public SaveSystem saveSystem;

    // Update is called once per frame
    void Update()
    {
        // Composite interactions
        for (int i=0; i< saveSystem.CurrentData.BehaviorInteractionsData.Count; i++)
        {
            saveSystem.CurrentData.VRInteractions[saveSystem.CurrentData.BehaviorInteractionsData[i].vrIndex].GetInputValue();
        }

        for (int i = 0; i < saveSystem.CurrentData.GenesInteractions.Count; i++)
        {
            saveSystem.CurrentData.VRInteractions[saveSystem.CurrentData.GenesInteractions[i].vrIndex].GetInputValue();
        }
    }
}
