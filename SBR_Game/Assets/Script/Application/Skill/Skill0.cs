using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.TextCore.Text;

public class Skill0 : SkillBase
{
    protected override void UpdateSkill()
    {
        
    }

    protected override void ApplySkill()
    {
        _hitBox = new HitBox(EHitShapeType.SQURE, _character.CurPos + _character.CurDir * 2, _character.CurDir, 1, 2);
        APP.Characters.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
            EHitSKillType.NONTARGET, _skillProto.HitTargetSelectType, _attackType, _skillProto.TargetCnt, _skillProto.MultiplierValue);
    }
}
