using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using static TimeHelper;

public class AnimBase : ScriptBase
{
    [SerializeField] private List<AnimationClip> _clipList = new();
    [SerializeField] private AnimationClip _introClip = null;
    [SerializeField] private AnimationClip _outroAnim = null;

    private Animator _animator;

    private PlayableGraph _playableGraph;
    private AnimationPlayableOutput _playableOutput;

    private Dictionary<string, int> _animationIdxList = new();
    private List<AnimationClipPlayable> _clipPlayableList = new();
    private string _curClipName = "";

    private TimeAction _timeAction = null;
    private Action _endAction = null;

    protected virtual bool UseInoutAnim => false;
    protected override bool OnCreateScript()
    {
        if(!UTIL.TryGetComponent<Animator>(out _animator, gameObject))
        {
            LOG.E($"Failed Get Animator(AnimBase) GameObjectName({gameObject.name})");
            return false;
        }
        // Playable 그래프 생성
        _playableGraph = PlayableGraph.Create();

        // 출력 설정
        _playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", _animator);

        // 각 애니메이션 클립을 Playable 상태로 변환하여 그래프에 추가
        for(int i=0; i < _clipList.Count; i++)
            AddClip(_clipList[i]);

        if (_introClip != null)
            AddClip(_introClip, "Intro");

        if (_outroAnim != null)
            AddClip(_outroAnim, "Outro");

        //TODO: Intro Outro 기능 추가


        return true;
    }

    public void PlayAnim(string name, Action endAction = null)
    {
        if (_playableGraph.IsPlaying())
        {
            StopAnim();
        }

        _curClipName = name;
        _playableOutput.SetSourcePlayable(GetAnim(name));
        _playableGraph.Play();

        if(endAction != null)
        {
            _endAction = endAction;
        }

        _timeAction = TimeHelper.AddTimeEvent($"name", GetAnimLength(), Stop);
    }

    public void StopAnim()// 강제 종료
    {
        _endAction = null;

        if ( _timeAction != null)
        {
            TimeHelper.RemoveTimeEvent(_timeAction);
            _timeAction = null;
        }

        Stop();
    }

    public float GetAnimLength()
    {
        AnimationClipPlayable playables = GetAnim(_curClipName);
        return playables.GetAnimationClip().length;
    }


    private void Stop()
    {
        _timeAction = null;
        _playableGraph.Stop();
        _curClipName = "";
        _endAction?.Invoke();
        _endAction = null;
    }

    private AnimationClipPlayable GetAnim(string name)
    {
        if (!_animationIdxList.TryGetValue(name, out var index))
        {
            LOG.E($"Not Found AnimClip Name({name})");
            index = 0;
        }
        return _clipPlayableList[index];
    }

    private void AddClip(AnimationClip clip, string name = "")
    {
        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(_playableGraph, clip);
        clipPlayable.SetDuration(clip.length);

        _clipPlayableList.Add(clipPlayable);

        name = string.IsNullOrEmpty(name) ? clip.name : name;
        _animationIdxList.Add(name, _animationIdxList.Count);
    }

    protected override void OnDestroyScript()
    {
        _playableGraph.Stop();
        _playableGraph.Destroy();

        _animator = null;
    }

    private void Update()
    {
       
    }
}

