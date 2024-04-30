using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule<T> where T : Component
{
    public int level { get; set; }
    public void Set(T t);

    public abstract void AfterSet();

    public abstract ModuleDataType GetData();

    public abstract void SetData(ModuleDataType data);

}
            