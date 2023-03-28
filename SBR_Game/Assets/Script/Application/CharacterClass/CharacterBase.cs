using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public partial class CharacterBase 
{
    public ECharacterType CharacterType;
    public int Id;
    
    public string Name;

    public Vector3 CurDir = Vector3.zero;
    public Vector3 CurPos = Vector3.zero;


    private Dictionary<EInputAction, SkillBase> _skillList = new Dictionary<EInputAction,SkillBase>();

    public CharacterBase(int characterId, ECharacterType characterType)
    {
        Id = characterId;
        CharacterType= characterType;
        InitCharacterSetting();
    }

    private float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//������, �ǰ���, ���ݷ� ����, ������ ���
    {
        float damage = atk == EAttack.ATK ? attacker.ATK.Value : attacker.MATK.Value;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//ũ��Ƽ�� ����

        damage *= (100 - victim.DEF.Value) / 100f;//�ǰ��� ���� ���� (ex 1% ����)

        return damage;

    }

    public float ApplyDamage(CharacterBase attacker, EAttack attackType, float multiply)
    {
        float damage = AccumulateDamage(attacker, this, attackType, multiply);

        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            //����
            GameLogger.Strong("{0}�� �׾���.", Name);
        }

        
        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.PLAYER? EEventActionType.PLAYER_HP_CHANGE: EEventActionType.ENEMY_HP_CHANGE,
            new HPEvent(Id, damage, HP.FullValue, HP.Value, true, attacker));

        return damage;
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
        {//ũ�� ����
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
        AddSkill(EInputAction.DODGE, charProto.DodgeSkill);
        AddSkill(EInputAction.SKILL1, charProto.Skill1);
        AddSkill(EInputAction.SKILL2, charProto.Skill2);
        AddSkill(EInputAction.SKILL3, charProto.Skill3);
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
