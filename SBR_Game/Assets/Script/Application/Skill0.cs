using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.TextCore.Text;

public class Skill0 : SkillBase
{
    public Skill0(CharacterBase character)
    {
        Init(character,
            //EHitShape hitShape, Vector3 centerPos, Vector3 dir, float width, float height
            new HitBox(EHitShapeType.SQURE, character.CurPos + character.CurDir *2, character.CurDir, 1, 2)
            ,EAttack.ATK
            ,1.5f
            , 5);
    }

    protected override void UseContinuosSkill()
    {

    }

    protected override void UseImmediateSkill()
    {
        GameLogger.Info("{0}가 {1} 시전 성공", _character.Name, nameof(Skill0));
        _hitBox.SetDirPos(_character.CurDir, _character.CurPos + _character.CurDir * 2);
        APP.Characters.FindTargetAndApplyDamage(_character, _hitBox, EHitTargetType.ALL, _attackType, _multiply);
    }
}
