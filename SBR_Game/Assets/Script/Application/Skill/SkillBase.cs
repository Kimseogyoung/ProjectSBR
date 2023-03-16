using System;
using System.Collections.Generic;
using System.Linq;
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


    private float _currentDurationTime = 0;
    private float _currentPeriodTime = 0;
    private float _currentCooldownTime = 0;
    private float _currentStartTime = 0;

    private bool _isSkillRunning = false;
    private bool _isSkillApplyed = false;

    private float CoolTime
    {
        get { return _skillProto.CoolTime * ((100 - _character.CDR.Value) / 100); }
    }

    public SkillBase(CharacterBase characterBase, int skillNum)
    {
        _character = characterBase;
       
        _skillProto = ProtoHelper.Get<SkillProto, int>(skillNum);
    }

    // 스킬 실행
    public bool TryUseSkill()
    {
        if(!IsReadySkill())
        {
            //쿨타임 안끝남
            return false;
        }
        _isSkillRunning = true;

        _currentCooldownTime = CoolTime;
        _currentStartTime = _skillProto.StartTime;
        _currentDurationTime = _skillProto.DurationTime;
        return true;
    }

    public void UpdateSkill()
    {
        _currentCooldownTime -= Time.deltaTime;
        if (_isSkillRunning)
        {//스킬이 실행중일 떄

            _currentStartTime -= _skillProto.StartTime;
            if (!_isSkillApplyed && _currentStartTime <= 0.0f)
            {
                //아직 시전되지 않았을 때, 발동 아직 안함.
                _isSkillApplyed = true;
                UseImmediateSkill();
                _currentPeriodTime = _skillProto.PeriodTime;

            }

            if (_isSkillApplyed)
            {
                _currentDurationTime -= Time.deltaTime;
                _currentPeriodTime -= Time.deltaTime;

                if (IsSkillDone())
                {//지속시간이 끝났는지 확인
                    _isSkillRunning = false;
                    _isSkillApplyed = false;
                    return;
                }

                //지속 데미지 확인
                if (IsReadyPeriodTime())
                {
                    _currentPeriodTime = _skillProto.PeriodTime;
                    //적용
                    UseContinuosSkill();
                }
            }
        }
    }

    abstract protected void UseImmediateSkill();

    abstract protected void UseContinuosSkill();


    private bool IsReadySkill() 
    {
        return _currentCooldownTime <= 0.0f;
    }

    private bool IsReadyPeriodTime()
    {
        return _currentPeriodTime <= 0.0f;
    }

    private bool IsSkillDone()
    {
        return _currentDurationTime <= 0.0f;
    }



}