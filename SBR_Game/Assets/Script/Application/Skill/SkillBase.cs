using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract public class SkillBase
{
    protected Vector3 _firstSkillPos;
    protected CharacterBase _target;
    protected HitBox _hitBox;
    protected CharacterBase _character;
    protected EAttack _attackType;

    public SkillProto _skillProto { get; private set; }
    private int _currentSkillCnt;
    

    private bool _isReadySkill = true;

    private float CoolTime
    {
        get { return _skillProto.CoolTime * ((100 - _character.CDR.Value) / 100); }
    }

    public SkillBase() { }

    public void Init(CharacterBase characterBase, int skillNum)
    {
        _character = characterBase;
        _skillProto = ProtoHelper.Get<SkillProto, int>(skillNum);

        if (_skillProto.IsNormalAttack)
        {
            _skillProto.StartTime = 0;
            _skillProto.DurationTime = 0;
        }
        //_cEventHandler = cHandler;

    }
    public bool CanUseSkill()
    {
        return _isReadySkill;
    }


    //스킬 실행 (취소될 수 있음) 취소되면 재사용 대기시간 초기화
    public void StartSkill(CharacterBase target = null)
    {
        _isReadySkill = false;

        _target = target;

        if (_skillProto.IsNormalAttack)
            TimeHelper.AddTimeEvent(_character.ATKSPD.Value, ResetCoolTime);//공격속도는 1초당 공격할 수 있는 횟수임.
        else
            TimeHelper.AddTimeEvent(CoolTime, ResetCoolTime); //TODO 쿨타임 감소 스탯 적용

    }

    // 스킬 완전 실행
    public void UseSkill()
    {

        GameLogger.Info("{0}가 {1} 시전 성공", _character.Name, _skillProto.Name);

        _currentSkillCnt++;
        UseImmediateSkill();

        if (_skillProto.Cnt > _currentSkillCnt && _skillProto.DurationTime >= _skillProto.PeriodTime * _currentSkillCnt)
        {
            TimeHelper.AddTimeEvent(_skillProto.PeriodTime, UseSkill);
        }

        //지속시간
        TimeHelper.AddTimeEvent(_skillProto.DurationTime, FinishSkill);

    }

    public void CancelSkill()
    {

    }

    public void UpdateSkill()
    {

    }

    private void FinishSkill()
    {
        
    }

    private void ResetCoolTime()
    {
        _isReadySkill = true;
        _currentSkillCnt = 0;
    }

    abstract protected void UseImmediateSkill();




}