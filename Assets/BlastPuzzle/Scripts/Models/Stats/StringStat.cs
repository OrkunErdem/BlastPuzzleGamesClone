using System;


public class StringStat : BaseStat
{
    public string CurrentValue { get; protected set; }

    public StringStat(string initial)
    {
        CurrentValue = initial;
    }

    public StringStat()
    {
    }

    public virtual void Reconstruct(string value)
    {
    }

    public override T Get<T>()
    {
        ValidateType(typeof(T));

        return (T)Convert.ChangeType(CurrentValue, typeof(T));
    }

    public override bool Set<T>(T value)
    {
        ValidateType(typeof(T));

        var newValue = Convert.ToString(value);

        if (newValue == CurrentValue) return false;

        CurrentValue = newValue;

        return base.Set(newValue);
    }

    public override Type GetStatType()
    {
        return typeof(string);
    }
}