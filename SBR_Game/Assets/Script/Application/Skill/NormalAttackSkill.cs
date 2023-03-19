﻿using System;
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

    protected override void UseImmediateSkill()
    {
        _hitBox = new HitBox(_skillProto.HitShapeType, _skillProto.Range, _skillProto.Angle, _character.CurPos, _character.CurDir);
        APP.Characters.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
            _skillProto.HitTargetType, _skillProto.HitTargetSelectType ,_attackType,  _skillProto.TargetCnt ,_skillProto.MultiplierValue);
    }
}

