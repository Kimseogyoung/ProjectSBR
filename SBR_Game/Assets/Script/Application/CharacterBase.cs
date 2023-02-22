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
    public float AttackRangeRadius = 2;//��ä���� ������
    public float AttackRangeAngle = 90;//��ä���� ����

    public float HPBase = 100;
    public float HP = 100;
    public float MPBase = 100;
    public float MP = 100;
    public float SPEED=5;

    public float ATKSPEED = 1;
    public float ATK =1; //���ݷ�
    public float MATK = 1;//���� ���ݷ�
    public float DEF = 1; //����
    public float CRT = 1;// ũ��Ƽ��

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
        HP = 100;
        MP = 100;
        SPEED = 3;
        ATKSPEED = 1;
        ATK = 1;
        MATK= 1;
        DEF = 1;
        CRT = 1;

    }
}
