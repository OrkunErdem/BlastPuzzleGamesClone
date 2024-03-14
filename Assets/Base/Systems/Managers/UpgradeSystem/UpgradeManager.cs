using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private bool isLoaded;
    private Dictionary<UpgradeTypes, int> upgradeDictionary = new Dictionary<UpgradeTypes, int>();
    public List<UpgradeData> UpgradeDatas = new List<UpgradeData>();

    public void DoUpgrade(UpgradeTypes type, int amount)
    {
        if (!isLoaded) Load();
        if (!upgradeDictionary.ContainsKey(type))
        {
            upgradeDictionary[type] = GetUpgradeTypeData(type).DefaultLevel;
        }

        upgradeDictionary[type] += amount;

        DataManager.Instance.SaveData("UpgradeContainer", upgradeDictionary);
        EventSystem.TriggerEvent(Events.OnUpgrade, type);
    }

    public bool CanUpdatable(UpgradeTypes type, int amount)
    {
        if (!isLoaded) Load();
        if (!upgradeDictionary.ContainsKey(type))
        {
            upgradeDictionary[type] = GetUpgradeTypeData(type).DefaultLevel;
        }

        if (GetUpgradeTypeData(type).MaxLevel < upgradeDictionary[type] + amount)
        {
            return false;
        }

        return true;
    }

    public int GetUpgradeFee(UpgradeTypes type)
    {
        if (!isLoaded) Load();
        if (!upgradeDictionary.ContainsKey(type))
        {
            upgradeDictionary[type] = GetUpgradeTypeData(type).DefaultLevel;
        }

        return GetUpgradeTypeData(type).GetPrice(upgradeDictionary[type]);
    }

    public void Load()
    {
        isLoaded = true;
        upgradeDictionary = DataManager.Instance.GetData<Dictionary<UpgradeTypes, int>>("UpgradeContainer");
        UpgradeDatas = Resources.Load<UpgradeContainer>("UpgradeContainer").UpgradeDatas;
        if (upgradeDictionary == null)
        {
            upgradeDictionary = new Dictionary<UpgradeTypes, int>();
        }
    }

    public UpgradeData GetUpgradeTypeData(UpgradeTypes upgradeType)
    {
        if (!isLoaded) Load();
        return UpgradeDatas.Find(x => x.UpgradeType == upgradeType);
    }

    public int GetUpgradeLevel(UpgradeTypes type)
    {
        if (!isLoaded) Load();
        if (!upgradeDictionary.ContainsKey(type)) return GetUpgradeTypeData(type).DefaultLevel;
        return upgradeDictionary[type];
    }

    public void ForceUpdateUpgradeLevel(UpgradeTypes type, int amount)
    {
        if (!isLoaded) Load();
        upgradeDictionary[type] = amount;
        DataManager.Instance.SaveData("UpgradeData", upgradeDictionary);
        EventSystem.TriggerEvent(Events.OnUpgrade, upgradeDictionary[type]);
    }
}