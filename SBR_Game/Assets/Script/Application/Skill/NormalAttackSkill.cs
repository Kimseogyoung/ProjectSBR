using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NormalAttackSkill : SkillBase
{
    protected override void UpdateSkill()
    {
        
    }

    protected override void ApplySkill()
    {
        _hitBox = new HitBox(Prt.HitShapeType, Prt.Range, Prt.Angle, _character.CurPos, _character.CurDir);
        APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, Prt.TargetTeam,
            Prt.HitTargetType, Prt.HitTargetSelectType , Prt.TargetCnt ,Prt.MultiplierValue);
    }

    protected override void ResetSkill()
    {
    }
}

