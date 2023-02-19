using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

abstract public class CharacterState<T> where T : CharacterBase
{
    protected T _character;
    public void OnEnterBase(T character)
    {
        _character = character;
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