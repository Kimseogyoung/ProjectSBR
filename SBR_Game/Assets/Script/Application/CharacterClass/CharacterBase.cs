using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EAttack
{
    ATK,
    MATK
}


[Serializable]
public partial class CharacterBase 
{
    public ECharacterType CharacterType;
    public int Id;
    
    public string Name;

    public Vector3 CurDir = Vector3.zero;
    public Vector3 CurPos = Vector3.zero;


    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    public CharacterBase(int characterId)
    {
        Id = characterId;
        Name = "DUMMY";
        SetBaseStat();
    }

    public float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//공격자, 피격자, 공격력 종류, 데미지 계수
    {
        float damage = atk == EAttack.ATK ? attacker.ATK.Value : attacker.MATK.Value;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//크리티컬 적용

        damage *= (100 - victim.DEF.Value) / 100f;//피격자 방어력 적용 (ex 1% 감소)

        return damage;

    }

    public float ApplyDamage(float damage)
    {
        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            //죽음
            GameLogger.Strong("{0}는 죽었다.", Name);
        }

        
        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.Player? EEventActionType.PlayerHpChange:
            CharacterType == ECharacterType.Boss? EEventActionType.BossHpChange : EEventActionType.ZzolHpChange,
            new HPEvent(Id, HP.TotalValue, HP.Value, true));

        return damage;
    }


    public void SetCharacterType(ECharacterType characterType)
    {
        CharacterType = characterType;
    }

    public bool IsDead()
    {
        return HP.Value <= 0 ? true : false;
    }

    public SkillBase GetSkill(EInputAction inputAction) { return _skillList[inputAction]; }

    private bool CheckCritical()
    {
        System.Random rand = new System.Random(DateTime.Now.Millisecond);
        if(rand.Next(100) <= CRT.Value)
        {//크리 터짐
            return true;
        }
        return false;
    }

    private void SetBaseStat()
    {
        //추후 캐릭터별 기본 스탯으로 조정
        if(Id == 1001)
        {
            HP = new Stat(EStat.HP, 100);
            MP = new Stat(EStat.MP, 100);
            SPD = new Stat(EStat.SPD, 3);
            ATKSPD = new Stat(EStat.ATKSPD, 1);
            ATK = new Stat(EStat.ATK, 10);
            MATK = new Stat(EStat.MATK, 10);
            DEF = new Stat(EStat.DEF, 10);
            CRT = new Stat(EStat.CRT, 1);
            CDR = new Stat(EStat.CDR, 1);
            DRAIN = new Stat(EStat.DRAIN, 1);
            HPGEN = new Stat(EStat.HPGEN, 1);

            AttackRangeRadius = 2;
        }
        else
        {
            HP = new Stat(EStat.HP, 100);
            MP = new Stat(EStat.MP, 100);
            SPD = new Stat(EStat.SPD, 3);
            ATKSPD = new Stat(EStat.ATKSPD, 1);
            ATK = new Stat(EStat.ATK, 2);
            MATK = new Stat(EStat.MATK, 10);
            DEF = new Stat(EStat.DEF, 10);
            CRT = new Stat(EStat.CRT, 1);
            CDR = new Stat(EStat.CDR, 1);
            DRAIN = new Stat(EStat.DRAIN, 1);
            HPGEN = new Stat(EStat.HPGEN, 1);

            AttackRangeRadius = 1.5f;
            AttackRangeAngle = 180;
        }

        _skillList.Add(EInputAction.SKILL1, new Skill0(this));
        _skillList.Add(EInputAction.SKILL2, new Skill0(this));
        _skillList.Add(EInputAction.SKILL3, new Skill0(this));
        _skillList.Add(EInputAction.ULT_SKILL, new Skill0(this));

    }
}
