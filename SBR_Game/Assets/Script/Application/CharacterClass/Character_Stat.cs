using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class CharacterBase
{
    //기본공격 관련 스탯
    public float AttackRangeAngle = 90;//부채꼴의 각도//TODO....사거리 증가 아이템이 있을 수 있음...

    public Stat HP;
    public Stat MP;
    public Stat SPD;
    public Stat ATKSPD; //공격속도(초당 기본 공격 횟수)
    public Stat ATK; //공격력
    public Stat MATK;//마법 공격력
    public Stat DEF; //방어력
    public Stat CRT;// 크리티컬
    public Stat CDR;// 쿨타임 감소 퍼센트
    public Stat HPGEN;// 지속 체력회복
    public Stat RANGE;// 사거리
    public Stat DRAIN;// 피흡

}
