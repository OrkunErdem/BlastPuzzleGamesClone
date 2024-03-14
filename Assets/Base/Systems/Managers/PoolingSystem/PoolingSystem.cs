﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class PoolObject
{
    public string Name;
    public int Count;
    public GameObject Object;
}

public class PoolingSystem : Singleton<PoolingSystem>
{
    private bool _isLoaded;
    private PoolContainer[] _containerArray;

    private Dictionary<string, Queue<PoolQueue<GameObject, PoolType>>> _poolDictionary =
        new Dictionary<string, Queue<PoolQueue<GameObject, PoolType>>>();

    private Dictionary<string, List<PoolQueue<GameObject, PoolType>>> _createDictionary =
        new Dictionary<string, List<PoolQueue<GameObject, PoolType>>>();

    private List<PoolObject> _poolObjectList = new List<PoolObject>();
    private GameObject _poolContainer;

    private void Start()
    {
        Init();
        DontDestroyOnLoad(gameObject);

    }

    public GameObject Instantiate(string id, Vector3 position, Quaternion rotation, Transform parent = null,
        PoolType poolType = PoolType.None)
    {
        if (!_isLoaded) Load();
        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new Queue<PoolQueue<GameObject, PoolType>>();
            _createDictionary[id] = new List<PoolQueue<GameObject, PoolType>>();
        }

        if (_poolDictionary[id].Count == 0)
        {
            CreateObject(id, poolType, parent);
        }

        var item = _poolDictionary[id].Dequeue();
        item.Item2 = poolType;
        var obj = item.Item1;

        _createDictionary[id].Add(new PoolQueue<GameObject, PoolType>(obj, poolType));
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnGetFromPool();

        obj.transform.SetParent(parent == null ? _poolContainer.transform : parent);
        obj.transform.localPosition = position;
        obj.transform.localRotation = rotation;
        obj.SetActive(true);
        return obj;
    }
    public GameObject InstantiateUI(string id, Vector2 rectPos, Quaternion rotation, Transform parent = null,
        PoolType poolType = PoolType.None)
    {
        if (!_isLoaded) Load();
        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new Queue<PoolQueue<GameObject, PoolType>>();
            _createDictionary[id] = new List<PoolQueue<GameObject, PoolType>>();
        }

        if (_poolDictionary[id].Count == 0)
        {
            CreateObject(id, poolType, parent);
        }

        var item = _poolDictionary[id].Dequeue();
        item.Item2 = poolType;
        var obj = item.Item1;

        _createDictionary[id].Add(new PoolQueue<GameObject, PoolType>(obj, poolType));
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnGetFromPool();
        obj.GetComponent<RectTransform>().anchoredPosition = rectPos;
        
        obj.transform.SetParent(parent == null ? _poolContainer.transform : parent);
        obj.transform.localRotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public GameObject Instantiate(string id, Transform parent = null, PoolType poolType = PoolType.None)
    {
        if (!_isLoaded) Load();
        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new Queue<PoolQueue<GameObject, PoolType>>();
            _createDictionary[id] = new List<PoolQueue<GameObject, PoolType>>();
        }

        if (_poolDictionary[id].Count == 0)
        {
            CreateObject(id, poolType, parent);
        }

        var item = _poolDictionary[id].Dequeue();
        item.Item2 = poolType;
        var obj = item.Item1;
        _createDictionary[id].Add(new PoolQueue<GameObject, PoolType>(obj, poolType));
        IPoolable poolable = obj.GetComponent<IPoolable>();
        if (poolable != null) poolable.OnGetFromPool();

        obj.transform.SetParent(parent == null ? _poolContainer.transform : parent);
        obj.SetActive(true);
        return obj;
    }

    public void Destroy(string id, GameObject @object, PoolType poolType = PoolType.None)
    {
        IPoolable poolable = @object.GetComponent<IPoolable>();
        poolable?.OnReturnToPool();
        @object.SetActive(false);
        @object.transform.SetParent(_poolContainer.transform);
        _createDictionary[id].Remove(_createDictionary[id].FirstOrDefault(x => x.Item1 == @object));
        _poolDictionary[id].Enqueue(new(@object, poolType));
    }

    public void DestroyAllByType(PoolType poolType = PoolType.None)
    {
        var dict = _createDictionary;
        foreach (var pool in dict)
        {
            var arr = pool.Value;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].Item2 == poolType)
                {
                    Destroy(pool.Key, arr[i].Item1);
                }
            }
        }
    }

    public void DestroyAll()
    {
        foreach (var pool in _createDictionary)
        {
            var arr = pool.Value.ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                Destroy(pool.Key, arr[i].Item1);
            }
        }
    }

    private void Init()
    {
        if (!_isLoaded) Load();

        foreach (var t in _poolObjectList)
        {
            for (int j = 0; j < t.Count; j++)
            {
                CreateObject(t.Name);
            }
        }
    }


    private PoolQueue<GameObject, PoolType> CreateObject(string id, PoolType poolType = PoolType.None,
        Transform parent = null)
    {
        var obj = Instantiate(_poolObjectList.FirstOrDefault(x => x.Name == id).Object,
            parent ??= _poolContainer.transform);
        obj.SetActive(false);

        if (!_poolDictionary.ContainsKey(id))
        {
            _poolDictionary[id] = new();
            _createDictionary[id] = new();
        }

        var poolQueue = new PoolQueue<GameObject, PoolType>(obj, poolType);
        _poolDictionary[id].Enqueue(poolQueue);
        return poolQueue;
    }


    private void Load()
    {
        _containerArray = Resources.LoadAll<PoolContainer>("PoolingSystemData");
        foreach (var _container in _containerArray)
        {
            _poolObjectList.AddRange(_container.PoolObjectList);
        }

        if (_poolContainer == null)
        {
            _poolContainer = new GameObject("PoolContainer");
            DontDestroyOnLoad(_poolContainer);
        }

        _isLoaded = true;
    }
}

public enum PoolType
{
    None,
    UI,
    Level,
    GamePlay
}

public class PoolQueue<GameObject, PoolType>
{
    public GameObject Item1;
    public PoolType Item2;

    public PoolQueue(GameObject item1, PoolType item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}