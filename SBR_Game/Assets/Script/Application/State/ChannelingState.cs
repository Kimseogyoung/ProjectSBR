using UnityEditor;
using UnityEngine;


public class ChannelingState : CharacterState<CharacterBase>
{
    protected override void OnEnter()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            GameLogger.Strong("플레이어 이동 이벤트 삭제");
            APP.InputManager.RemoveInputAction(EInputAction.MOVE, _stateMachine.MoveCharacterPos);
            APP.InputManager.RemoveInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.RemoveInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.RemoveInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);

            APP.InputManager.AddInputAction(EInputAction.MOVE, Exit);
        }
    }

    protected override void OnExit()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            APP.InputManager.RemoveInputAction(EInputAction.MOVE, Exit);

            APP.InputManager.AddInputAction(EInputAction.MOVE, _stateMachine.MoveCharacterPos);
            APP.InputManager.AddInputAction(EInputAction.ATTACK, _stateMachine.Attack);
            APP.InputManager.AddInputAction(EInputAction.SKILL1, _stateMachine.UseSkill1);
            APP.InputManager.AddInputAction(EInputAction.SKILL2, _stateMachine.UseSkill2);
            APP.InputManager.AddInputAction(EInputAction.SKILL3, _stateMachine.UseSkill3);
            APP.InputManager.AddInputAction(EInputAction.ULT_SKILL, _stateMachine.UseUltSkill);
        }

    }

    protected override void Update()
    {

    }

    private void Exit()
    {
        if (_character.CharacterType == ECharacterType.PLAYER)
        {
            TimeHelper.RemoveTimeEvent("channeling");
            _stateMachine.SetState(new playerNormalState());
        }
    }
}