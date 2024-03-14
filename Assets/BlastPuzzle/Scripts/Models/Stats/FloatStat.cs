using System;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class FloatStat : BaseStat
    {
        public float Min { get; protected set; }
        public float Max { get; protected set; }
        public float CurrentValue { get; protected set; }

        public FloatStat(float initial)
        {
            Min = float.MinValue;
            Max = float.MaxValue;
            CurrentValue = initial;
        }

        public FloatStat(float min, float max, float initial)
        {
            Min = min;
            Max = max;
            CurrentValue = initial;
        }

        public FloatStat(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(float));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = Mathf.Clamp(Convert.ToSingle(value), Min, Max);

            if (newValue == CurrentValue)
            {
                return false;
            }

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(float);
        }
    }
}