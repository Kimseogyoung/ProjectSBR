using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterEventHandler : MonoBehaviour
{
    [SerializeField] public List<SBRAnimator.AnimData> AnimList = new List<SBRAnimator.AnimData>();
    [SerializeField][HideInInspector] public int protoIndex = 0;

    private Dictionary<string, AnimationClip> _skillClipDict = new Dictionary<string, AnimationClip>();
    private Animator _animator;
    private Transform _transform;
    private Quaternion _rightQuaternion;
    private Quaternion _leftQuaternion;

    private string _currentPlayingSkillAnim;
    public bool _isPlayingSkill { get; private set; }

    public bool _isMoving = false;
    private string _isMovingKey = "IsMoving";
    private string _atkSpdKey = "AtkSpd";
    private AnimationClip _attackClip;

    private TimeHelper.TimeAction _finishTimeAction;

    public void Initialize(int id, string type)
    {

        // 새 애니메이터 생성
        _animator = gameObject.GetComponent<Animator>();
        _animator.applyRootMotion = true;
       
        _transform = gameObject.transform;
        _leftQuaternion = Quaternion.Euler(_transform.rotation.eulerAngles);
        _rightQuaternion = Quaternion.Euler(new Vector3(-_leftQuaternion.eulerAngles.x, 180, _leftQuaternion.eulerAngles.z));

        foreach (var anim in AnimList)
        {
            if (anim.Type == ECharacterActionType.ATTACK)
                _attackClip = anim.AnimationClip;
            _skillClipDict.Add(anim.Type.ToString(), anim.AnimationClip);
            //Debug.Log($"State Name: {((AnimationClip)state.state.motion).length}");
        }

    }

    public void Move(Vector3 dir)
    {
        if (dir.x < 0)
            _transform.rotation = _leftQuaternion;
        else if (dir.x > 0)
            _transform.rotation = _rightQuaternion;
        _isMoving = true;
    }

    public void SetIdleState()
    {
        _animator.Play(ECharacterActionType.IDLE.ToString(), 0);
        OnFinishSkill();
        TimeHelper.RemoveTimeEvent(_finishTimeAction);
    }

    public void PlayStartAnim() => _animator.Play(ECharacterActionType.START.ToString(), 0);
    public void PlayDieAnim() => _animator.Play(ECharacterActionType.DIE.ToString(), 0);


    public void SetAttackSpeed(float spd)
    {
        //_attackClip.speed= spd;
        //_attackAnimState.speed = spd;
        //_animator.SetFloat(_atkSpdKey, spd);
    }

    public bool PlayAttackAnim(EInputAction inputAction)
    {
        if (_isPlayingSkill)
            return false;

        switch (inputAction)
        {       
            case EInputAction.ATTACK:
                _animator.Play(inputAction.ToString(), 0,0);
                _finishTimeAction = TimeHelper.AddTimeEvent("character-attack-finish-time", _skillClipDict[inputAction.ToString()].length/1/* _attackClip.speed*/, OnFinishSkill);
                _isPlayingSkill = true;
                break;
            case EInputAction.SKILL1:
            case EInputAction.SKILL3:
            case EInputAction.SKILL4:
            case EInputAction.ULT_SKILL:
            case EInputAction.SKILL2:
                _animator.Play(inputAction.ToString(),0,0);
                _finishTimeAction = TimeHelper.AddTimeEvent("character-attack-finish-time", _skillClipDict[inputAction.ToString()].length, OnFinishSkill);
                _isPlayingSkill = true;
                break;
            default:
                LOG.E($"NoAttackAnim EInputAction({inputAction})");
                return false;
        }

        return true;
    }

    private void FixedUpdate()
    {
        if (_animator == null)
            return;

        _animator.SetBool(_isMovingKey, _isMoving);
        _isMoving = false;

        //종료 시점 체크
        /*
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("CurrentStateName") &&
     _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("Current State Name 애니메이션 종료.");
        }
        */
    }
 
    private void OnFinishSkill()
    {
        _isPlayingSkill = false;
    }
}
