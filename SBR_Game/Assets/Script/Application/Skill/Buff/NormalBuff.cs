using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NormalBuff : BuffBase
{
    public NormalBuff(IBuffAppliable target, BuffProto proto) : base(target, proto)
    {
    }

    protected override void ApplyBase()
    {
        _target.ApplyBuff(this);
    }
}
