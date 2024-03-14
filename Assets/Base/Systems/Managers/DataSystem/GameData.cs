using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : IData
{
    public Dictionary<string, object> DataCollection = new Dictionary<string, object>();
    public T GetData<T>(string key)
    {
        T _data;
        if (DataCollection.ContainsKey(key))
        {
            _data = Json.ConvertFromJson<T>(DataCollection[key].ToString());
        }
        else
        {
            _data = default(T);
        }
        return _data;
    }

    public Type GetDataType()
    {
        return typeof(GameData);
    }

    public void UpdateData<T>(string key, T value)
    {
        if (DataCollection.ContainsKey(key))
        {
            DataCollection[key] = Json.ConvertToJson(value);
        }
        else
        {
            DataCollection.Add(key, value);
        }
    }
}