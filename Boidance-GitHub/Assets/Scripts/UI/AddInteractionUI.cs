using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class AddInteractionUI<T> : MonoBehaviour
{
    public FlockUI parent;
    public GameObject displayUIPrefab;
    public GameObject contentDisplay;
    public List<InteractionUI<T>> prefabUIList;
    public UserInteractionManager userInteractionManager;
    public static T[] targetPrefabs;
    public string prefabFolderPath;

    protected Button addButton;

    protected static Object[] targetPrefabObject;
    private void Start()
    {
        prefabUIList = new List<InteractionUI<T>>();
        addButton = (this.GetComponent<Button>()) as Button;
        addButton.onClick.AddListener(OnAddButtonClicked);

        LoadTargetPrefabOptions();
        parent.saveSystem.OnPreferencesChange += OnPreferencesChange;
    }

    public void OnPreferencesChange(PreferencesData change)
    {
        foreach (InteractionUI<T> uI in prefabUIList)
        {
            Destroy(uI.gameObject);
        }
        prefabUIList = new List<InteractionUI<T>>();
        LoadTargetPrefabOptions();
    }
    public void OnAddButtonClicked()
    {
        GameObject newDisplayerUI = Instantiate<GameObject>(displayUIPrefab);
        newDisplayerUI.transform.SetParent(contentDisplay.transform);
        newDisplayerUI.transform.localScale = Vector3.one;
        newDisplayerUI.SetActive(true);
        InteractionUI<T> interactionUI = newDisplayerUI.GetComponent<InteractionUI<T>>();
        interactionUI.addInteractionParentUI = this;
        prefabUIList.Add(interactionUI);
        UpdateInteractionHandler(interactionUI);
    }
    public abstract void UpdateInteractionHandler(InteractionUI<T> interactionUI);
    public void LoadTargetPrefabOptions()
    {
        targetPrefabObject = Resources.LoadAll(prefabFolderPath, typeof(T));
        targetPrefabs = new T[targetPrefabObject.Length];
        PopulatePrefabUI();
    }
    public abstract void PopulatePrefabUI();
    public abstract void UpdatePreferences();

}

