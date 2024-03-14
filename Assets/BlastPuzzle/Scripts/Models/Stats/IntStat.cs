using System;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class IntStat : BaseStat
    {
        public int Min { get; protected set; }
        public int Max { get; protected set; }
        public int CurrentValue { get; protected set; }

        public IntStat(int initial)
        {
            Min = int.MinValue;
            Max = int.MaxValue;
            CurrentValue = initial;
        }
        public IntStat(int min, int max, int initial)
        {
            Min = min;
            Max = max;
            CurrentValue = initial;
        }

        public IntStat(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(int));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = Mathf.Clamp(Convert.ToInt32(value), Min, Max);

            if (newValue == CurrentValue)
            {
                return false;
            }

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(int);
        }
    }
}