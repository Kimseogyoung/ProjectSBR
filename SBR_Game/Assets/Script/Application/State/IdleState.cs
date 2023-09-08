using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//모든 캐릭터 공통 아무것도 안하는 상태 == Stop
public class IdleState : CharacterState<Character>
{
    protected override void OnEnter()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            LOG.W("플레이어 이동 이벤트 삭제");
            APP.InputManager.RemoveInputAction(EInputAction.RUN, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL4, _stateMachine.UseSkill4);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);
        }
        else
        {
        }
    }

    protected override void OnExit()
    {
        if(_character.CharacterType == ECharacterType.PLAYER)
        {
            APP.InputManager.AddInputAction(EInputAction.RUN, _stateMachine.MoveCharacterPos);
            APP.InputManager.AddInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.AddInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.AddInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.AddInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.AddInputAction(EInputAction.SKILL4, _stateMachine.UseSkill4);
            APP.InputManager.AddInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);

        }
    }

    protected override void Update()
    {

    }
}
