using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class UIPanel
{
    public string ID;
    public GameObject Panel;
}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<string, GameObject> UICreatedDictionary = new Dictionary<string, GameObject>();
    private Canvas canvas;
    public Canvas Canvas => canvas? canvas : canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

    public void ShowPanel(string panelID, params object[] variables)
    {
        if (!UICreatedDictionary.ContainsKey(panelID) || UICreatedDictionary[panelID] == null)
        {
            GameObject panel = Instantiate(PanelLoad(panelID).gameObject,Canvas.transform);
            UICreatedDictionary[panelID] = panel;
        }

        UICreatedDictionary[panelID].transform.SetParent(Canvas.transform);
        UICreatedDictionary[panelID].GetComponent<PanelBase>().ShowPanel(variables);
        EventSystem.TriggerEvent(Events.OnShowPanel,panelID);
    }

    public void HidePanel(string panelID)
    {
        UICreatedDictionary[panelID].GetComponent<PanelBase>().HidePanel();
        UICreatedDictionary[panelID].transform.SetParent(transform);
        EventSystem.TriggerEvent(Events.OnHidePanel, panelID);
    }

    private PanelBase PanelLoad(string panelID)
    {
        return Resources.Load<PanelBase>("UISystemData/Panels/"+panelID);
    }
}
