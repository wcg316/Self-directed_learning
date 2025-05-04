using UnityEngine;
using System.Collections.Generic;

public class FSM<T>
{
    IState<T> currentInstance;
    Dictionary<T, IState<T>> instances = new();
    public T CurrentState
    {
        get;
        private set;
    }

    public void AddState(T state, IState<T> instance)
    {
        instances[state] = instance;
    }

    public void ChangeStateTo(T state)
    {
        currentInstance?.Exit();
        currentInstance = instances[state];
        CurrentState = state;
        currentInstance.Enter();
    }

    public void Execute(FSM<T> fsm)
    {
        currentInstance?.Execute(this);
    }
}
