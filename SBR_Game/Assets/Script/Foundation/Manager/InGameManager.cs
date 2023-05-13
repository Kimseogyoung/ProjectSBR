using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ICharacters
{
    public float GetGameTime();
    public void FindTarget(CharacterBase attacker, HitBox hitBox, ECharacterTeamType targetTeamType,
        EHitSKillType hitType, EHitTargetSelectType hitTargetSelectType, int targetCnt);
    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, ECharacterTeamType targetTeamType, 
        EHitSKillType hitType, EHitTargetSelectType hitTargetSelectType, int targetCnt, float multiply = 1);
    public List<CharacterBase> GetLivedEnemyList();
    public List<CharacterBase> GetLivedHeroList();
    public List<CharacterBase> GetEnemyList();
    public List<CharacterBase> GetHeroList();
    public List<CharacterBase> GetAllCharacterList();
    public CharacterBase GetBoss();
    public CharacterBase GetPlayer();

}

//Manager재 정비 필요.  현재 문제 - Init하기전  Update됨
public class InGameManager : IManager, IManagerUpdatable, ICharacters
{
    public Action<CharacterBase> OnCreateCharacter { get; set; }
    public Action<int> OnDieCharacter { private get; set; }

    public static SkillSystem Skill { get { Debug.Assert(_skillSystem != null); return _skillSystem; } }
    private static SkillSystem _skillSystem;
    private DamageTextSystem _damageTextSystem;
    private float _gameTime;

    private List<StateMachineBase> _stateMachines = new List<StateMachineBase>();
    private List<CharacterBase> _enemyList = new List<CharacterBase>();
    private List<CharacterBase> _heroList = new List<CharacterBase>();
    private Player _player;
    private CharacterBase _boss;

    private Vector2 _minimumMapPos;
    private Vector2 _maximumMapPos;
    private int createNum = 1;

    public void Init()
    {
        createNum = 1;

        _skillSystem = new SkillSystem();
        _damageTextSystem = new DamageTextSystem();

        APP.InGame = this;

        _minimumMapPos = new Vector2(-APP.CurrentStage.Width / 2, -APP.CurrentStage.Height / 2);
        _maximumMapPos = new Vector2(APP.CurrentStage.Width / 2, APP.CurrentStage.Height / 2);

        _player = (Player)Spawn(1, true);
        _boss = Spawn(1010);
        Spawn(1011);
    }


    public void StartManager()
    {
        for (int i = 0; i < _stateMachines.Count; i++)
        {
            _stateMachines[i].SetState(new IdleState());
            _stateMachines[i].PlayStartAnim();          
        }

        // 3초 후 시작
        TimeHelper.AddTimeEvent(3.0f, () =>
        {
            
            _gameTime = 0;
            for (int i = 0; i < _stateMachines.Count; i++)
            {
                _stateMachines[i].SetIdle();
                if (_stateMachines[i] is PlayerStateMachine)
                {
                    _stateMachines[i].SetState(new playerNormalState());
                }
                else if (_stateMachines[i] is CharacterStateMachine)
                {
                    _stateMachines[i].SetState(new EnemyFollowState());
                }
            }
        });   
    }

    public void UpdateManager()
    {
        _gameTime += Time.fixedDeltaTime;
        for (int i=0; i<_stateMachines.Count; i++)
        {
            _stateMachines[i].UpdateStateMachine();
        }
        _damageTextSystem.Update();
        Skill.UpdateSkill();
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
        _damageTextSystem.Destroy();
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
            character = new Player(id, createNum++);
        }
        else
        {
            stateMachine = Util.GameObj.GetOrAddComponent<CharacterStateMachine>(characterObj);
            character = new CharacterBase(id, characterType, createNum++);
        }

        stateMachine.Initialize(character, characterType, _minimumMapPos, _maximumMapPos);
        _stateMachines.Add(stateMachine);

        if (characterProto.TeamType == ECharacterTeamType.HERO)
        {
            characterObj.layer = LayerMask.NameToLayer("Hero");
            _heroList.Add(character);
            character.SetPos(new Vector3(2, 0, 0));
        }
        else
        {
            characterObj.layer = LayerMask.NameToLayer("Enemy");
            _enemyList.Add(character);
        }

        // 생성 이벤트, 죽음 이벤트
        OnCreateCharacter.Invoke(character);
        character.OnDieCharacter = (character) => { OnDieCharacter.Invoke(character.CreateNum); };

