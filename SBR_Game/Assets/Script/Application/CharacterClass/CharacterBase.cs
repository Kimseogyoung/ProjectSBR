using System;
using System.Collections;
using System.Collections.Generic;
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

    public float AccumulateDamage(CharacterBase attacker, CharacterBase victim, EAttack atk ,float multiplier =1f)//공격자, 피격자, 공격력 종류, 데미지 계수
    {
        float damage = atk == EAttack.ATK ? attacker.ATK.Value : attacker.MATK.Value;
        damage = attacker.CheckCritical() ? damage * 2 : damage;//크리티컬 적용

        damage *= (100 - victim.DEF.Value) / 100f;//피격자 방어력 적용 (ex 1% 감소)

        return damage;

    }

    public float ApplyDamage(float damage)
    {
        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            //죽음
            GameLogger.Strong("{0}는 죽었다.", Name);
        }

        
        EventQueue.PushEvent<HPEvent>(
            CharacterType == ECharacterType.PLAYER? EEventActionType.PLAYER_HP_CHANGE:
            CharacterType == ECharacterType.BOSS? EEventActionType.BOSS_HP_CHANGE : EEventActionType.ZZOL_HP_CHANGE,
            new HPEvent(Id, HP.FullValue, HP.Value, true));

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

        
        _skillList.Add(EInputAction.ATTACK, new NormalAttackSkill(this, charProto.AttackSkill));
        _skillList.Add(EInputAction.SKILL1, new Skill0(this, charProto.Skill1));
        _skillList.Add(EInputAction.SKILL2, new Skill0(this, charProto.Skill2));
        _skillList.Add(EInputAction.SKILL3, new Skill0(this, charProto.Skill3));
        _skillList.Add(EInputAction.ULT_SKILL, new Skill0(this, charProto.UltSkill));

    }
}
