using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public interface IBuffAppliable
{
    void ApplyBuff(BuffBase buff);
    void ApplyTickBuff(BuffBase buff);
    void FinishBuff(BuffBase buff);

    public List<BuffBase> GetBuffList();
}

public class BuffBase
{
    public BuffProto Proto { get; private set; }

    protected IBuffAppliable _target;

    private float _duration;
    private TimeHelper.TimeAction _timeAction;
    private float _tickTime;
    private static readonly string _durationTimeEventName = "buff-duration";
    public BuffBase(IBuffAppliable target, BuffProto proto)
    {
        Init(target, proto);
    }

    private void Init(IBuffAppliable target, BuffProto proto)
    {
        _target = target;
        Proto = proto;
        _duration = Proto.Duration;
    }

    public void Apply()
    {
        GameLogger.I($"{Proto.Name} 버프 적용.");
        _timeAction = TimeHelper.AddTimeEvent(_durationTimeEventName, _duration, () => { Cancel(); });

        _tickTime = 0;
        _target.ApplyBuff(this);
    }

    public void Update()
    {
        _tickTime -= Time.fixedDeltaTime;
        if (_tickTime < 0)
        {
            _tickTime = Proto.TickInterval;
            _target.ApplyTickBuff(this);
            GameLogger.I($"{Proto.Name} 버프 틱 효과 적용.  다음 틱 적용시간 {_tickTime}");
        }
    }

    public void Cancel()
    {
        TimeHelper.RemoveTimeEvent(_timeAction);
        _target.FinishBuff(this);
    }

    public float GetCurDuration()
    {
        if(_timeAction == null)
        {
            GameLogger.E("TimeAction Null");
            return 0;
        }
        return TimeHelper.GetTimeEventRemainingTime(_timeAction);
    }

    public float GetDuration()
    {
        return _duration;
    }

    virtual protected void UpdateBase() { } // LEGACY
}

