using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public partial class CharacterBase 
{
    public ECharacterType CharacterType;
    public int Id;
    
    public string Name;

    public Vector3 CurDir { get; private set; } = Vector3.zero;
    public Vector3 CurPos { get; private set; } = Vector3.zero;

    public CharacterProto Proto { get; private set; }
    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    public CharacterBase(int characterId, ECharacterType type)
    {
        Id = characterId;
        Proto = ProtoHelper.Get<CharacterProto, int>(characterId);
        CharacterType = type;
        InitCharacterSetting();
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
        {//Å©¸® ÅÍÁü
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
        var skillInstance = Activator.CreateInstance(Type.GetType(ProtoHelper.Get<SkillProto, int>(skillId).ClassType));
        SkillBase skill = (SkillBase)skillInstance;
        skill.Init(this, skillId);
        _skillList.Add(action, skill);
    }
}
