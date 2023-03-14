//------------------------------------------------------------------- Excel
public enum EStat
{
    NONE = 0,
    HP,
    MP,
    SPD,//이동 속도
    ATKSPD,//공격속도(초당 기본 공격 횟수)
    ATK,  //공격력
    MATK,//마법 공격력
    DEF, //방어력
    CRT,// 크리티컬
    CDR,// 쿨타임 감소 퍼센트
    HPGEN,// 지속 체력회복
    RANGE,//기본 공격 사거리
    DRAIN,// 피흡
}

public enum ECharacterType
{
    NONE = 0,
    PLAYER,
    HERO,
    BOSS,
    ZZOL
}

//-------------------------------------------------------------------

public enum ECharacterTeamType
{
    NONE = 0,
    HERO,
    ENEMY
}

public enum EInputAction
{
    NONE = 0,
    MOVE,
    PAUSE,
    PLAY,
    ESC,
    TAB,
    FAST_MODE,
    ATTACK,
    SKILL1,
    SKILL2,
    SKILL3,
    ULT_SKILL
}

public enum EAttack
{
    NONE = 0,
    ATK,
    MATK
}


public enum EEventActionType
{
    NONE = 0,
    PAUSE,
    PLAY,

    //Game
    LOAD_INGAME,

    //UI
    PLAYER_HP_CHANGE,
    PLAYER_MP_CHANGE,
    BOSS_HP_CHANGE,
    ZZOL_HP_CHANGE,
    PHASE1_START,
    PHASE2_START,
    STAGE_INFO_SET,
    PLAYER_DEAD,
    BOSS_DEAD,
    ZZOL_DEAD
}

public enum EHitType
{
    NONE = 0,
    ALONE,
    ALL
}

public enum EHitShape
{
    NONE = 0,
    CIRCLE,//원
    CORN,//원뿔
    SQURE
}