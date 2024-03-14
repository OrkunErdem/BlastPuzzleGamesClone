using System;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class Vector2Stat : BaseStat
    {
        public Vector2 CurrentValue { get; protected set; }

        public Vector2Stat(Vector2 initial)
        {
            CurrentValue = initial;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(Vector2));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = (Vector2)Convert.ChangeType(value, typeof(Vector2));

            if (newValue == CurrentValue)
            {
                return false;
            }

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(Vector2);
        }
    }
}