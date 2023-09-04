using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NormalTargetAttack : SkillBase
{
    protected override void UpdateSkill()
    {
        
    }

    protected override void ApplySkill()
    {
        if ((_target.CurPos - _character.CurPos).magnitude > Prt.Range)
        {
            GameLogger.I($"{_character.Name}의 스킬 : {Prt.Name} 실행 취소");
            return;
        }

        _target.ApplyDamageWithMuliply(_character, 1);
    }
    protected override void ResetSkill()
    {
       
    }
}

