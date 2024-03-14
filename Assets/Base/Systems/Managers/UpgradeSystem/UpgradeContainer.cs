using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeContainer", menuName = "ScriptableObjects/UpgradeContainer", order = 1)]
public class UpgradeContainer : ScriptableObject
{
    public List<UpgradeData> UpgradeDatas = new List<UpgradeData>();
}
[System.Serializable]
public class UpgradeData
{
    public UpgradeTypes UpgradeType;
    public int DefaultLevel = 1;
    public int MaxLevel = 10;
    public float Multiplier;
    public float Price;
    [Header("Force Price")]
    public bool ForcePrice;
    public List<int> ForcePriceList = new List<int>();
    
    public int GetPrice(int level)
    {
        if (ForcePrice)
        {
            return ForcePriceList[level];
        }
        return (int) (Price * Multiplier* level);
    }
}