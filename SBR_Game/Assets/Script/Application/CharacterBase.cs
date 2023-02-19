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
    public double ATK; //공격력
    public double MATK;//마법 공격력
    public double DEF; //방어력
    public double CRT;// 크리티컬

    protected void SetCharacterId(int characterId)
    {
        _id = characterId;
        _name = "DUMMY";
        SetBaseStat();
    }
    private void SetBaseStat()
    {
        //추후 캐릭터별 기본 스탯으로 조정
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
