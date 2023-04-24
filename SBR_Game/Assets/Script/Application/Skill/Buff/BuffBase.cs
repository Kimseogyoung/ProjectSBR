using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public interface IBuffAppliable
{
    void ApplyBuff(BuffBase buff);
    void ApplyTickBuff(BuffBase buff);
    void FinishBuff(BuffBase buff);
}

abstract public class BuffBase
{
    public BuffProto Proto { get; private set; }
    protected IBuffAppliable _target;
    private float _duration;
    private TimeHelper.TimeAction _timeAction;

    public BuffBase(IBuffAppliable target, BuffProto proto)
    {
        Init(target, proto);
    }

    public void Init(IBuffAppliable target, BuffProto proto)
    {
        _target = target;
        Proto = proto;
        _duration = Proto.Duration;
    }

    public void Apply()
    {
        _timeAction = TimeHelper.AddTimeEvent(_duration, () => _target.FinishBuff(this));
        ApplyBase();
    }

    public void Update()
    {
        UpdateBase();
    }

    public void Cancel()
    {
        TimeHelper.RemoveTimeEvent(_timeAction);
        _target.FinishBuff(this);
    }

    abstract protected void ApplyBase();
    virtual protected void UpdateBase() { }
}

