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
using static UnityEngine.GraphicsBuffer;


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

    private CharacterEventHandler _cEventHandlder;
    private void Awake()
    {
        _transform = gameObject.transform;

        _cEventHandlder = GetComponentInChildren<CharacterEventHandler>();
        if(_cEventHandlder != null)
            _cEventHandlder.OnApplySkill += UseNormalAttck;

        //상태 전환 흐름 설정
        Init();
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        _cEventHandlder.OnApplySkill -= UseNormalAttck;
        _cEventHandlder = null;
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
        _character.CurDir = new Vector3(dir.x, 0, dir.y);
        dir = dir * _character.SPD.Value * Time.fixedDeltaTime;

        if (!CanMove(_character.CurPos + new Vector3(dir.x, 0, dir.y))) return;
        _character.CurPos += new Vector3(dir.x, 0, dir.y);

        if(_cEventHandlder != null)
            _cEventHandlder.Move(_character.CurDir);

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


        //때리기
        _currentAtkCoolTime = _character.ATKSPD.Value;
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
        if(skill != null)
        {
            if (!_cEventHandlder._isPlayingSkill && skill.CanUseSkill())
            {
                
                //스킬 애니메이션 재생
                _cEventHandlder.PlayAttackAnim(inputAction);
                skill.StartSkill(_currentTarget);
                if (!skill._skillProto.CanMove)
                {
                    SetState(new ChannelingState());
                }

                TimeHelper.AddTimeEvent(skill._skillProto.ApplyPointTime, () => { skill.UseSkill(); }, "channeling");
                GameLogger.Info("{0} 사용 시도", inputAction);

                //스킬 사용 성공
                return;
            }
        }
        //실패
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