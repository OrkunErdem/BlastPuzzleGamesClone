using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnGetFromPool();
    void OnReturnToPool();
    string PoolInstanceId { get; set; }
}
