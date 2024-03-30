using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public StateMachine stateMachine;

    public BaseState(StateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}
