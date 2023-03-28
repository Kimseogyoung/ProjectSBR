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
        if ((_target.CurPos - _character.CurPos).magnitude > _skillProto.Range)
        {
            GameLogger.Info($"{_character.Name}의 스킬 : {_skillProto.Name} 실행 취소");
            return;
        }

        _target.ApplyDamage(_character, EAttack.ATK, 1);
    }
}

