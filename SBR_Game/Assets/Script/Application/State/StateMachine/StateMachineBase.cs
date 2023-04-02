using Newtonsoft.Json.Bson;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using static TimeHelper;


public class StateMachineBase : MonoBehaviour 
{
    public CharacterBase _currentTarget = null;
    [SerializeField] protected CharacterBase _character;

    private CharacterState<CharacterBase> _currentState;
    [SerializeField] private string _currentStateName;

    private Transform _transform;//현재 캐릭터 위치
    private float _currentAtkCoolTime = 0;

    private Vector2 _mapRangeStartPos;
    private Vector2 _mapRangeEndPos;

    private CharacterEventHandler _cEventHandler;
    private SkillBase _currentPlaySkill;
    private TimeAction _currentSkillTimeEvent;
    private void Awake()
    {
        _transform = gameObject.transform;

        //상태 전환 흐름 설정
        Init();
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        _cEventHandler = null;
    }

    public CharacterBase GetCharacter()
    {
        return _character;
    }

    public void UpdateStateMachine()
    {
        if (_currentState == null) return;

        _transform.position = _character.CurPos;

        _currentAtkCoolTime -= Time.fixedDeltaTime;

        _currentState.UpdateBase();
        _character.GetSkill(EInputAction.SKILL1).UpdateBase();
        _character.GetSkill(EInputAction.SKILL2).UpdateBase();
        _character.GetSkill(EInputAction.SKILL3).UpdateBase();
        _character.GetSkill(EInputAction.SKILL4).UpdateBase();
        _character.GetSkill(EInputAction.ULT_SKILL).UpdateBase();

    }

    public void Initialize(CharacterBase character, ECharacterType characterType, Vector2 mapPos1, Vector2 mapPos2)
    {
        _cEventHandler = GetComponentInChildren<CharacterEventHandler>();
        _cEventHandler.Initialize(character.Proto.Id, character.Proto.TeamType.ToString());

        _character = character;
        _transform.position = _character.CurPos;
        _mapRangeStartPos= mapPos1;
        _mapRangeEndPos= mapPos2;
        SetState(new IdleState());
    }

    public void SetState(CharacterState<CharacterBase> nextState)
    {
        if (_currentState != null)
        {
            // 기존의 상태가 존재했다면 OnExit()호출
            _currentState.OnExitBase();

        }

        // 다음state 시작
        _currentStateName = nextState.ToString();
        _currentState = nextState;
        _currentState.OnEnterBase(_character, this);

    }

    //Character
    public void MoveCharacterPos(Vector2 dir)
    {
        _character.SetDir(new Vector3(dir.x, 0, dir.y));
        dir = dir * _character.SPD.Value * Time.fixedDeltaTime;

        //TODO : 추후 제한 구역 싱글톤으로 빼기
        if (!CanMove(_character.CurPos + new Vector3(dir.x, 0, dir.y))) return;
        _character.TranslateDir( new Vector3(dir.x, 0, dir.y));

        if(_cEventHandler != null)
            _cEventHandler.Move(_character.CurDir);
    }

    public void Attack()
    {
        if (!IsReadyToAttack()) return;

        //때리기
        _currentAtkCoolTime = 1 / _character.ATKSPD.Value ;
        UseNormalAttck();
    }

    public void NonTargetingDirAttack(Vector3 dir)
    {
        if (!IsReadyToAttack()) return;
        _currentAtkCoolTime = _character.ATKSPD.Value;
        //APP.Characters.FindTargetAndApplyDamage(_character, new HitBox(EHitShapeType.CORN, _character.RANGE.Value, dir, _character.AttackRangeAngle)
        //    , EHitSKillType.ALONE
        //    , EAttack.ATK);
    }

    public void TargetingDirAttack(CharacterBase target)
    {
        if (!IsReadyToAttack()) return;
        _currentAtkCoolTime = _character.ATKSPD.Value;
        // APP.Characters.FindTargetAndApplyDamage(_character, new HitBox(EHitShape.Corn, _character.AttackRangeRadius, dir, _character.AttackRangeAngle)
        //    , EHitType.ALONE
        //    , EAttack.ATK);
    }

    public void PlayStartAnim() => _cEventHandler.PlayStartAnim();
    public void PlayDieAnim() => _cEventHandler.PlayDieAnim();
    public void SetIdle() => _cEventHandler.SetIdleState();
    

    public void UseNormalAttck() => UseSkill(EInputAction.ATTACK);
    public void UseSkill1() => UseSkill(EInputAction.SKILL1);
    public void UseSkill3() => UseSkill(EInputAction.SKILL2); 
    public void UseSkill4() => UseSkill(EInputAction.SKILL3);
    public void UseUltSkill() => UseSkill(EInputAction.ULT_SKILL);
    public void UseSkill2() => UseSkill(EInputAction.SKILL4);

    private void UseSkill(EInputAction inputAction)
    {
        SkillBase skill= _character.GetSkill(inputAction);
        if(skill == null)
            return;

        if(_cEventHandler._isPlayingSkill || !skill.CanUseSkill())
            return;

        float applyTiming = skill._skillProto.ApplyPointTime;
        if (inputAction == EInputAction.ATTACK)
        {//기본 공격 속도 조정
            applyTiming /= _character.ATKSPD.Value;
            _cEventHandler.SetAttackSpeed(_character.ATKSPD.Value);
        }
        _currentPlaySkill = skill;

        //스킬 애니메이션 재생
        _cEventHandler.PlayAttackAnim(inputAction);
        skill.StartSkill(_currentTarget);

        if (skill._skillProto.CanNotMoveTime > 0)// 못움직이는 시간
        {
            SetState(new ChannelingState(skill._skillProto.CanNotMoveTime, skill._skillProto.CanCancel));
        }

        _currentSkillTimeEvent = TimeHelper.AddTimeEvent(applyTiming,
            ApplyCurrentSkill, skill._skillProto.Name);
    }

    //현재 시전중인 스킬 효과 시전
    public void ApplyCurrentSkill()
    {
        if(_currentPlaySkill == null)
        {
            GameLogger.Info($"{_character.Name} 시전중인 스킬이 없음");
            return;
        }
        _currentPlaySkill.UseSkill();
    }

    public void CancelCurrentSkill()
    {
        if (_currentSkillTimeEvent == null)
        {
            GameLogger.Info($"{_character.Name} 시전중인 스킬이 없는데 취소함.");
            return;
        }

        _currentPlaySkill.CancelSkill();
        _currentPlaySkill = null;

        _cEventHandler.SetIdleState();
        TimeHelper.RemoveTimeEvent(_currentSkillTimeEvent);
        SetState(new playerNormalState());
        GameLogger.Info($"시전중인 스킬 {_currentSkillTimeEvent.Name} 취소");
        
    }

    private bool CanMove(Vector3 nextPos)
    {
        return nextPos.x > _mapRangeStartPos.x && nextPos.x < _mapRangeEndPos.x
            && nextPos.z > _mapRangeStartPos.y && nextPos.z < _mapRangeEndPos.y;
    }

    private bool IsReadyToAttack()
    {
        return _currentAtkCoolTime <= 0;
    }

    virtual protected void Init()
    {

    }

}