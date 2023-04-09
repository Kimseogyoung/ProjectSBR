using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class StateMachineBase: IBuffAppliable
{
    private List<BuffBase> _buffList = new List<BuffBase>();
    public void ApplyBuff(BuffBase buff)
    {
        _buffList.Add(buff);
    }

    public void FinishBuff(BuffBase buff)
    {

        if (!_buffList.Contains(buff))
        {
            GameLogger.Error("buff 이미 종료됨.");
            return;
        }

        _buffList.Remove(buff);
    }

    public List<BuffBase> GetAllBuff()
    {
       return _buffList;
    }
}

