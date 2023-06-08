using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

abstract public class CharacterState<T> where T : Character
{
    protected T _character;
    protected StateMachineBase _stateMachine;

    public void OnEnterBase(T character, StateMachineBase stateMachine) 
    {
        _character = character;
        _stateMachine = stateMachine;
        OnEnter();
    }

    public void UpdateBase()
    {
        Update();
    }

    public void OnExitBase()
    {
        OnExit();
    }

    abstract protected void OnEnter();
    abstract protected void Update();
    abstract protected void OnExit();

}