using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    public bool SaveData<T>(string relativePath, T data, bool encrypted);

    public T LoadData<T>(string relativePath, bool encrypted);
}
