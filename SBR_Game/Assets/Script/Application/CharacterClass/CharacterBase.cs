using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[Serializable]
public partial class Character : ClassBase, IBuffAppliable
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

    private List<ItemProto> _itemPrtList = new List<ItemProto>(); // TODO: 테스트용. 작동 확인 필요 (+포션류 분리?)
    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    private bool _initialize = false;
    public static bool Create(out Character character, int characterId, ECharacterType type, int createNum, List<int> itemNumList)
    {
        character = new Character();
        character.Id = characterId;
        character.CharacterType = type;
        character.CreateNum = createNum;

        for(int i=0; i< itemNumList.Count; i++)
        {
            if(!ProtoHelper.TryGet<ItemProto>(itemNumList[i], out var prt))
            {
                LOG.E($"Not Found Item Proto Num({itemNumList[i]})");
                return false;
            }
            character._itemPrtList.Add(prt);
        }

        if (!character.OnCreate())
        {
            Destroy(ref character);
            return false;
        }

        return true;
    }

    protected override bool OnCreate()
    {
        if (_initialize)
            return false;

        Proto = ProtoHelper.Get<CharacterProto>(Id);
        InitCharacterSetting();
        RefreshGetItemStat();
        SetStatFull();
        _initialize = true;
        return true;
    }

    protected override void OnDestroy()
    {
       
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
        Name = this.Proto.Name;
        _statDict.Add(EStat.HP, new Stat(EStat.HP, Proto.HP));
        _statDict.Add(EStat.MP, new Stat(EStat.MP, Proto.MP));
        _statDict.Add(EStat.SPD, new Stat(EStat.SPD, Proto.SPD));
        _statDict.Add(EStat.ATKSPD, new Stat(EStat.ATKSPD, Proto.ATKSPD));
        _statDict.Add(EStat.ATK, new Stat(EStat.ATK, Proto.ATK));
        _statDict.Add(EStat.MATK, new Stat(EStat.MATK, Proto.MATK));
        _statDict.Add(EStat.DEF, new Stat(EStat.DEF, Proto.DEF));
        _statDict.Add(EStat.CRT, new Stat(EStat.CRT, Proto.CRT));
        _statDict.Add(EStat.CDR, new Stat(EStat.CDR, Proto.CDR));
        _statDict.Add(EStat.DRAIN, new Stat(EStat.DRAIN, Proto.DRAIN));
        _statDict.Add(EStat.RANGE, new Stat(EStat.RANGE, Proto.RANGE));
        _statDict.Add(EStat.HPGEN, new Stat(EStat.HPGEN, Proto.HPGEN));


        AddSkill(EInputAction.ATTACK, Proto.AttackSkill);
        AddSkill(EInputAction.SKILL1, Proto.Skill1);
        AddSkill(EInputAction.SKILL2, Proto.Skill2);
        AddSkill(EInputAction.SKILL3, Proto.Skill3);
        AddSkill(EInputAction.SKILL4, Proto.Skill4);
        AddSkill(EInputAction.ULT_SKILL, Proto.UltSkill);
    }
    
    private void AddSkill(EInputAction action, int skillId)
    {
        var prtSkill = ProtoHelper.Get<SkillProto>(skillId);
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

    // 두번호출 X
    private List<Stat> RefreshGetItemStat()
    {
        foreach(var prt in _itemPrtList)
        {
            HP.ChangePlusStat(prt.HP);
            MP.ChangePlusStat(prt.MP);
            SPD.ChangePlusStat(prt.SPD);
            ATKSPD.ChangePlusStat(prt.ATK);
            ATK.ChangePlusStat(prt.ATK);
            DEF.ChangePlusStat(prt.DEF);
            CDR.ChangePlusStat(prt.CDR);
            HPGEN.ChangePlusStat(prt.HPGEN);
            CRT.ChangePlusStat(prt.CRT);
            RANGE.ChangePlusStat(prt.RANGE);
            DRAIN.ChangePlusStat(prt.DRAIN);

            HP.ChangePercentStat(prt.HPPer);
            MP.ChangePercentStat(prt.MPPer);
            SPD.ChangePercentStat(prt.SPDPer);
            ATKSPD.ChangePercentStat(prt.ATKSPDPer);
            ATK.ChangePercentStat(prt.ATKPer);
            DEF.ChangePercentStat(prt.DEFPer);
            CDR.ChangePercentStat(prt.CDRPer);
            HPGEN.ChangePercentStat(prt.HPGENPer);
            CRT.ChangePercentStat(prt.CRTPer);
            RANGE.ChangePercentStat(prt.RANGEPer);
            DRAIN.ChangePercentStat(prt.DRAINPer);
        }

        return _statDict.Values.ToList();
    }

    public List<BuffBase> GetBuffList()
    {
        return BuffList;
    }
}
