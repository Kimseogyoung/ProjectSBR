using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public partial class CharacterBase 
{

    private float AccumulateDamage(CharacterBase attacker, CharacterBase victim, float multiplier =1f)//공격자, 피격자, 공격력 종류, 데미지 계수
    {
        float damage = attacker.ATK.Value;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//크리티컬 적용

        damage *= (100 - victim.DEF.Value) / 100f;//피격자 방어력 적용 (ex 1% 감소)

        return damage;

    }

    public void ApplySkillDamage(CharacterBase attacker, SkillProto skillProto)
    {
        ApplyDamageWithMuliply(attacker, skillProto.MultiplierValue);

        if (skillProto.PushSpeed > 0)
            InGameManager.Skill.AddPushAction(this, skillProto.PushSpeed, skillProto.PushDistance, (CurPos - attacker.CurPos).normalized);
    }

    public float ApplyDamageWithMuliply(CharacterBase attacker, float multiply)
    {
        float damage = AccumulateDamage(attacker, this, multiply);
        return ApplyDamagePure(damage, attacker);
    }

    public float ApplyDamagePercent(float damagePercent, CharacterBase attacker = null)
    {
        float damage = HP.FullValue * damagePercent / 100f;
        return ApplyDamagePure(damage, attacker);
    }

    public float ApplyDamagePure(float damage, CharacterBase attacker = null)
    {
        if(damage <= 0)
        {
            return 0;
        }

        if(attacker != null)
        {
            GameLogger.Strong($"{attacker.Name}이 {Name}  때림");
        }

        EventQueue.PushEvent<ShowTextEvent>(EEventActionType.SHOW_DAMAGE_TEXT, new ShowTextEvent(damage, CurPos, (attacker.CurPos - CurPos).normalized));

        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            //죽음
            GameLogger.Strong("{0}는 죽었다.", Name);
        }

        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.PLAYER ? EEventActionType.PLAYER_HP_CHANGE : EEventActionType.ENEMY_HP_CHANGE,
            new HPEvent(Id, damage, HP.FullValue, HP.Value, true, attacker));

        return damage;
    }

    public float ApplyHealPercent(float healPercent)
    {
        float heal = HP.FullValue * healPercent / 100f;
        return ApplyHealPure(heal);
    }

    public float ApplyHealPure(float heal)
    {
        float aftHp = HP.Value + heal;
        float realHeal = HP.FullValue < aftHp ? heal - (aftHp - HP.FullValue) : heal;

        if (realHeal <= 0)
        {
            return 0;
        }

        EventQueue.PushEvent<ShowTextEvent>(EEventActionType.SHOW_HEAL_TEXT, new ShowTextEvent(realHeal, CurPos));

        HP.Value += realHeal;

        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.PLAYER ? EEventActionType.PLAYER_HP_CHANGE : EEventActionType.ENEMY_HP_CHANGE,
            new HPEvent(Id, realHeal, HP.FullValue, HP.Value, false, null));

        return realHeal;
    }

    public float ApplyMpPercent(float mpPercent)
    {
        float mp = MP.FullValue * mpPercent / 100f;
        return ApplyMpPure(mp);
    }

    public float ApplyMpPure(float mp)
    {
        if (mp > 0)
        {
            MP.Value += mp;
        }
        else if(mp < 0)
        {
            MP.Value -= mp;
        }

        return mp;
    }

}
