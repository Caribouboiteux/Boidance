using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractionUI<T> : MonoBehaviour 
{
    public AddInteractionUI<T> addInteractionParentUI;
    public Button deleteButton;

    public abstract void DeleteInteraction();
}
