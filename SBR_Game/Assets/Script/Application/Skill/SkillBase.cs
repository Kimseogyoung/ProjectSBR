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
    protected List<CharacterBase> _firstTargetList;
    protected HitBox _hitBox;
    protected CharacterBase _character;
    protected EAttack _attackType;

    protected SkillProto _skillProto;

    private int _currentSkillCnt;
    

    private bool _isReadySkill = true;

    private float CoolTime
    {
        get { return _skillProto.CoolTime * ((100 - _character.CDR.Value) / 100); }
    }

    public SkillBase(CharacterBase characterBase, int skillNum)
    {
        _character = characterBase;
       
        _skillProto = ProtoHelper.Get<SkillProto, int>(skillNum);

        if (_skillProto.IsNormalAttack)
        {
            _skillProto.StartTime = 0;
            _skillProto.DurationTime = 0;
        }

    }

    // 스킬 실행
    public bool TryUseSkill()
    {
        if(!_isReadySkill)
        {
            //쿨타임 안끝남
            return false;
        }
        _isReadySkill = false;

        if (_skillProto.IsNormalAttack)
            TimeHelper.AddTimeEvent(_character.ATKSPD.Value, ResetCoolTime);
        else
            TimeHelper.AddTimeEvent(CoolTime, ResetCoolTime); //TODO 쿨타임 감소 스탯 적용

        TimeHelper.AddTimeEvent(_skillProto.StartTime, StartSkill);

        return true;
    }

    public void UpdateSkill()
    {

    }

    private void StartSkill()
    {
        GameLogger.Info("{0}가 {1} 시전 성공", _character.Name, _skillProto.Name);

        UseSkill();

        TimeHelper.AddTimeEvent(_skillProto.DurationTime, FinishSkill);
    }

    private void UseSkill()
    {
        _currentSkillCnt++;
        UseImmediateSkill();

        if (_skillProto.Cnt > _currentSkillCnt && _skillProto.DurationTime >= _skillProto.PeriodTime * _currentSkillCnt)
        {
            TimeHelper.AddTimeEvent(_skillProto.PeriodTime, UseSkill);
        }

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