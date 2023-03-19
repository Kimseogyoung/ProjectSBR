using System;
using UnityEngine;

public class NormalProjectileSkill : SkillBase
{
    public NormalProjectileSkill(CharacterBase characterBase, int skillNum) : base(characterBase, skillNum)
    {
    }

    protected override void UseImmediateSkill()
    {

        CharacterBase target = APP.Characters.GetBoss();
        if((target.CurPos- _character.CurPos).magnitude > _skillProto.Range)
        {//범위 밖이면 논타겟
            target = null;
        }

        APP.Bullet.InstantiateBullet(OnFoundTarget, _character,
            _skillProto.ProjectilePrefab, _character.CurDir, 
            _skillProto.ProjectileSpeed, _skillProto.Range, _skillProto.TargetTeam, target);
    }

    private void OnFoundTarget(Vector3 pos, CharacterBase victim)
    {
        GameLogger.Info("{0}이 맞음!", victim.Name);
    }
}

