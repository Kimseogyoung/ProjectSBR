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
        BuffProto prtBuff = ProtoHelper.Get<BuffProto>(Prt.BuffNum);
        if(Prt.TargetTeam == ECharacterTeamType.ENEMY)
        {
            LOG.I("Enemy 대상 버프 스킬 미구현");
        }
        else//HERO
        {
            if(Prt.HitTargetSelectType == EHitTargetSelectType.SELF)
            {
                BuffBase buff = new BuffBase(_character, prtBuff);
                buff.Apply();
            }
            else
            {
                _hitBox = new HitBox(Prt.HitShapeType, Prt.Range, Prt.Angle, _character.CurPos, _character.CurDir);
                APP.InGame.FindTargetAndApplyDamage(_character, _hitBox, Prt.TargetTeam,
                    Prt.HitTargetType, Prt.HitTargetSelectType,  Prt.TargetCnt, Prt.MultiplierValue);
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

