using System;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class Vector3Stat : BaseStat
    {
        public Vector3 CurrentValue { get; protected set; }
        
        public Vector3Stat(Vector3 initial)
        {
            CurrentValue = initial;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(Vector3));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = (Vector3)Convert.ChangeType(value, typeof(Vector3));

            if (newValue == CurrentValue)
            {
                return false;
            }

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(Vector3);
        }
    }
}