using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Stat
{
    public EStat StatType { get; private set; }
    public string Name { get { return StatType.ToString(); } }
    public float BaseStat { get; private set; }
    public float PlusStat { get; private set; } = 0;
    public float PercentStat { get; private set; } = 0;
    private float _currentStat;

    public Stat(EStat statType, float baseStat)
    {
        StatType = statType;
        BaseStat = baseStat;
        _currentStat = BaseStat;
    }

    public void ChangePlusStat(float stat)
    {
        PlusStat += stat;
    }

    public void ChangePercentStat(float stat)
    {
        PercentStat += stat;
    }


    public float TotalValue
    {
        get { return (BaseStat + PlusStat) + ((BaseStat + PlusStat) * Mathf.Min(PercentStat,100) / 100f); }
    }

    public float Value
    {
        get { return _currentStat; }
        set 
        { 
            _currentStat = value;
            if(_currentStat < 0)
            {
                _currentStat = 0;
            } 
            else if(_currentStat > TotalValue)
            {
                _currentStat = TotalValue;
            }
        }
    }


}

public enum EStat
{
    None = 0,
    HP,
    MP,
    SPD ,//이동 속도
    ATKSPD,//공격속도(초당 기본 공격 횟수)
    ATK,  //공격력
    MATK,//마법 공격력
    DEF, //방어력
    CRT,// 크리티컬
    CDR,// 쿨타임 감소 퍼센트
    HPGEN,// 지속 체력회복
    DRAIN,// 피흡
}
