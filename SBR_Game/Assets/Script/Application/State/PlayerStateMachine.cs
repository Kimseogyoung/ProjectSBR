using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachineBase<Player>
{
    
    protected override void Init()
    {
        APP.InputManager.AddInputAction(EInputAction.MOVE, MoveCharacter);  
        APP.InputManager.AddInputAction(EInputAction.ATTACK, Attack);
    }
  
}
