using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyAttackState : CharacterState<CharacterBase>
{
    private CharacterBase _player;
    protected override void OnEnter()
    {
        _player = APP.Characters.GetPlayer();
    }

    protected override void OnExit()
    {

    }

    protected override void Update()
    {
        if (_character.IsDead())
        {
            _stateMachine.SetState(new DeadState());
            return;
        }

        if (_player.IsDead())
        {
            _stateMachine.SetState(new IdleState());
            return;
        }

        Vector3 dir = (_player.CurPos - _character.CurPos);

        //공격 불가한 범위인가
        if (dir.magnitude > _character.AttackRangeRadius)
        {
            _stateMachine.SetState(new EnemyFollowState());
            return;
        }

        _stateMachine.NonTargetingDirAttack(dir.normalized);
    }

}
