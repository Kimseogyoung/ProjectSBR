using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Stat
{
    public EStat StatType { get; private set; }
    public string Name { get { return StatType.ToString(); } }
    [field: SerializeField] public float BaseStat { get; private set; }
    [field: SerializeField] public float PlusStat { get; private set; } = 0;
    [field: SerializeField] public float PercentStat { get; private set; } = 0;
    [field: SerializeField] private float _currentStat;

    public Stat(EStat statType, float baseStat)
    {
        StatType = statType;
        BaseStat = baseStat;
        _currentStat = BaseStat;
    }

    public void ChangePlusStat(float stat)
    {
        PlusStat += stat;
        ApplyCurrentValue();
    }

    public void ChangePercentStat(float stat)
    {
        PercentStat += stat;
        ApplyCurrentValue();
    }

    public float FullValue
    {
        get { return (BaseStat + PlusStat) + ((BaseStat + PlusStat) * PercentStat); }
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
            else if(_currentStat > FullValue)
            {
                _currentStat = FullValue;
            }
        }
    }

    private void ApplyCurrentValue()
    {
        if (StatType == EStat.HP || StatType == EStat.MP)
        {
            return;
        }

        _currentStat = FullValue;
    }


}

