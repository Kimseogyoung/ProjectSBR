﻿using UnityEditor;
using UnityEngine;


public class ChannelingState : CharacterState<Character>
{
    private bool _canCancel = false;
    public ChannelingState(float time, bool canCancel)
    {
        _canCancel = canCancel;
        TimeHelper.AddTimeEvent("channeling-state", time, ()=> { _stateMachine.SetState(new playerNormalState()); });
    }

    protected override void OnEnter()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            GameLogger.Strong("플레이어 이동 이벤트 삭제");
            APP.InputManager.RemoveInputAction(EInputAction.RUN, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL4, _stateMachine.UseSkill4);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);

            if(_canCancel)
                APP.InputManager.AddInputAction(EInputAction.RUN, Exit);
        }
    }

    protected override void OnExit()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            if (_canCancel)
                APP.InputManager.RemoveInputAction(EInputAction.RUN, Exit);

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

    private void Exit(Vector2 dir)
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            _stateMachine.CancelCurrentSkill();
        }
    }
}