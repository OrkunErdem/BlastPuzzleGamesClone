using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


public class ListStat<T> : StringStat
    {
        public List<T> CurrentList { get; protected set; }

        public ListStat(List<T> initial) : base(JsonConvert.SerializeObject(initial))
        {
            var json = JsonConvert.SerializeObject(initial);
            CurrentList = initial;
            CurrentValue = json;
        }

        public ListStat() {}

        public override void Reconstruct(string value)
        {
            base.Reconstruct(value);

            CurrentValue = value;
            
            CurrentList = string.IsNullOrEmpty(CurrentValue) ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(CurrentValue);
        }

        public override TT Get<TT>()
        {
            if (typeof(TT) != typeof(List<T>))
            {
                Reconstruct(CurrentValue);
                if (typeof(TT) != typeof(string))
                {
                    Debug.LogError($"Cannot get typeof {typeof(TT).Name} to type {typeof(List<T>).Name}");
                    throw new InvalidCastException();
                }
            }

            return (TT)Convert.ChangeType(CurrentValue, typeof(TT));
        }

        public override bool Set<TT>(TT value)
        {
            if (typeof(TT) == typeof(List<T>))
            {
                return SerializeList(value as List<T>);
            }
            
            if (typeof(TT) == typeof(string))
            {
                return ParseString(value as string);
            }

            throw new InvalidCastException(
                $"Cannot cast value of type {value.GetType().Name} to type {typeof(List<T>).Name}");
        }

        private bool SerializeList(List<T> list)
        {
            if (Equals(list, CurrentList) || ReferenceEquals(list, CurrentList)) return false;

            CurrentList = list;

            CurrentValue = JsonUtility.ToJson(list);

            IsDirty = true;

            return true;
        }

        private bool ParseString(string value)
        {
            string newValue = Convert.ToString(value);

            if (newValue == CurrentValue) return false;

            CurrentValue = newValue;

            CurrentList = JsonConvert.DeserializeObject<List<T>>(CurrentValue);
            
            //todo ChangedCallback

            IsDirty = true;

            return true;
        }

        public override Type GetStatType()
        {
            return typeof(List<T>);
        }
    }
