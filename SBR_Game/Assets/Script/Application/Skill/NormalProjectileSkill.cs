using System;
using UnityEngine;

public class NormalProjectileSkill : SkillBase
{
    protected override void UpdateSkill()
    {
    }

    protected override void ApplySkill()
    {
        Character target = null;

        if (Prt.HitTargetType == EHitSKillType.NONTARGET)
        {
            target = APP.InGame.GetBoss();
            if ((target.CurPos - _character.CurPos).magnitude > Prt.Range)
            {//범위 밖이면 논타겟
                target = null;
            }
        }

        APP.Bullet.InstantiateBullet(OnFoundTarget, _character,
            Prt.ProjectilePrefab, _character.CurDir, 
            Prt.Speed, Prt.Range, Prt.TargetTeam, target);
    }

    protected override void ResetSkill()
    {
        throw new NotImplementedException();
    }

    private void OnFoundTarget(Vector3 pos, Character victim)
    {
        GameLogger.I("{0}이 맞음!", victim.Name);
        if (Prt.HitShapeType == EHitShapeType.NONE)
        {
            victim.ApplySkillDamage(_character, Prt);
            return;
        }

        APP.InGame.FindTargetAndApplyDamage(_character,
            new HitBox(Prt.HitShapeType, pos, Prt.HitWidth), Prt.TargetTeam,
            Prt.HitTargetType, Prt.HitTargetSelectType, Prt.TargetCnt, Prt.MultiplierValue);

    }
}

