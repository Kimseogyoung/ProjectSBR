using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public interface IBuffAppliable
{
    void ApplyBuff(BuffBase buff);
    void ApplyTickBuff(BuffBase buff);
    void FinishBuff(BuffBase buff);
}

public class BuffBase
{
    public BuffProto Proto { get; private set; }

    protected IBuffAppliable _target;

    private float _duration;
    private TimeHelper.TimeAction _timeAction;
    private float _tickTime;

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
        GameLogger.Info($"{Proto.Name} 버프 적용.");
        _timeAction = TimeHelper.AddTimeEvent(_duration, () => _target.FinishBuff(this));

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
            GameLogger.Info($"{Proto.Name} 버프 틱 효과 적용.  다음 틱 적용시간 {_tickTime}");
        }
    }

    public void Cancel()
    {
        TimeHelper.RemoveTimeEvent(_timeAction);
        _target.FinishBuff(this);
    }

    virtual protected void UpdateBase() { } // LEGACY
}

