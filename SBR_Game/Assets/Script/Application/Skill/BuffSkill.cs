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
                var buffInstance = Activator.CreateInstance(Type.GetType(prtBuff.Class));
                SkillBase skill = (SkillBase)buffInstance;
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

