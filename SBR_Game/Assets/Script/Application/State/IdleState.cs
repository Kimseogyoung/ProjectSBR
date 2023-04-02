using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��� ĳ���� ���� �ƹ��͵� ���ϴ� ���� == Stop
public class IdleState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            GameLogger.Strong("�÷��̾� �̵� �̺�Ʈ ����");
            APP.InputManager.RemoveInputAction(EInputAction.RUN, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill2);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL4, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseDodgeSkill);
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
            APP.InputManager.AddInputAction(EInputAction.SKILL3, _stateMachine.UseSkill2);
            APP.InputManager.AddInputAction(EInputAction.SKILL4, _stateMachine.UseSkill3);
            APP.InputManager.AddInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);
            APP.InputManager.AddInputAction(EInputAction.SKILL2, _stateMachine.UseDodgeSkill);

        }
    }

    protected override void Update()
    {

    }
}
