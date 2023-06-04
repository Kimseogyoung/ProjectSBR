using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ShowTextEvent : EventBase
{
    public float Value;
    public Vector3 Pos;
    public Vector3 Dir;

    public ShowTextEvent(float damage, Vector3 pos, Vector3? dir = null)
    {
        Value = damage;
        Pos = pos;        Dir = dir ?? Vector3.zero;

    }
}

public class HPEvent : EventBase
{
    public int CharacterId;
    public float FullHP;
    public float CurHP;
    public float DeltaHP;
    public Character Attacker;

    public bool IsAttacked;
    public HPEvent(int characterId, float deltaHP, float fullHP, float curHP, bool isAttacked, Character attacker)
    {
        CharacterId = characterId;
        FullHP = fullHP;
        CurHP = curHP;
        DeltaHP = deltaHP;
        IsAttacked = isAttacked;
        Attacker = attacker;
    }
}

public class ChangePhaseEvent : EventBase
{
    public int StageNum;
    public int PhaseNum;
    public ChangePhaseEvent(int stageNum, int phaseNum)
    {
        StageNum = stageNum;
        PhaseNum = phaseNum;
    }
}

public class NotiEvent : EventBase
{
    public int NotiType;
    public string Text;
    public NotiEvent(int notiType, string text)
    {
        NotiType = notiType;
        Text = text;
    }
}

public class CharacterDeadEvent : EventBase
{
    public int CharacterId;

    public CharacterDeadEvent(int characterId)
    {
        CharacterId = characterId;
    }
}
public class PauseEvent : EventBase
{
    public bool IsPause;

    public PauseEvent(bool isPause)
    {
        IsPause = isPause;
    }
}


