using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract public class SkillBase
{
    public SkillProto _skillProto { get; private set; }

    protected Vector3 _firstSkillPos;
    protected Vector3 _firstSkillDir;
    protected CharacterBase _target;
    protected HitBox _hitBox;
    protected CharacterBase _character;
    protected EAttack _attackType;
    private int _currentSkillCnt;
    
    private bool _isPlayingSkill = false;

    private TimeHelper.TimeAction _resetTimeEvent;

    public float CurCoolTime = 0;

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
        return CurCoolTime <= 0;
    }


    //스킬 실행 (취소될 수 있음) 취소되면 재사용 대기시간 초기화
    public void StartSkill(CharacterBase target = null)
    {
        ResetSkill();

        _target = target;

        _firstSkillPos = _character.CurPos;
        _firstSkillDir = _character.CurDir;

        if (!_skillProto.IsNormalAttack)
        {
            _isPlayingSkill = true;
            _resetTimeEvent = TimeHelper.AddTimeEvent(CoolTime, ResetCoolTime); //TODO 쿨타임 감소 스탯 적용
        }

        CurCoolTime = CoolTime;
    }

    // 스킬 완전 실행
    public void UseSkill()
    {
        GameLogger.Info("{0}가 {1} 시전 성공", _character.Name, _skillProto.Name);

        _currentSkillCnt++;
        ApplySkill();

        if (_skillProto.Cnt > _currentSkillCnt && _skillProto.DurationTime >= _skillProto.PeriodTime * _currentSkillCnt)
        {
            TimeHelper.AddTimeEvent(_skillProto.PeriodTime, UseSkill);
        }

        //지속시간
        TimeHelper.AddTimeEvent(_skillProto.DurationTime, FinishSkill);
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
        if (_skillProto.IsNormalAttack)
            return;

        if (CanUseSkill())
            return;

        CurCoolTime -= Time.fixedDeltaTime;

        if (!_isPlayingSkill)
            return;
        
        //스킬 실행중일 때만
        UpdateSkill();
    }
    abstract protected void ResetSkill();
    abstract protected void UpdateSkill();
    abstract protected void ApplySkill();




}