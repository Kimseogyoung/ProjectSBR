using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Character
{
    //기본공격 관련 스탯
    public float AttackRangeAngle = 90;//부채꼴의 각도//TODO....사거리 증가 아이템이 있을 수 있음...

    private Dictionary<EStat, Stat> _statDict= new Dictionary<EStat, Stat>();

    public Stat HP { get { return _statDict[EStat.HP]; } }
    public Stat MP { get { return _statDict[EStat.MP]; } }
    public Stat SPD { get { return _statDict[EStat.SPD]; } }
    public Stat ATKSPD { get { return _statDict[EStat.ATKSPD]; } } //공격속도(초당 기본 공격 횟수)
    public Stat ATK { get { return _statDict[EStat.ATK]; } } //공격력
    public Stat MATK { get { return _statDict[EStat.MATK]; } }//마법 공격력
    public Stat DEF { get { return _statDict[EStat.DEF]; } }//방어력
    public Stat CRT { get { return _statDict[EStat.CRT]; } }// 크리티컬
    public Stat CDR { get { return _statDict[EStat.CDR]; } }// 쿨타임 감소 퍼센트
    public Stat HPGEN { get { return _statDict[EStat.HPGEN]; } }// 지속 체력회복
    public Stat RANGE { get { return _statDict[EStat.RANGE]; } }// 사거리
    public Stat DRAIN { get { return _statDict[EStat.DRAIN]; } }// 피흡

}
