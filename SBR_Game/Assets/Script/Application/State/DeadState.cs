using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {
        _stateMachine.PlayDieAnim();

        //Dead¿Ã∫•∆Æ Push
        EventQueue.PushEvent(_character.CharacterType == ECharacterType.PLAYER?
            EEventActionType.PLAYER_DEAD: _character.CharacterType == ECharacterType.BOSS? EEventActionType.BOSS_DEAD : EEventActionType.ZZOL_DEAD, 
            new CharacterDeadEvent(_character.Id));

        if(_character.CharacterType == ECharacterType.PLAYER)
        {
            APP.InputManager.RemoveInputAction(EInputAction.MOVE, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);

        }

    }

    protected override void OnExit()
    {
    }

    protected override void Update()
    {
    }
}
