using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TickBuff : BuffBase
{
    private float _tickTime;

    public TickBuff(IBuffAppliable target, BuffProto proto) : base(target, proto)
    {
    }

    protected override void ApplyBase()
    {
        _tickTime = 0;
        _target.ApplyBuff(this);
    }

    protected override void UpdateBase()
    {

        _tickTime += Time.fixedTime;
        if (_tickTime >= Proto.TickInterval)
        {
            _tickTime = 0;
            _target.ApplyTickBuff(this);
        }
        
    }

}

