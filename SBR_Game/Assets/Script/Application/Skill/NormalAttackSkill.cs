using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NormalAttackSkill : SkillBase
{
    public NormalAttackSkill(CharacterBase character, int skillNum)
        :base(character, skillNum)
    {
    }
    protected override void UseContinuosSkill()
    {
    }

    protected override void UseImmediateSkill()
    {
        GameLogger.Info("{0}가 {1} 시전 성공", _character.Name, nameof(Skill0));
        _hitBox = new HitBox(_skillProto.HitShapeType, _character.CurPos, _skillProto.Range);
        APP.Characters.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
            _skillProto.HitTargetType, _skillProto.HitTargetSelectType ,_attackType,  _skillProto.TargetCnt ,_skillProto.MultiplierValue);
    }
}

