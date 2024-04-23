using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModuleData 
{
    public string className;

    public int level;

    public List<ModuleData> modules = new List<ModuleData>();
}
