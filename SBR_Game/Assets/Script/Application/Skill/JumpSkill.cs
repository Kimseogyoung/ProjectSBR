using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class JumpSkill : SkillBase
{
    bool isJumpFinish = false;
    protected override void ApplySkill()
    {
        isJumpFinish = false;
        _hitBox = new HitBox(_skillProto.HitShapeType, _character.CurPos, _skillProto.HitWidth);
        APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
            _skillProto.HitTargetType, _skillProto.HitTargetSelectType, _attackType, _skillProto.TargetCnt, _skillProto.MultiplierValue);
    }

    protected override void ResetSkill()
    {
        isJumpFinish = false;
    }

    protected override void UpdateSkill()
    {
        if (isJumpFinish)
            return;

        _character.TranslatePos(_firstSkillDir * _skillProto.Speed * Time.fixedDeltaTime);
        if ((_firstSkillPos - _character.CurPos).magnitude > _skillProto.Range)
        {
            OnFinishedJump();
        }
    }

    private void OnFinishedJump()
    {
        isJumpFinish = true;
        _hitBox = new HitBox(_skillProto.HitShapeType, _character.CurPos, _skillProto.HitWidth);
        APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
            _skillProto.HitTargetType, _skillProto.HitTargetSelectType, _attackType, _skillProto.TargetCnt, _skillProto.MultiplierValue);
    }
}