        return character;
    }
    public void FindTarget(CharacterBase attacker, HitBox hitBox, ECharacterTeamType targetTeamType, EHitSKillType hitType, EHitTargetSelectType hitTargetSelectType, int targetCnt)
    {
        throw new NotImplementedException();
    }

    public void FindTargetAndApplyDamage(CharacterBase attacker, HitBox hitBox, ECharacterTeamType targetTeamType,
        EHitSKillType hitType, EHitTargetSelectType hitTargetSelectType, int targetCnt, float multiply)
    {
        List<CharacterBase> targetList = new List<CharacterBase>();
        List<CharacterBase> enemyList;

        if (attacker.CharacterType == ECharacterType.BOSS || attacker.CharacterType == ECharacterType.ZZOL)
            enemyList = APP.InGame.GetHeroList();
        else
            enemyList = APP.InGame.GetLivedEnemyList();

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (hitBox.CheckHit(attacker.CurPos, enemyList[i].CurPos))
            {
                targetList.Add(enemyList[i]);
            }
        }

        if (targetList.Count == 0) return;//적이 없음

        //공격할 타겟 수가 영역 내 타겟 수보다 많으면 전부 공격처리
        if(targetCnt >= targetList.Count || EHitTargetSelectType.ALL == hitTargetSelectType)
        {
            for(int i=0; i< targetList.Count; i++)
            {
                targetList[i].ApplyDamageWithMuliply(attacker, multiply);
            }
            return;
        }

        switch (hitTargetSelectType)
        {
            case EHitTargetSelectType.CLOSE:
                List<CharacterDistance> distanceList = new List<CharacterDistance>();
                for (int i = 1; i < targetList.Count; i++)
                {
                    distanceList.Add(new CharacterDistance { 
                        Character = targetList[i],
                        Distance = (targetList[i].CurPos - attacker.CurPos).magnitude
                    });
                    
                }
                distanceList = distanceList.OrderBy(x => x.Distance).ToList();
                for(int i=0; i<targetCnt; i++)
                {
                    distanceList[i].Character.ApplyDamageWithMuliply(attacker, multiply);
                }
                break;
            case EHitTargetSelectType.DIR:
                List<CharacterDistance> distanceAngleList = new List<CharacterDistance>();
                for (int i = 1; i < targetList.Count; i++)
                {
                    distanceAngleList.Add(new CharacterDistance
                    {
                        Character = targetList[i],
                        Distance = (targetList[i].CurPos - attacker.CurPos).magnitude,
                        Angle = Mathf.Atan2(attacker.CurPos.y, targetList[i].CurPos.x) * Mathf.Rad2Deg
                    });

                }
                distanceAngleList = distanceAngleList.OrderBy(x => x.Angle).ThenBy(x => x.Distance).ToList();
                for (int i = 0; i < targetCnt; i++)
                {
                    distanceAngleList[i].Character.ApplyDamageWithMuliply(attacker, multiply);
                }
                break;
            case EHitTargetSelectType.RANDOM:
                System.Random random = new System.Random(DateTime.Now.Millisecond);
                
                for(int i=0; i<targetCnt; i++)
                {
                    int targetIdx = random.Next(targetList.Count - 1);
                    targetList[targetIdx].ApplyDamageWithMuliply(attacker, multiply);
                    targetList.RemoveAt(targetIdx);
                }

                break;
            default:
                GameLogger.Error("WRONG {0}", hitTargetSelectType);
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

    public float GetGameTime() { return _gameTime; }
    public CharacterBase GetPlayer() { return _player; }
    public CharacterBase GetBoss() { return _boss; }
    public List<CharacterBase> GetLivedHeroList() { return _heroList.FindAll((hero) => hero.IsDead() == false); }
    public List<CharacterBase> GetLivedEnemyList() { return _enemyList.FindAll((enemy) => enemy.IsDead() == false); }
    public List<CharacterBase> GetEnemyList() { return _enemyList; }
    public List<CharacterBase> GetHeroList() { return _heroList; }
    public List<CharacterBase> GetAllCharacterList() { return (List<CharacterBase>)_enemyList.Concat(_heroList); }

    //public List<StateMachineBase<CharacterBase>> GetAllStateMachine() { return _stateMachines; }
}

class CharacterDistance{
    public float Angle;
    public float Distance;
    public CharacterBase Character;
}