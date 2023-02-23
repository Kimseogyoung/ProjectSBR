using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {
        //Dead¿Ã∫•∆Æ Push
        EventQueue.PushEvent(_character.CharacterType == ECharacterType.Player?
            EEventActionType.PlayerDead: _character.CharacterType == ECharacterType.Boss? EEventActionType.BossDead : EEventActionType.ZzolDead, 
            new CharacterDeadEvent(_character.Id));

        if(_character.CharacterType == ECharacterType.Player)
        {
            APP.InputManager.RemoveInputAction(EInputAction.MOVE, _stateMachine.MoveCharacter);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
        }

    }

    protected override void OnExit()
    {
    }

    protected override void Update()
    {
    }
}
