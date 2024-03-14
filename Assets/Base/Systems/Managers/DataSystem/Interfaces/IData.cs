using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IData
{
    T GetData<T>(string key);
    void UpdateData<T>(string Key, T value);
    Type GetDataType(); 
}
