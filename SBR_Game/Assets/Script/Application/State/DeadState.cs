using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : CharacterState<Character>
{
    protected override void OnEnter()
    {
        _stateMachine.PlayDieAnim();
        _character.OnDieCharacter(_character);

        //Dead�̺�Ʈ Push
        EventQueue.PushEvent(_character.CharacterType == ECharacterType.PLAYER?
            EEventActionType.PLAYER_DEAD: _character.CharacterType == ECharacterType.BOSS? EEventActionType.BOSS_DEAD : EEventActionType.ZZOL_DEAD, 
            new CharacterDeadEvent(_character.Id));

        if(_character.CharacterType == ECharacterType.PLAYER)
        {
            APP.InputManager.RemoveInputAction(EInputAction.RUN, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL4, _stateMachine.UseSkill4);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);

        }

    }

    protected override void OnExit()
    {
    }

    protected override void Update()
    {
    }
}
