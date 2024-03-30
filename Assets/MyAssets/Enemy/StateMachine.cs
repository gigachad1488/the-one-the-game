using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState? currentState;

    public void SetState(BaseState state)
    {
        currentState = state;
        currentState?.OnEnter();
    }
}
