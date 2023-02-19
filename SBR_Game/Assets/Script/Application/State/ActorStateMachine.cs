using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStateMachine : StateMachineBase<Player>
{
    protected override void Init()
    {
        APP.InputManager.AddInputAction(EInputAction.MOVE, MoveCharacter);   
    }
  
}
