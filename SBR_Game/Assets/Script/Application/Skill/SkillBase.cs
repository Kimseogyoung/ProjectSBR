using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract public class SkillBase
{
    public SkillProto Prt { get; private set; }
    public EInputAction MatchedInputAction { get; private set; }
    public float CurCoolTime { get; private set; }
    public float FullCoolTime { get; private set; }


    protected Vector3 _firstSkillPos;
    protected Vector3 _firstSkillDir;
    protected Character _target;
    protected HitBox _hitBox;
    protected Character _character;

    private int _currentSkillCnt;
    private bool _isPlayingSkill = false;
    private TimeHelper.TimeAction _resetTimeEvent;

    public SkillBase() { }

    public void Init(Character characterBase, int skillNum, EInputAction inputAction)
    {
        _character = characterBase;
        
        Prt = ProtoHelper.Get<SkillProto, int>(skillNum);
        MatchedInputAction = inputAction;

        if (Prt.IsNormalAttack)
        {
            Prt.StartTime = 0;
            Prt.DurationTime = 0;
        }

        FullCoolTime = Prt.CoolTime;
        //_cEventHandler = cHandler;

    }
    public bool CanUseSkill()
    {
        return CurCoolTime <= 0;
    }


    //스킬 실행 (취소될 수 있음) 취소되면 재사용 대기시간 초기화
    public void StartSkill(float coolDownValue, Character target = null)
    {
        ResetSkill();

        _target = target;

        _firstSkillPos = _character.CurPos;
        _firstSkillDir = _character.CurDir;
        
        if (!Prt.IsNormalAttack)
        {

            FullCoolTime = Prt.CoolTime * coolDownValue;
            CurCoolTime = FullCoolTime;
            _isPlayingSkill = true;
            _resetTimeEvent = TimeHelper.AddTimeEvent("skill-cool-time", FullCoolTime, ResetCoolTime);
        }
        else
        {
            FullCoolTime = coolDownValue;
            CurCoolTime = FullCoolTime;
        }
    }

    // 스킬 완전 실행
    public void UseSkill()
    {
        GameLogger.I("{0}가 {1} 시전 성공", _character.Name, Prt.Name);

        _currentSkillCnt++;
        ApplySkill();

        if (Prt.Cnt > _currentSkillCnt && Prt.DurationTime >= Prt.PeriodTime * _currentSkillCnt)
        {
            TimeHelper.AddTimeEvent("skill-period-time", Prt.PeriodTime, UseSkill);
        }

        //지속시간
        TimeHelper.AddTimeEvent("skill-duration-time", Prt.DurationTime, FinishSkill);
    }

    public void CancelSkill()
    {
        TimeHelper.RemoveTimeEvent(_resetTimeEvent);
        ResetCoolTime();
    }

    private void FinishSkill()
    {
        _isPlayingSkill = false;
    }

    private void ResetCoolTime()
    {
        _isPlayingSkill = false;
        _currentSkillCnt = 0;

        CurCoolTime = 0;
    }

    public void UpdateBase()
    {
        if (CanUseSkill())
            return;

        CurCoolTime -= Time.fixedDeltaTime;

        if (Prt.IsNormalAttack)
            return;

        if (!_isPlayingSkill)
            return;
        
        //스킬 실행중일 때만
        UpdateSkill();
    }
    abstract protected void ResetSkill();
    abstract protected void UpdateSkill();
    abstract protected void ApplySkill();




}