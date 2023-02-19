using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class CharacterBase 
{
    public int _id;
    
    public string _name;

    public int HP;
    public int MP;
    public float SPEED;

    public float ATKSPEED;
    public double ATK; //���ݷ�
    public double MATK;//���� ���ݷ�
    public double DEF; //����
    public double CRT;// ũ��Ƽ��

    protected void SetCharacterId(int characterId)
    {
        _id = characterId;
        _name = "DUMMY";
        SetBaseStat();
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
