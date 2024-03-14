using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public static class EventSystem
{
    private static Dictionary<Events, Dictionary<string, Action<EventData>>> eventDictionary =
        new Dictionary<Events, Dictionary<string, Action<EventData>>>();


    public struct EventData
    {
        public object[] DataArray;

   
    }

    public static void StartListening(Events eventName, string caller, Action<EventData> listener)
    {
        Dictionary<string, Action<EventData>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            events.Add(caller, listener);
        }
        else
        {
            eventDictionary.Add(eventName, new Dictionary<string, Action<EventData>>() { { caller, listener } });
        }
    }

    public static void StopListening(Events eventName, string caller)
    {
        Dictionary<string, Action<EventData>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            events.Remove(caller);
        }
    }

    public static void TriggerEvent(Events eventName, params object[] data)
    {
        Dictionary<string, Action<EventData>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            var eventData = new EventData
            {
                DataArray = data
            };

            for (int i = 0; i < events.Count; i++)
            {
                var item = events.ElementAt(i);
                try
                {
                    item.Value(eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception at Event Name: " + eventName + " on: " + item.Key + " error: " + e);
                }
            }
        }
    }
}