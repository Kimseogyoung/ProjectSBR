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

    //�⺻���� ���� ����
    public float AttackRangeRadius = 2;//��ä���� ������
    public float AttackRangeAngle = 90;//��ä���� ����

    public float HPBase = 100;
    public float HP = 100;
    public float MPBase = 100;
    public float MP = 100;
    public float SPD=5;
    public float ATKSPD = 1; //���ݼӵ�(�ʴ� �⺻ ���� Ƚ��)
    public float ATK =1; //���ݷ�
    public float MATK = 1;//���� ���ݷ�
    public float DEF = 1; //����
    public float CRT = 1;// ũ��Ƽ��
    public float CDR = 1;// ��Ÿ�� ���� �ۼ�Ʈ

    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    public CharacterBase(int characterId)
    {
        Id = characterId;
        Name = "DUMMY";
        SetBaseStat();
    }

    public float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//������, �ǰ���, ���ݷ� ����, ������ ���
    {
        float damage = atk == EAttack.ATK ? attacker.ATK : attacker.MATK;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//ũ��Ƽ�� ����

        damage *= (100 - victim.DEF) / 100f;//�ǰ��� ���� ���� (ex 1% ����)

        return damage;

    }

    public float ApplyDamage(float damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            HP = 0;
            //����
            GameLogger.Strong("{0}�� �׾���.", Name);
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
