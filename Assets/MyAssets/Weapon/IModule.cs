using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule<T> where T : Component
{
    public void Set(T t);

    public abstract void AfterSet();
}
            