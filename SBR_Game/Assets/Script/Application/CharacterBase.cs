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
public class CharacterBase 
{
    public ECharacterType CharacterType;
    public int Id;
    
    public string Name;

    public Vector3 CurDir = Vector3.zero;
    public Vector3 CurPos = Vector3.zero;

    //기본공격 관련 스탯
    public float AttackRangeRadius = 2;//부채꼴의 반지름
    public float AttackRangeAngle = 90;//부채꼴의 각도

    public float HPBase = 100;
    public float HP = 100;
    public float MPBase = 100;
    public float MP = 100;
    public float SPD=5;
    public float ATKSPD = 1; //공격속도(초당 기본 공격 횟수)
    public float ATK =1; //공격력
    public float MATK = 1;//마법 공격력
    public float DEF = 1; //방어력
    public float CRT = 1;// 크리티컬
    public float CDR = 1;// 쿨타임 감소 퍼센트

    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    public CharacterBase(int characterId)
    {
        Id = characterId;
        Name = "DUMMY";
        SetBaseStat();
    }

    public float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//공격자, 피격자, 공격력 종류, 데미지 계수
    {
        float damage = atk == EAttack.ATK ? attacker.ATK : attacker.MATK;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//크리티컬 적용

        damage *= (100 - victim.DEF) / 100f;//피격자 방어력 적용 (ex 1% 감소)

        return damage;

    }

    public float ApplyDamage(float damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
            //죽음
            GameLogger.Strong("{0}는 죽었다.", Name);
        }

        
        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.Player? EEventActionType.PlayerHpChange:
            CharacterType == ECharacterType.Boss? EEventActionType.BossHpChange : EEventActionType.ZzolHpChange,
            new HPEvent(Id, HPBase, HP, true));

        return damage;
    }


    public void SetCharacterType(ECharacterType characterType)
    {
        CharacterType = characterType;
    }

    public bool IsDead()
    {
        return HP <= 0 ? true : false;
    }

    public SkillBase GetSkill(EInputAction inputAction) { return _skillList[inputAction]; }

    private bool CheckCritical()
    {
        System.Random rand = new System.Random(DateTime.Now.Millisecond);
        if(rand.Next(100) <= CRT)
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
            HP = 100;
            MP = 100;
            SPD = 3;
            AttackRangeRadius = 2;
            ATKSPD = 1;
            ATK = 10;
            MATK = 1;
            DEF = 1;
            CRT = 1;
        }
        else
        {
            HP = 100;
            MP = 100;
            SPD = 2;
            AttackRangeRadius = 1.5f;
            AttackRangeAngle = 180;
            ATKSPD = 2;
            ATK = 2;
            MATK = 1;
            DEF = 1;
            CRT = 1;
        }

        _skillList.Add(EInputAction.SKILL1, new Skill0(this));
        _skillList.Add(EInputAction.SKILL2, new Skill0(this));
        _skillList.Add(EInputAction.SKILL3, new Skill0(this));
        _skillList.Add(EInputAction.ULT_SKILL, new Skill0(this));

    }
}
