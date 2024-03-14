using System;

namespace Mek.Models.Stats
{
    public class DateStat : BaseStat
    {
        public DateTime CurrentValue { get; protected set; }

        public DateStat(DateTime initial)
        {
            CurrentValue = initial;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(DateTime));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = Convert.ToDateTime(value);

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(DateTime);
        }
    }
}