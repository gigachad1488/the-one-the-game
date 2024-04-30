using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleData 
{
    public string className;
    public string addressablesPath;

    public int level;

    public List<ModuleDataType> modules = new List<ModuleDataType>();
}
