using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[Serializable]
public partial class Character : IBuffAppliable
{
    public Action<BuffBase> OnAddBuff { get; set; }


    public ECharacterType CharacterType { get; private set; }
    public int Id { get; private set; }
    public int CreateNum { get; private set; }
    public string Name { get; private set; }

    public List<BuffBase> BuffList { get; private set; } =  new List<BuffBase>();
    public Vector3 CurDir { get; private set; } = Vector3.zero;
    public Vector3 CurPos { get; private set; } = Vector3.zero;

    public CharacterProto Proto { get; private set; }
    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();


    public Character(int characterId, ECharacterType type, int createNum)
    {
        Id = characterId;
        Proto = ProtoHelper.Get<CharacterProto, int>(characterId);
        CharacterType = type;
        InitCharacterSetting();
        CreateNum = createNum;
    }

    public void TranslatePos(Vector3 pos)
    {
        CurPos += pos; 
    }

    public void SetPos(Vector3 pos)
    {
        CurPos = pos;
    }

    public void TranslateDir(Vector3 dir)
    {
        CurPos += dir;
    }

    public void SetDir(Vector3 dir)
    {
        CurDir = dir;
    }

    public bool IsDead()
    {
        return HP.Value <= 0 ? true : false;
    }

    public SkillBase GetSkill(EInputAction inputAction) { return _skillList[inputAction]; }

    private bool CheckCritical()
    {
        System.Random rand = new System.Random(DateTime.Now.Millisecond);
        if(rand.Next(100) <= CRT.Value)
        {//크리 터짐
            return true;
        }
        return false;
    }

    private void InitCharacterSetting()
    {
        CharacterProto charProto = ProtoHelper.Get<CharacterProto, int>(Id);
        Name = charProto.Name;
        HP = new Stat(EStat.HP, charProto.HP);
        MP = new Stat(EStat.MP, charProto.MP);
        SPD = new Stat(EStat.SPD, charProto.SPD);
        ATKSPD = new Stat(EStat.ATKSPD, charProto.ATKSPD);
        ATK = new Stat(EStat.ATK, charProto.ATK);
        MATK = new Stat(EStat.MATK, charProto.MATK);
        DEF = new Stat(EStat.DEF, charProto.DEF);
        CRT = new Stat(EStat.CRT, charProto.CRT);
        CDR = new Stat(EStat.CDR, charProto.CDR);
        DRAIN = new Stat(EStat.DRAIN, charProto.DRAIN);
        RANGE = new Stat(EStat.RANGE, charProto.CDR);
        HPGEN = new Stat(EStat.HPGEN, charProto.HPGEN);

        AddSkill(EInputAction.ATTACK, charProto.AttackSkill);
        AddSkill(EInputAction.SKILL1, charProto.Skill1);
        AddSkill(EInputAction.SKILL2, charProto.Skill2);
        AddSkill(EInputAction.SKILL3, charProto.Skill3);
        AddSkill(EInputAction.SKILL4, charProto.Skill4);
        AddSkill(EInputAction.ULT_SKILL, charProto.UltSkill);
    }
    
    private void AddSkill(EInputAction action, int skillId)
    {
        var prtSkill = ProtoHelper.Get<SkillProto, int>(skillId);
        LOG.I($"{Name} 캐릭터에게 스킬 {prtSkill.Name} 등록. Action({action.ToString()})");

        var skillInstance = Activator.CreateInstance(Type.GetType(prtSkill.ClassType));

        SkillBase skill = (SkillBase)skillInstance;
        skill.Init(this, skillId, action);
        _skillList.Add(action, skill);
    }

    public void ApplyBuff(BuffBase buff)
    {
        BuffList.Add(buff);
        ApplyBuffStatToTarget(buff.Proto, true);
        OnAddBuff?.Invoke(buff);
    }

    public void FinishBuff(BuffBase buff)
    {

        if (!BuffList.Contains(buff))
        {
            LOG.E("buff 이미 종료됨.");
            return;
        }

        BuffList.Remove(buff);
        ApplyBuffStatToTarget(buff.Proto, false);
    }

    public void ApplyTickBuff(BuffBase buff)
    {
        var prtBuff = buff.Proto;

        if (prtBuff.TickHp < 0)
            ApplyDamagePure(-prtBuff.TickHp);
        else if (prtBuff.TickHp > 0)
            ApplyHealPure(prtBuff.TickHp);

        if (prtBuff.TickHpPer < 0)
            ApplyDamagePercent(-prtBuff.TickHpPer);
        else if (prtBuff.TickHpPer > 0)
            ApplyHealPercent(prtBuff.TickHpPer);

        if (prtBuff.TickMp != 0)
            ApplyMpPure(prtBuff.TickMp);

        if (prtBuff.TickMpPer != 0)
            ApplyMpPercent(prtBuff.TickMpPer);
    }

    private void ApplyBuffStatToTarget(BuffProto prtBuff, bool start)
    {
        int mul = start ? 1 : -1;
        HP.ChangePlusStat(prtBuff.HPInc * mul);
        MP.ChangePlusStat(prtBuff.MPInc * mul);
        SPD.ChangePlusStat(prtBuff.SPDInc * mul);
        ATKSPD.ChangePlusStat(prtBuff.ATKSPDInc * mul);
        ATK.ChangePlusStat(prtBuff.ATKInc * mul);
        DEF.ChangePlusStat(prtBuff.DEFInc * mul);
        CDR.ChangePlusStat(prtBuff.CDRInc * mul);
        HPGEN.ChangePlusStat(prtBuff.HPGENInc * mul);
        CRT.ChangePlusStat(prtBuff.CRTInc * mul);
        RANGE.ChangePlusStat(prtBuff.RANGEInc * mul);
        DRAIN.ChangePlusStat(prtBuff.DRAINInc * mul);

        HP.ChangePercentStat(prtBuff.HPPer * mul);
        MP.ChangePercentStat(prtBuff.MPPer * mul);
        SPD.ChangePercentStat(prtBuff.SPDPer * mul);
        ATKSPD.ChangePercentStat(prtBuff.ATKSPDPer * mul);
        ATK.ChangePercentStat(prtBuff.ATKPer * mul);
        DEF.ChangePercentStat(prtBuff.DEFPer * mul);
        CDR.ChangePercentStat(prtBuff.CDRPer * mul);
        HPGEN.ChangePercentStat(prtBuff.HPGENPer * mul);
        CRT.ChangePercentStat(prtBuff.CRTPer * mul);
        RANGE.ChangePercentStat(prtBuff.RANGEPer * mul);
        DRAIN.ChangePercentStat(prtBuff.DRAINPer * mul);
    }

    public List<BuffBase> GetBuffList()
    {
        return BuffList;
    }
}
