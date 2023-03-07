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

    public float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//������, �ǰ���, ���ݷ� ����, ������ ���
    {
        float damage = atk == EAttack.ATK ? attacker.ATK.Value : attacker.MATK.Value;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//ũ��Ƽ�� ����

        damage *= (100 - victim.DEF.Value) / 100f;//�ǰ��� ���� ���� (ex 1% ����)

        return damage;

    }

    public float ApplyDamage(float damage)
    {
        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            //����
            GameLogger.Strong("{0}�� �׾���.", Name);
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
        {//ũ�� ����
            return true;
        }
        return false;
    }

    private void SetBaseStat()
    {
        //���� ĳ���ͺ� �⺻ �������� ����
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
