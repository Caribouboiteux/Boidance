using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandPanel : MonoBehaviour
{
    public RectTransform expendablePanel;
    public Transform content;
    public Transform panelName;
    public Button button; 

    public void Start()
    {
        button.onClick.AddListener(OnExpendPanel);
    }

    public void OnExpendPanel()
    {
        if (content.gameObject.activeSelf)
        {
            content.gameObject.SetActive(false);
            expendablePanel.sizeDelta = new Vector2(25, 300);
            panelName.gameObject.SetActive(true);
        }
        else
        {
            content.gameObject.SetActive(true);
            expendablePanel.sizeDelta = new Vector2(345, 300);
            panelName.gameObject.SetActive(false);
        }
    }
}
