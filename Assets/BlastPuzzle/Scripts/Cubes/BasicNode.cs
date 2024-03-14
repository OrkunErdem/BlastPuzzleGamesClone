using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BasicNode : MonoBehaviour, IPoolable
{
    public string Id => "0";

    public Vector2Int gridPos;
    string IPoolable.PoolInstanceId { get; set; }

    [NotNull] public string PoolInstanceId => "BasicNode";

    private int _level;


    public void OnGetFromPool()
    {
    }

    public void OnReturnToPool()
    {
    }
}