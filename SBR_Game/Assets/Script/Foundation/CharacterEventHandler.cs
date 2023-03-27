using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public enum ECharacterAnim
{
    NONE,
    IDLE,
    RUN,
    ATTACK,
    START,
    DIE
}

public class CharacterEventHandler : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> _skillClipList = new List<AnimationClip>();
    [SerializeField] private AnimationClip _idleClip;
    [SerializeField] private AnimationClip _runClip;
    [SerializeField] private AnimationClip _startClip;
    [SerializeField] private AnimationClip _dieClip;

    private Dictionary<EInputAction, AnimationClip> _skillClipDict = new Dictionary<EInputAction, AnimationClip>();
    private Animator _animator;
    private Transform _transform;
    private Quaternion _rightQuaternion;
    private Quaternion _leftQuaternion;

    private string _currentPlayingSkillAnim;
    public bool _isPlayingSkill { get; private set; }

    public bool _isMoving = false;
    private string _isMovingKey = "IsMoving";
    private string _atkSpdKey = "AtkSpd";

    private AnimatorState _attackAnimState;
    private AnimatorController _controller;
    private TimeHelper.TimeAction _finishTimeAction;

    public void Initialize()
    {
        for (int i = 0; i < _skillClipList.Count; i++)
        {
            _skillClipDict.Add((EInputAction)EInputAction.ATTACK + i, _skillClipList[i]);
        }

        // 새 애니메이터 생성
        _animator = gameObject.AddComponent<Animator>();
        _animator.applyRootMotion = true;
        _controller = AnimatorController.CreateAnimatorControllerAtPath("Assets/NewController.controller");
        _animator.runtimeAnimatorController = _controller;

        _transform = gameObject.transform;
        _leftQuaternion = Quaternion.Euler(_transform.rotation.eulerAngles);
        _rightQuaternion = Quaternion.Euler(new Vector3(-_leftQuaternion.eulerAngles.x, 180, _leftQuaternion.eulerAngles.z));


        RegisterAnimClip();
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
        _animator.Play("Idle", 0);
        OnFinishSkill();
        TimeHelper.RemoveTimeEvent(_finishTimeAction);
    }

    public void PlayStartAnim() => _animator.Play(_startClip.name, 0);
    public void PlayDieAnim() => _animator.Play(_dieClip.name, 0);


    public void SetAttackSpeed(float spd)
    {
        _attackAnimState.speed = spd;
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
                _finishTimeAction = TimeHelper.AddTimeEvent(_skillClipDict[inputAction].length/ _attackAnimState.speed, OnFinishSkill);
                _isPlayingSkill = true;
                break;
            case EInputAction.SKILL1:
            case EInputAction.SKILL2:
            case EInputAction.SKILL3:
            case EInputAction.ULT_SKILL:
                _animator.Play(inputAction.ToString(),0,0);
                _finishTimeAction = TimeHelper.AddTimeEvent(_skillClipDict[inputAction].length, OnFinishSkill);
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

        _animator.SetBool("IsMoving", _isMoving);
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

    private void RegisterAnimClip()
    {
        _controller.AddMotion(_startClip);
        _controller.AddMotion(_dieClip);

        // 파라미터 추가
        AnimatorControllerParameter movingParam = new AnimatorControllerParameter();
        movingParam.type = AnimatorControllerParameterType.Bool;
        movingParam.name = _isMovingKey;

        AnimatorControllerParameter atkSpdParam = new AnimatorControllerParameter();
        atkSpdParam.type = AnimatorControllerParameterType.Float;
        atkSpdParam.name = _atkSpdKey;

        _controller.AddParameter(movingParam);
        _controller.AddParameter(atkSpdParam);

        // 상태 머신에서 기본 Idle 상태 추가
        AnimatorState idleState = _controller.layers[0].stateMachine.AddState("Idle");
        idleState.motion = _idleClip;
        _controller.layers[0].stateMachine.defaultState = idleState;
        

        // 상태 머신에서 Run 상태 추가
        AnimatorState runState = _controller.layers[0].stateMachine.AddState("Run");
        runState.motion = _runClip;

        // Idle -> Run 전이 추가
        AnimatorStateTransition idleToRun = idleState.AddTransition(runState);
        idleToRun.AddCondition(AnimatorConditionMode.If, 0, _isMovingKey);
        idleToRun.duration = 0.1f;

        // Run -> Idle 전이 추가
        AnimatorStateTransition runToIdle = runState.AddTransition(idleState);
        runToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, _isMovingKey);
        runToIdle.duration = 0.1f;


        foreach(var clip in _skillClipDict)
        {
            AnimatorState skillState = _controller.layers[0].stateMachine.AddState(clip.Key.ToString());
            skillState.motion = clip.Value;
            if(clip.Key == EInputAction.ATTACK)
                _attackAnimState = skillState;
            /*
            AnimationEvent animEvent = new AnimationEvent();
            // TODO : 각 스킬 타격점 으로 변경
            animEvent.time = 0.55f;
            animEvent.functionName = "OnApplyedSkill";
 
            clip.Value.AddEvent(animEvent);
            */
            AnimatorStateTransition skillToIdle = skillState.AddTransition(idleState);
            skillToIdle.hasExitTime = true;
        }
    }

    private void OnFinishSkill()
    {
        _isPlayingSkill = false;
    }
}
