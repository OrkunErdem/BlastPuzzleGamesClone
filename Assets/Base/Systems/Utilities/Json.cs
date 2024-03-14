using System;
using Newtonsoft.Json;

public static class Json
{
    public static string ConvertToJson<T>(T value)
    {
        return JsonConvert.SerializeObject(value);
    }
    public static T ConvertFromJson<T>(string value, Type type = null)
    {
        return (T) JsonConvert.DeserializeObject(value, type ?? typeof(T));
    }
}