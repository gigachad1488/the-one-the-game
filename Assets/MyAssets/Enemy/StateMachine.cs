using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState? currentState;

    public void SetState(BaseState state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }
}
