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
public enum EEquipType
{
    NONE = 0,
    // 착용 아이템 부위
    SWORD = 1,
    HEAD = 2,
    BODY = 3,
    GLAVE = 4,
    SHOE = 5,
    ACC = 6,
    ETC = 7,
}

public enum EItemType
{
    NONE = 0,

    // 착용 아이템
    EQUIP = 1,
    SWORD = 1,
    HEAD = 2,
    BODY = 3,
    GLAVE = 4,
    SHOE = 5,
    ACC = 6,

    //보유 아이템
    NORMAL = 10,

    // 일회용 아이템
    ONCE = 11,
    PORTION = 11,
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

    SELF,

    ALL

}

public enum ESkillType
{
    NONE = 0,
    IMMEDIATE,
    PROJECTILE,
    BUFF
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
public enum EBuffType
{
    NONE = 0,
    BUFF,
    DEBUFF
}


//-------------------------------------------------------------------

public enum EInputAction
{
    NONE = 0,
    PAUSE,
    PLAY,
    ESC,
    TAB,
    FAST_MODE,

    //CharacterActionType과 이름이 같아야함.
    RUN = 100,
    ATTACK,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
    ULT_SKILL
}

public enum ECharacterActionType
{
    NONE = 0,
    IDLE,
    START,
    DIE,
    RUN = 100,
    ATTACK,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
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
    INGAME =  1000,

    //UI
    SHOW_DAMAGE_TEXT = 1001,
    SHOW_HEAL_TEXT = 1002,
    PLAYER_HP_CHANGE = 1101,
    PLAYER_MP_CHANGE = 1102,
    ENEMY_HP_CHANGE = 1103,
    PLAYER_DEAD = 1104, //TODO : Rule로 대체하기
    BOSS_DEAD = 1105,
    ZZOL_DEAD = 1106,

    LOBBY = 2000,
}