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

    private Transform _transform;//���� ĳ���� ��ġ
    private float _currentAtkCoolTime = 0;

    private Vector2 _mapRangeStartPos;
    private Vector2 _mapRangeEndPos;

    private CharacterEventHandler _cEventHandler;
    private SkillBase _currentPlaySkill;
    private TimeAction _currentSkillTimeEvent;
    private void Awake()
    {
        _transform = gameObject.transform;

        //���� ��ȯ �帧 ����
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
            // ������ ���°� �����ߴٸ� OnExit()ȣ��
            _currentState.OnExitBase();

        }

        // ����state ����
        _currentStateName = nextState.ToString();
        _currentState = nextState;
        _currentState.OnEnterBase(_character, this);

    }

    //Character
    public void MoveCharacterPos(Vector2 dir)
    {
        _character.SetDir(new Vector3(dir.x, 0, dir.y));
        dir = dir * _character.SPD.Value * Time.fixedDeltaTime;

        //TODO : ���� ���� ���� �̱������� ����
        if (!CanMove(_character.CurPos + new Vector3(dir.x, 0, dir.y))) return;
        _character.TranslateDir( new Vector3(dir.x, 0, dir.y));

        if(_cEventHandler != null)
            _cEventHandler.Move(_character.CurDir);
    }

    public void Attack()
    {
        if (!IsReadyToAttack()) return;

        //������
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
        {//�⺻ ���� �ӵ� ����
            applyTiming /= _character.ATKSPD.Value;
            _cEventHandler.SetAttackSpeed(_character.ATKSPD.Value);
        }
        _currentPlaySkill = skill;

        //��ų �ִϸ��̼� ���
        _cEventHandler.PlayAttackAnim(inputAction);
        skill.StartSkill(_currentTarget);

        if (skill._skillProto.CanNotMoveTime > 0)// �������̴� �ð�
        {
            SetState(new ChannelingState(skill._skillProto.CanNotMoveTime, skill._skillProto.CanCancel));
        }

        _currentSkillTimeEvent = TimeHelper.AddTimeEvent(applyTiming,
            ApplyCurrentSkill, skill._skillProto.Name);
    }

    //���� �������� ��ų ȿ�� ����
    public void ApplyCurrentSkill()
    {
        if(_currentPlaySkill == null)
        {
            GameLogger.Info($"{_character.Name} �������� ��ų�� ����");
            return;
        }
        _currentPlaySkill.UseSkill();
    }

    public void CancelCurrentSkill()
    {
        if (_currentSkillTimeEvent == null)
        {
            GameLogger.Info($"{_character.Name} �������� ��ų�� ���µ� �����.");
            return;
        }

        _currentPlaySkill.CancelSkill();
        _currentPlaySkill = null;

        _cEventHandler.SetIdleState();
        TimeHelper.RemoveTimeEvent(_currentSkillTimeEvent);
        SetState(new playerNormalState());
        GameLogger.Info($"�������� ��ų {_currentSkillTimeEvent.Name} ���");
        
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