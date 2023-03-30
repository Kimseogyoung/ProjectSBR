using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static BulletManager;

public class MovingSkill : SkillBase
{
    private int _attackTargetCnt = 0;
    private bool _isMoving = false;
    protected override void UpdateSkill()
    {
        if (!_isMoving)
            return;

        _character.TranslatePos( _firstSkillDir * _skillProto.Speed * Time.fixedDeltaTime);
        FindFrontTarget();
        if ((_firstSkillPos - _character.CurPos).magnitude > _skillProto.Range)
        {
            _isMoving = false;
        }

    }

    protected override void ApplySkill()
    {

    }


    protected override void ResetSkill()
    {
        _isMoving = true;
        _attackTargetCnt = 0;

    }
    private void FindFrontTarget()
    {
        if (_attackTargetCnt >= _skillProto.TargetCnt)
            return;

        RaycastHit hit;
        Debug.DrawRay( _character.CurPos, _firstSkillDir, Color.red, 0.1f);
        if (Physics.Raycast(_character.CurPos, _firstSkillDir, out hit, 0.1f, _skillProto.TargetTeam== ECharacterTeamType.ENEMY? LayerMask.GetMask("Enemy"):LayerMask.GetMask("Hero")))
        {
            _attackTargetCnt++;
            CharacterBase victim = hit.collider.GetComponent<StateMachineBase>().GetCharacter();
            victim.ApplySkillDamage(_character, _skillProto);
            GameLogger.Info($"{victim.Name}을 밀음");
            _attackTargetCnt++;
        }
    }
}
