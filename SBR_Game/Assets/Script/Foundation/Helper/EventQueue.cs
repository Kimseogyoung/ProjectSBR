using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Util;
public enum EEventActionType
{
    None = 0,
    PlayerHpChange,
    PlayerMpChange,
    BossHpChange,
    ZzolHpChange,
    Phase1Start,
    Phase2Start,
    StageInfoSet,
    PlayerDead,
    BossDead,
    ZzolDead
}

public class EventQueue : MonoBehaviour
{
    private static Dictionary<EEventActionType, Action<EventBase>> _actions = new Dictionary<EEventActionType, Action<EventBase>>();
    private static Queue<EventBase> _eventQueue= new Queue<EventBase>();

    private float _playingTime = 0;
    private bool _isPlayingEvent = false;
    private EventBase _playingEvent = null;

    static public void PushEvent<T>(EEventActionType eventActionType, T evt, double time = 0) where T : EventBase
    {
        evt.eventActionType = eventActionType;
        evt.eventPlayTime = time;
        _eventQueue.Enqueue(evt);
    }

    static public void AddEventListener<T>(EEventActionType eventActionType, Action<T> action) where T : EventBase
    {
        Action<EventBase> act = (e) => { action.Invoke((T)e); };
        if (!_actions.ContainsKey(eventActionType))
        {
            _actions.Add(eventActionType, act);
        }
        _actions[eventActionType] += act;
    }
    static public void RemoveEventListener<T>(EEventActionType eventActionType, Action<T> action) where T : EventBase
    {
        Action<EventBase> act = (e) => { action.Invoke((T)e); };
        if (!_actions.ContainsKey(eventActionType))
        {
            return;
        }
        _actions[eventActionType] = null;
    }

    private void Update()
    {
        if(!_isPlayingEvent)
        {
            if (_eventQueue.Count > 0)
            {
                _playingEvent = _eventQueue.Dequeue();
                if(!_actions.TryGetValue(_playingEvent.eventActionType, out Action<EventBase> action) || action == null)
                {
                    return; 
                }
                GameLogger.Info("EventQueue Dequeue : {0} 이벤트 호출", _playingEvent.eventActionType);
                action.Invoke(_playingEvent);
              
            }
            return;
        }
        if (_playingTime >= _playingEvent.eventPlayTime)
        {
            _playingTime = 0;
            _isPlayingEvent = false;
            return;
        }
        else
            _isPlayingEvent = true;

        _playingTime += Time.deltaTime;
    }

    private static MonoBehaviour monoInstance;
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(EventQueue)}]").AddComponent<EventQueue>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }
}

