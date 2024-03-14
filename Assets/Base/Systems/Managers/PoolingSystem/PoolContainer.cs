using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolContainer", menuName = "ScriptableObjects/PoolContainer", order = 1)]
public class PoolContainer : ScriptableObject
{
    private bool isLoaded;
    public Dictionary<string, PoolObject> PoolObjects
    {
        get
        {

            if (isLoaded) Load();

            return poolObjects;
        }
        set
        {
            PoolObjects = value;
        }
    }
    private Dictionary<string, PoolObject> poolObjects = new Dictionary<string, PoolObject>();

    [SerializeField]
    public List<PoolObject> PoolObjectList = new List<PoolObject>();

    private void OnEnable()
    {
        Load();
    }
    private void Load()
    {
        foreach (var item in PoolObjectList)
        {
            poolObjects[item.Name] = item;
        }
        isLoaded = true;
    }
}