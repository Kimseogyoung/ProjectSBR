using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HPEvent : EventBase
{
    public int CharacterId;
    public float FullHP;
    public float CurHP;

    public bool IsAttacked;
    public HPEvent(int characterId, float fullHP, float curHP, bool isAttacked)
    {
        CharacterId = characterId;
        FullHP = fullHP;
        CurHP = curHP;
        IsAttacked = isAttacked;
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

