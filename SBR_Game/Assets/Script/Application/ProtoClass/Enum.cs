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

//ref Character Proto
public enum ECharacterType
{
    NONE = 0,
    PLAYER,
    HERO,
    BOSS,
    ZZOL
}
public enum ECharacterTeamType
{
    NONE = 0,
    HERO,
    ENEMY
}

//ref Skill Proto

public enum EHitSKillType
{
    NONE = 0,
    TARGET,
    NONTARGET,
    ALLTARGET
}

public enum EHitTargetSelectType
{
    NONE = 0,

    CLOSE,
    DIR,
    RANDOM,

    ALL

}

public enum EHitStyleType
{
    NONE = 0,
    IMMEDIATE,
    PROJECTILE,
}
public enum EHitShapeType
{
    NONE = 0,
    CIRCLE,//원
    CORN,//원뿔
    SQURE
}
public enum EGamePropertyType//네이밍 수정 (속성 공격에 해당)
{
    NONE = 0,
    FIRE,
    ICE,
    LIGHT,
    DARK,
    NATURE
}


//-------------------------------------------------------------------

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
    DODGE,
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
    ENEMY_HP_CHANGE,
    PHASE1_START,
    PHASE2_START,
    STAGE_INFO_SET,
    PLAYER_DEAD,
    BOSS_DEAD,
    ZZOL_DEAD
}