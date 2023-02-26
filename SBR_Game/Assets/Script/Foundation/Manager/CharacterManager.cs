using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacters
{
    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, EHitType hitType, EAttack attackPowerType, float multiply = 1);
    public List<CharacterBase> GetLivedEnemyList();
    public List<CharacterBase> GetLivedHeroList();
    public List<CharacterBase> GetEnemyList();
    public List<CharacterBase> GetHeroList();
    public List<CharacterBase> GetAllCharacterList();
    public CharacterBase GetPlayer();

}

public class CharacterManager : IManager, IManagerUpdatable, ICharacters
{
    //private List<StateMachineBase<CharacterBase>> _stateMachines = new List<StateMachineBase<CharacterBase>>();
    private List<CharacterBase> _enemyList = new List<CharacterBase>();
    private List<CharacterBase> _heroList = new List<CharacterBase>();
    private Player _player;

    public void Init()
    {
        APP.Characters = this;
        _player = (Player)Spawn(1001);
        Spawn(1010);
        Spawn(1011);
    }

    public void StartManager()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            //_enemyList[i].Set
        }
    }

    public void UpdateManager()
    {

    }

    public void FinishManager()
    {

    }

    public CharacterBase Spawn(int id)
    {
        CharacterBase character;
        GameObject characterObj;

        if (id == 1001)//플레이어 캐릭터라면 (수정)
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Hero1");
            PlayerStateMachine stateMachine = Util.GameObj.GetOrAddComponent<PlayerStateMachine>(characterObj);
            character = new Player(id);
            stateMachine.SetCharacter((Player)character, ECharacterType.Player);

            //_stateMachines.Add((StateMachineBase<CharacterBase>)stateMachine);

        }
        else
        {
            characterObj = Util.Resource.Instantiate(Path.CharacterDir + "Enemy1");
            CharacterStateMachine stateMachine = Util.GameObj.GetOrAddComponent<CharacterStateMachine>(characterObj);
            character = new CharacterBase(id);
            if(id % 10 == 0)//  보스
                stateMachine.SetCharacter(character, ECharacterType.Boss);
            else
                stateMachine.SetCharacter(character, ECharacterType.Zzol);

            //_stateMachines.Add(stateMachine);
        }

        if (id < 1010)
        {
            _heroList.Add(character);
            //플레이어 캐릭터라면
        }
        else
        {
            _enemyList.Add(character);

        }

        return character;
    }
    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, EHitType hitType, EAttack attackPowerType, float multiply)
    {
        List<CharacterBase> targetList = new List<CharacterBase>();
        List<CharacterBase> enemyList;

        if (attacker.CharacterType == ECharacterType.Boss || attacker.CharacterType == ECharacterType.Zzol)
            enemyList = APP.Characters.GetHeroList();
        else
            enemyList = APP.Characters.GetLivedEnemyList();

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (hitBox.CheckHit(attacker.CurPos, enemyList[i].CurPos))
            {
                targetList.Add(enemyList[i]);
            }
        }

        if (targetList.Count == 0) return;//적이 없음

        switch (hitType)
        {
            case EHitType.ALONE:
                CharacterBase target = targetList[0];
                float distance = (target.CurPos - attacker.CurPos).magnitude;
                for (int i = 1; i < targetList.Count; i++)
                {
                    float newTargetDistance = (targetList[i].CurPos - attacker.CurPos).magnitude;
                    if (distance > newTargetDistance)
                    {
                        target = targetList[i];
                        distance = newTargetDistance;
                    }
                }
                target.ApplyDamage(attacker.AccumulateDamage(attacker, target, attackPowerType, multiply));
                break;
            case EHitType.ALL:
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetList[i].ApplyDamage(attacker.AccumulateDamage(attacker, targetList[i], attackPowerType, multiply));
                }
                break;
            default:

                break;
        }
    }
    public CharacterBase GetPlayer() { return _player; }
    public List<CharacterBase> GetLivedHeroList() { return _heroList.FindAll((hero) => hero.IsDead() == false); }
    public List<CharacterBase> GetLivedEnemyList() { return _enemyList.FindAll((enemy) => enemy.IsDead() == false); }
    public List<CharacterBase> GetEnemyList() { return _enemyList; }
    public List<CharacterBase> GetHeroList() { return _heroList; }
    public List<CharacterBase> GetAllCharacterList() { return (List<CharacterBase>)_enemyList.Concat(_heroList); }
    //public List<StateMachineBase<CharacterBase>> GetAllStateMachine() { return _stateMachines; }
}
