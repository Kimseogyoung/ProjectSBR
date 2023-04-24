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

        ApplyBuffStatToTarget(buff.Proto, true);
    }

    public void FinishBuff(BuffBase buff)
    {

        if (!_buffList.Contains(buff))
        {
            GameLogger.Error("buff 이미 종료됨.");
            return;
        }

        _buffList.Remove(buff);
        ApplyBuffStatToTarget(buff.Proto, false);
    }

    public void ApplyTickBuff(BuffBase buff)
    {
        var prtBuff = buff.Proto;

        if (prtBuff.TickHp < 0)
            _character.ApplyDamagePure(-prtBuff.TickHp);
        else if (prtBuff.TickHp > 0)
            _character.ApplyHealPure(prtBuff.TickHp);

        if (prtBuff.TickHpPer < 0)
            _character.ApplyDamagePercent(-prtBuff.TickHpPer);
        else if (prtBuff.TickHpPer > 0)
            _character.ApplyHealPercent(prtBuff.TickHpPer);

        if (prtBuff.TickMp != 0)
            _character.ApplyMpPure(prtBuff.TickMp);

        if (prtBuff.TickMpPer != 0)
            _character.ApplyMpPercent(prtBuff.TickMpPer);
    }

    public List<BuffBase> GetAllBuff()
    {
       return _buffList;
    }

    private void UpdateBuff()
    {
        for(int i=0; i< _buffList.Count; i++)
        {
            _buffList[i].Update();
        }
    }

    private void ApplyBuffStatToTarget(BuffProto prtBuff, bool start)
    {
        int mul = start ? 1 : -1;
        _character.HP.ChangePlusStat(prtBuff.HPInc * mul);
        _character.MP.ChangePlusStat(prtBuff.MPInc * mul);
        _character.SPD.ChangePlusStat(prtBuff.SPDInc * mul);
        _character.ATKSPD.ChangePlusStat(prtBuff.ATKSPDInc * mul);
        _character.ATK.ChangePlusStat(prtBuff.ATKInc * mul);
        _character.DEF.ChangePlusStat(prtBuff.DEFInc * mul);
        _character.CDR.ChangePlusStat(prtBuff.CDRInc * mul);
        _character.HPGEN.ChangePlusStat(prtBuff.HPGENInc * mul);
        _character.CRT.ChangePlusStat(prtBuff.CRTInc * mul);
        _character.RANGE.ChangePlusStat(prtBuff.RANGEInc * mul);
        _character.DRAIN.ChangePlusStat(prtBuff.DRAINInc * mul);

        _character.HP.ChangePercentStat(prtBuff.HPPer * mul);
        _character.MP.ChangePercentStat(prtBuff.MPPer * mul);
        _character.SPD.ChangePercentStat(prtBuff.SPDPer * mul);
        _character.ATKSPD.ChangePercentStat(prtBuff.ATKSPDPer * mul);
        _character.ATK.ChangePercentStat(prtBuff.ATKPer * mul);
        _character.DEF.ChangePercentStat(prtBuff.DEFPer * mul);
        _character.CDR.ChangePercentStat(prtBuff.CDRPer * mul);
        _character.HPGEN.ChangePercentStat(prtBuff.HPGENPer * mul);
        _character.CRT.ChangePercentStat(prtBuff.CRTPer * mul);
        _character.RANGE.ChangePercentStat(prtBuff.RANGEPer * mul);
        _character.DRAIN.ChangePercentStat(prtBuff.DRAINPer * mul);
    }

}

