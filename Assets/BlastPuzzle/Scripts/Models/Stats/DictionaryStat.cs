using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DictionaryStat<T1, T2> : StringStat
{
    public Dictionary<T1,T2> CurrentDictionary { get; protected set; }

    public DictionaryStat(Dictionary<T1,T2> initial) : base(JsonConvert.SerializeObject(initial))
    {
        var json = JsonConvert.SerializeObject(initial);
        CurrentDictionary = initial;
        CurrentValue = json;
    }

    public DictionaryStat()
    {
    }

    public override void Reconstruct(string value)
    {
        base.Reconstruct(value);

        CurrentValue = value;

        CurrentDictionary = string.IsNullOrEmpty(CurrentValue)
            ? new Dictionary<T1,T2>()
            : JsonConvert.DeserializeObject<Dictionary<T1,T2>>(CurrentValue);
    }

    public override TT Get<TT>()
    {
        if (typeof(TT) != typeof(Dictionary<T1,T2>))
        {
            Reconstruct(CurrentValue);
            if (typeof(TT) != typeof(string))
            {
                Debug.LogError($"Cannot get typeof {typeof(TT).Name} to type {typeof(Dictionary<T1,T2>).Name}");
                throw new InvalidCastException();
            }
        }

        return (TT)Convert.ChangeType(CurrentValue, typeof(TT));
    }

    public override bool Set<TT>(TT value)
    {
        if (typeof(TT) == typeof(Dictionary<T1,T2>))
        {
            return SerializeList(value as Dictionary<T1,T2>);
        }

        if (typeof(TT) == typeof(string))
        {
            return ParseString(value as string);
        }

        throw new InvalidCastException(
            $"Cannot cast value of type {value.GetType().Name} to type {typeof(Dictionary<T1,T2>).Name}");
    }

    private bool SerializeList(Dictionary<T1, T2> dict)
    {
        if (Equals(dict, CurrentDictionary) || ReferenceEquals(dict, CurrentDictionary)) return false;

        CurrentDictionary = dict;

        CurrentValue = JsonUtility.ToJson(dict);

        IsDirty = true;

        return true;
    }

    private bool ParseString(string value)
    {
        string newValue = Convert.ToString(value);

        if (newValue == CurrentValue) return false;

        CurrentValue = newValue;

        CurrentDictionary = JsonConvert.DeserializeObject<Dictionary<T1,T2>>(CurrentValue);

        //todo ChangedCallback

        IsDirty = true;

        return true;
    }

    public override Type GetStatType()
    {
        return typeof(Dictionary<T1,T2>);
    }
}