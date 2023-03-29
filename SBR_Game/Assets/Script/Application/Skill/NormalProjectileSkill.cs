using System;
using UnityEngine;

public class NormalProjectileSkill : SkillBase
{
    protected override void UpdateSkill()
    {
    }

    protected override void ApplySkill()
    {
        CharacterBase target = null;

        if (_skillProto.HitTargetType == EHitSKillType.NONTARGET)
        {
            target = APP.Characters.GetBoss();
            if ((target.CurPos - _character.CurPos).magnitude > _skillProto.Range)
            {//범위 밖이면 논타겟
                target = null;
            }
        }

        APP.Bullet.InstantiateBullet(OnFoundTarget, _character,
            _skillProto.ProjectilePrefab, _character.CurDir, 
            _skillProto.Speed, _skillProto.Range, _skillProto.TargetTeam, target);
    }

    private void OnFoundTarget(Vector3 pos, CharacterBase victim)
    {
        GameLogger.Info("{0}이 맞음!", victim.Name);
        if (_skillProto.HitShapeType == EHitShapeType.NONE)
        {
            victim.ApplySkillDamage(_character, _skillProto);
            return;
        }

        APP.Characters.FindTargetAndApplyDamage(_character,
            new HitBox(_skillProto.HitShapeType, pos, _skillProto.HitWidth), _skillProto.TargetTeam,
            _skillProto.HitTargetType, _skillProto.HitTargetSelectType, EAttack.ATK, _skillProto.TargetCnt, _skillProto.MultiplierValue);

    }
}

