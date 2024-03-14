using System;

namespace Mek.Models.Stats
{
    public class BoolStat : BaseStat
    {
        public bool CurrentValue { get; protected set; }

        public BoolStat(bool initial = false)
        {
            CurrentValue = initial;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(bool));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = Convert.ToBoolean(value);

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(bool);
        }
    }
}