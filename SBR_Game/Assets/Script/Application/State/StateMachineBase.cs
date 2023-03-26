using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

        _cEventHandler = GetComponentInChildren<CharacterEventHandler>();

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

        _currentAtkCoolTime -= Time.fixedDeltaTime;

        _currentState.UpdateBase();
        _character.GetSkill(EInputAction.SKILL1).UpdateSkill();
        _character.GetSkill(EInputAction.SKILL2).UpdateSkill();
        _character.GetSkill(EInputAction.SKILL3).UpdateSkill();
        _character.GetSkill(EInputAction.ULT_SKILL).UpdateSkill();
       
    }

    public void SetCharacter(CharacterBase character, ECharacterType characterType, Vector2 mapPos1, Vector2 mapPos2)
    {
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
        _character.CurDir = new Vector3(dir.x, 0, dir.y);
        dir = dir * _character.SPD.Value * Time.fixedDeltaTime;

        if (!CanMove(_character.CurPos + new Vector3(dir.x, 0, dir.y))) return;
        _character.CurPos += new Vector3(dir.x, 0, dir.y);

        if(_cEventHandler != null)
            _cEventHandler.Move(_character.CurDir);

        SyncCharacterPos();
    }

    public void MoveCharacterPos(Vector3 vector3)
    {
        if (!CanMove(_character.CurPos + vector3)) return;
        _character.CurPos += vector3;
        SyncCharacterPos();
    }
    public void SetCharacterPos(Vector3 vector3)
    {
        if (!CanMove(vector3)) return;
        _character.CurPos = vector3;
        SyncCharacterPos();
    }

    public void SyncCharacterPos()
    {
        
        _transform.position = _character.CurPos;
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

    public void UseNormalAttck() => UseSkill(EInputAction.ATTACK);
    public void UseSkill1() => UseSkill(EInputAction.SKILL1);
    public void UseSkill2() => UseSkill(EInputAction.SKILL2); 
    public void UseSkill3() => UseSkill(EInputAction.SKILL3);
    public void UseUltSkill() => UseSkill(EInputAction.ULT_SKILL);

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

        if (!skill._skillProto.CanMove)//�����ð����� ������ �� �ִ°�? (ĵ���� �� ����)
        {
            SetState(new ChannelingState(applyTiming));
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