using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyAttackState : CharacterState<CharacterBase>
{
    private CharacterBase _target;
    public EnemyAttackState(CharacterBase target)
    {
        _target = target;
    }

    protected override void OnEnter()
    {
        _stateMachine._currentTarget = _target;
        //TODO: 공격 애니메이션 재생

        //TODO: 타겟팅
        _stateMachine.Attack();

        _stateMachine.SetState(new EnemyFollowState());
    }

    protected override void OnExit()
    {

    }

    protected override void Update()
    {
    }

}
