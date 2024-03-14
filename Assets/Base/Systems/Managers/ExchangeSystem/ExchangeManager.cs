using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExchangeType
{
    Coin,
}

public class ExchangeManager : Singleton<ExchangeManager>
{
    private bool isLoaded;
    private Dictionary<ExchangeType, float> exchangeDictionary = new Dictionary<ExchangeType, float>();

    public void DoExchange(ExchangeType type, float amount)
    {
        if (!isLoaded) Load();
        if (!exchangeDictionary.ContainsKey(type))
        {
            exchangeDictionary[type] = 0;
        }

        exchangeDictionary[type] += amount;
        DataManager.Instance.SaveData("ExchangeData", exchangeDictionary);
        EventSystem.TriggerEvent(Events.OnExchange, type);
    }

    public void Load()
    {
        isLoaded = true;
        exchangeDictionary = DataManager.Instance.GetData<Dictionary<ExchangeType, float>>("ExchangeData");
        if (exchangeDictionary == null)
        {
            exchangeDictionary = new Dictionary<ExchangeType, float>();
        }
    }

    public float GetExchange(ExchangeType type)
    {
        if (!isLoaded) Load();
        if (!exchangeDictionary.ContainsKey(type)) return 0;
        return exchangeDictionary[type];
    }

    public void StateExchange(ExchangeType type, float amount)
    {
        if (!isLoaded) Load();
        exchangeDictionary[type] = amount;
        DataManager.Instance.SaveData("ExchangeData", exchangeDictionary);
        EventSystem.TriggerEvent(Events.OnExchange, exchangeDictionary[type]);
    }
}