using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

abstract public class CharacterState<T> where T : CharacterBase
{
    protected T _character;
    protected ICharacterAccessible _characterList;
    protected StateMachineBase _stateMachine;

    public void OnEnterBase(T character, ICharacterAccessible characterList, StateMachineBase stateMachine) 
    {
        _character = character;
        _characterList = characterList;
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