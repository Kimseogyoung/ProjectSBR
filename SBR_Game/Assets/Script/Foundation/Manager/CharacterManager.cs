using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacters
{
    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, EHitTargetType hitType, EAttack attackPowerType, float multiply = 1);
    public List<CharacterBase> GetLivedEnemyList();
    public List<CharacterBase> GetLivedHeroList();
    public List<CharacterBase> GetEnemyList();
    public List<CharacterBase> GetHeroList();
    public List<CharacterBase> GetAllCharacterList();
    public CharacterBase GetPlayer();

}

public class CharacterManager : IManager, IManagerUpdatable, ICharacters
{
    private List<StateMachineBase> _stateMachines = new List<StateMachineBase>();
    private List<CharacterBase> _enemyList = new List<CharacterBase>();
    private List<CharacterBase> _heroList = new List<CharacterBase>();
    private Player _player;

    private Vector2 _minimumMapPos;
    private Vector2 _maximumMapPos;
    public void Init()
    {
        APP.Characters = this;

        _minimumMapPos = new Vector2(-APP.CurrentStage.Width / 2, -APP.CurrentStage.Height / 2);
        _maximumMapPos = new Vector2(APP.CurrentStage.Width / 2, APP.CurrentStage.Height / 2);

        _player = (Player)Spawn(1, true);
        Spawn(1010);
        Spawn(1011);
    }


    public void StartManager()
    {
        for (int i = 0; i < _stateMachines.Count; i++)
        {
            if (_stateMachines[i] is PlayerStateMachine)
            {
                _stateMachines[i].SetState(new playerNormalState());
            }
            else if(_stateMachines[i] is CharacterStateMachine )
            {
                _stateMachines[i].SetState(new EnemyFollowState());
            }
        }
    }

    public void UpdateManager()
    {
        for (int i=0; i<_stateMachines.Count; i++)
        {
            _stateMachines[i].UpdateStateMachine();
        }
    }
    public void UpdatePausedManager()
    {
       
    }

    public void Pause(bool IsPause)
    {

    }

    public void FinishManager()
    {
        for (int i = 0; i < _stateMachines.Count; i++)
        {
            _stateMachines[i].SetState(new IdleState());
        }
        _stateMachines.Clear();
        _heroList.Clear();
        _enemyList.Clear();
    }

    public CharacterBase Spawn(int id, bool isPlayer = false)
    {
        CharacterBase character;
        GameObject characterObj;
        StateMachineBase stateMachine;
        CharacterProto characterProto = ProtoHelper.Get<CharacterProto, int>(id);

        characterObj = Util.Resource.Instantiate(characterProto.Prefab);

        ECharacterType characterType = isPlayer ? ECharacterType.PLAYER : characterProto.Type;
        if (!CanSpawnCharacter(characterType))
        {
            GameLogger.Info($"{characterType} 과 동일한 타입의 캐릭터가 이미 존재하여 Spawn 실패");
            return null;
        }

        if (isPlayer)
        {
            stateMachine = Util.GameObj.GetOrAddComponent<PlayerStateMachine>(characterObj);
            character = new Player(id);
        }
        else
        {
            stateMachine = Util.GameObj.GetOrAddComponent<CharacterStateMachine>(characterObj);
            character = new CharacterBase(id);
        }

        stateMachine.SetCharacter(character,characterType, _minimumMapPos, _maximumMapPos);
        _stateMachines.Add(stateMachine);

        if (characterProto.TeamType == ECharacterTeamType.HERO)
        {
            _heroList.Add(character);
        }
        else
        {
            _enemyList.Add(character);
        }

        return character;
    }
    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, EHitTargetType hitType, EAttack attackPowerType, float multiply)
    {
        List<CharacterBase> targetList = new List<CharacterBase>();
        List<CharacterBase> enemyList;

        if (attacker.CharacterType == ECharacterType.BOSS || attacker.CharacterType == ECharacterType.ZZOL)
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
            case EHitTargetType.ALONE:
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
            case EHitTargetType.ALL:
                for (int i = 0; i < targetList.Count; i++)
                {
                    targetList[i].ApplyDamage(attacker.AccumulateDamage(attacker, targetList[i], attackPowerType, multiply));
                }
                break;
            default:

                break;
        }
    }

    private bool CanSpawnCharacter(ECharacterType characterType)
    {
        if (characterType == ECharacterType.BOSS)
        {
            for (int i = 0; i < _enemyList.Count; i++)
            {
                if (_enemyList[i].CharacterType == ECharacterType.BOSS)
                    return false;
            }
        }
        else if(characterType == ECharacterType.PLAYER)
        {
            for (int i = 0; i < _heroList.Count; i++)
            {
                if (_heroList[i].CharacterType == ECharacterType.PLAYER)
                    return false;
            }

        }
        return true;
    }

    public CharacterBase GetPlayer() { return _player; }
    public List<CharacterBase> GetLivedHeroList() { return _heroList.FindAll((hero) => hero.IsDead() == false); }
    public List<CharacterBase> GetLivedEnemyList() { return _enemyList.FindAll((enemy) => enemy.IsDead() == false); }
    public List<CharacterBase> GetEnemyList() { return _enemyList; }
    public List<CharacterBase> GetHeroList() { return _heroList; }
    public List<CharacterBase> GetAllCharacterList() { return (List<CharacterBase>)_enemyList.Concat(_heroList); }
    //public List<StateMachineBase<CharacterBase>> GetAllStateMachine() { return _stateMachines; }
}
