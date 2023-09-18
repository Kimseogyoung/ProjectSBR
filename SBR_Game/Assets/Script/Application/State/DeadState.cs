using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DeadState : CharacterState<Character>
{
    protected override void OnEnter()
    {
        _stateMachine.PlayDieAnim();
        APP.GAME.InGame.UI.RemoveHpBar(_character.CreateNum);

        //Dead¿Ã∫•∆Æ Push

        if(_character.CharacterType == ECharacterType.PLAYER)
            APP.GAME.InGame.Rule.Notify_GameFailure();
        else if (_character.CharacterType == ECharacterType.BOSS)
            APP.GAME.InGame.Rule.Notify_GameSuccess();

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
