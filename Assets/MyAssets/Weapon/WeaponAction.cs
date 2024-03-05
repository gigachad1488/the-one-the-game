using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    public Weapon weapon;
    public List<Module> modules = new List<Module>();

    public abstract class Module : IModule<WeaponAction>
    {
        [SerializeField]
        private WeaponAction action;
        public void Set(WeaponAction t)
        {
            action = t;
        }
    }

    public void OnAction()
    {

    }
}


