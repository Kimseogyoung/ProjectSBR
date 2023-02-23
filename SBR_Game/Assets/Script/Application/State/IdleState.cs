using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//모든 캐릭터 공통 아무것도 안하는 상태 == Stop
public class IdleState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {
        if (_character.CharacterType == ECharacterType.Player)
        {
            APP.InputManager.RemoveInputAction(EInputAction.MOVE, _stateMachine.MoveCharacter);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            _stateMachine.SetState(new playerNormalState());
        }
        else
        {
            _stateMachine.SetState(new EnemyFollowState());
        }
    }

    protected override void OnExit()
    {
        if(_character.CharacterType == ECharacterType.Player)
        {
            APP.InputManager.AddInputAction(EInputAction.MOVE, _stateMachine.MoveCharacter);
            APP.InputManager.AddInputAction(EInputAction.ATTACK, _stateMachine.Attack);
        }
    }

    protected override void Update()
    {

    }
}
