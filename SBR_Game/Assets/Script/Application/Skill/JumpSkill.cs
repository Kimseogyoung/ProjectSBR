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
        _hitBox = new HitBox(Prt.HitShapeType, _character.CurPos, Prt.HitWidth);
        APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, Prt.TargetTeam,
            Prt.HitTargetType, Prt.HitTargetSelectType, Prt.TargetCnt, Prt.MultiplierValue);
    }

    protected override void ResetSkill()
    {
        isJumpFinish = false;
    }

    protected override void UpdateSkill()
    {
        if (isJumpFinish)
            return;

        _character.TranslatePos(_firstSkillDir * Prt.Speed * Time.fixedDeltaTime);
        if ((_firstSkillPos - _character.CurPos).magnitude > Prt.Range)
        {
            OnFinishedJump();
        }
    }

    private void OnFinishedJump()
    {
        isJumpFinish = true;
        _hitBox = new HitBox(Prt.HitShapeType, _character.CurPos, Prt.HitWidth);
        APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, Prt.TargetTeam,
            Prt.HitTargetType, Prt.HitTargetSelectType, Prt.TargetCnt, Prt.MultiplierValue);
    }
}

