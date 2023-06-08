using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyFollowState : CharacterState<Character>
{
    private Character _target;
    private Dictionary<int, float> _attackerDamageDict = new Dictionary<int, float>();
    private Action<EventBase> _handlerInstance;
    protected override void OnEnter()
    {
        List<Character> heroList = APP.InGame.GetLivedHeroList();
        _target = heroList.OrderBy(h => (h.CurPos - _character.CurPos).magnitude).FirstOrDefault();
        _attackerDamageDict.Add(_target.Id, 0);

        _handlerInstance = EventQueue.AddEventListener<HPEvent>(EEventActionType.ENEMY_HP_CHANGE, UpdateTarget);
    }

    protected override void OnExit()
    {
        EventQueue.RemoveEventListener(EEventActionType.ENEMY_HP_CHANGE, _handlerInstance);
    }


    protected override void Update()
    {
        if (_character.IsDead())
        {
            _stateMachine.SetState(new DeadState());
            return;
        }

        if (_target.IsDead())
        {
            _stateMachine.SetState(new IdleState());
            return;
        }

        Vector3 dir = (_target.CurPos - _character.CurPos);
        //공격 가능한 범위인가
        if (dir.magnitude < _character.RANGE.Value - 0.5f)
        {
            _stateMachine.SetState(new EnemyAttackState(_target));
            return;
        }
         
        _stateMachine.MoveCharacterPos(dir.normalized.ConvertVec2());
    }

    private void UpdateTarget(HPEvent evt)
    {
        if (evt.CharacterId != _character.Id)
            return;

        if (evt.Attacker == null)
            return;

        if (!_attackerDamageDict.ContainsKey(evt.Attacker.Id))
        {
            _attackerDamageDict.Add(evt.Attacker.Id, evt.DeltaHP);
        }
        else
        {
            _attackerDamageDict[evt.Attacker.Id] = evt.DeltaHP;
        }

        if (_attackerDamageDict[_target.Id] < _attackerDamageDict[evt.Attacker.Id])
        {
            _target = evt.Attacker;
        }
    }


}
