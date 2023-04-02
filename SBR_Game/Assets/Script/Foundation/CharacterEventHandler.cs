using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;


public class CharacterEventHandler : MonoBehaviour
{
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
    private AnimatorState _attackState;

    private TimeHelper.TimeAction _finishTimeAction;

    public void Initialize(int id, string type)
    {

        // 새 애니메이터 생성
        _animator = gameObject.AddComponent<Animator>();
        _animator.applyRootMotion = true;
        _animator.runtimeAnimatorController = Util.Resource.Load<RuntimeAnimatorController>($"Character/Controller/{type}_{id}");

        _transform = gameObject.transform;
        _leftQuaternion = Quaternion.Euler(_transform.rotation.eulerAngles);
        _rightQuaternion = Quaternion.Euler(new Vector3(-_leftQuaternion.eulerAngles.x, 180, _leftQuaternion.eulerAngles.z));

        AnimatorController controller = _animator.runtimeAnimatorController as AnimatorController;
        foreach (var state in controller.layers[0].stateMachine.states)
        {
            if (state.state.name == ECharacterActionType.ATTACK.ToString())
                _attackState = state.state;
            _skillClipDict.Add(state.state.name, (AnimationClip)state.state.motion);
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
        _attackState.speed= spd;
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
                _finishTimeAction = TimeHelper.AddTimeEvent(_skillClipDict[inputAction.ToString()].length/ _attackState.speed, OnFinishSkill);
                _isPlayingSkill = true;
                break;
            case EInputAction.SKILL1:
            case EInputAction.SKILL3:
            case EInputAction.SKILL4:
            case EInputAction.ULT_SKILL:
            case EInputAction.SKILL2:
                _animator.Play(inputAction.ToString(),0,0);
                _finishTimeAction = TimeHelper.AddTimeEvent(_skillClipDict[inputAction.ToString()].length, OnFinishSkill);
                _isPlayingSkill = true;
                break;
            default:
                GameLogger.Error($"NoAttackAnim EInputAction({inputAction})");
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
