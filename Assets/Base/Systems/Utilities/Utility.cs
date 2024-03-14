using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
    {
        T key = default;
        
        foreach (KeyValuePair<T, W> pair in dict)
        {
            if (EqualityComparer<W>.Default.Equals(pair.Value, val))
            {
                key = pair.Key;
                break;
            }
        }
        return key;
    }
    public static List<T> Randomize<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); 
            randomizedList.Add(list[index]); 
            list.RemoveAt(index);
        }
        return randomizedList;
    }
    
    public static Vector3 GetWorldPoint(Vector3 screenPoint, float z = 0)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Plane xy = new Plane(Vector3.down, new Vector3(0, 0, z));
        Vector3 worldPos = Vector3.zero;
        if (xy.Raycast(ray, out float distance))
        {
            worldPos = ray.GetPoint(distance);
        }
        return worldPos;
    }
}