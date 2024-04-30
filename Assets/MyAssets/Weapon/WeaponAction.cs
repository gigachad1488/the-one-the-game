using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAction : MonoBehaviour, IModule<Weapon>
{
    [HideInInspector]
    public Weapon weapon;
    public List<ActionModule> modules = new List<ActionModule>();

    public GameObject weaponModel;

    public string className;

    public int level { get; set; }

    public delegate void WeaponActionDelegate(Vector2 directionOffset);
    public event WeaponActionDelegate? OnWeaponAction;
    public abstract void Action();

    private void Start()
    {
        className = this.GetType().Name;
    }

    public void Set(Weapon t)
    {    
        weapon = t;

        ActionModule[] mods = GetComponentsInChildren<ActionModule>();
        modules.Clear();

        foreach (ActionModule m in mods) 
        {
            m.gameObject.SetActive(true);
            modules.Add(m);
            m.Set(this);
        }

        AfterSet();
    }

    public abstract void AfterSet();

    public void InvokeAction(Vector2 directionOffset)
    {
        OnWeaponAction?.Invoke(directionOffset);
    }

    public ModuleDataType GetAllData()
    {
        ModuleDataType data = GetData();

        foreach (var module in modules) 
        {
            data.data.modules.Add(module.GetData());
        }

        return data;
    }

    public abstract ModuleDataType GetData();

    public abstract void SetData(ModuleDataType data);
}

public abstract class ActionModule : MonoBehaviour, IModule<WeaponAction>
{
    [HideInInspector]
    public WeaponAction action;

    public string className;

    public int level { get; set; }

    private void Start()
    {
        className = this.GetType().Name;
    }

    public void Set(WeaponAction t)
    {
        action = t;
        action.OnWeaponAction += OnAction;
        AfterSet();
    }

    public abstract void AfterSet();

    public void Dispose()
    {
        action.OnWeaponAction -= OnAction;
    }

    public abstract void OnAction(Vector2 direction);

    public abstract ModuleDataType GetData();

    public abstract void SetData(ModuleDataType data);
}


