using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuffSkill : SkillBase
{

    protected override void ApplySkill()
    {
        BuffProto prtBuff = ProtoHelper.Get<BuffProto, int>(_skillProto.BuffNum);
        if(_skillProto.TargetTeam == ECharacterTeamType.ENEMY)
        {
            GameLogger.Info("Enemy 대상 버프 스킬 미구현");
        }
        else//HERO
        {
            if(_skillProto.HitTargetSelectType == EHitTargetSelectType.SELF)
            {
                BuffBase buff = new BuffBase(_character, prtBuff);
                buff.Apply();
            }
            else
            {
                _hitBox = new HitBox(_skillProto.HitShapeType, _skillProto.Range, _skillProto.Angle, _character.CurPos, _character.CurDir);
                APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, _skillProto.TargetTeam,
                    _skillProto.HitTargetType, _skillProto.HitTargetSelectType, _attackType, _skillProto.TargetCnt, _skillProto.MultiplierValue);
            }

        }
    }

    protected override void ResetSkill()
    {

    }

    protected override void UpdateSkill()
    {
       
    }
}

