using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Core.Tokens;

public enum EInputAction
{
    MOVE,
    PAUSE,
    PLAY,
    ESC,
    TAB,
    FASTMODE,
    ATTACK,
    SKILL1,
    SKILL2,
    SKILL3,
    ULT_SKILL
}

public class InputManager :IManager, IManagerUpdatable
{
    private bool _isStopped = false;

    private Dictionary<EInputAction, List<KeyCode>> _action2keyMappings = new Dictionary<EInputAction, List<KeyCode>>();
    private Dictionary<EInputAction, Action> _actions = new Dictionary<EInputAction, Action>();

    private Dictionary<EInputAction, float> _lastPlayedActionTime = new Dictionary<EInputAction, float>();

    //pause ���¿����� ��� ������ �׼�
    private List<EInputAction> _actionsCanUsePause = new List<EInputAction>()
    {
        EInputAction.TAB, EInputAction.PAUSE, EInputAction.PLAY, EInputAction.ESC, EInputAction.FASTMODE
    };

    //Move�� dir�� �ޱ� ������ �и�
    private Action<Vector2> _moveAction;

    private float _time = 0f;
    private float _baseCoolTime = 0.1f;


    public void Init()
    {
        _action2keyMappings.Add(EInputAction.ESC, new List<KeyCode>() { KeyCode.Escape });
        _action2keyMappings.Add(EInputAction.TAB, new List<KeyCode>() { KeyCode.Tab });
        _action2keyMappings.Add(EInputAction.PAUSE, new List<KeyCode>() { KeyCode.P });
        _action2keyMappings.Add(EInputAction.PLAY, new List<KeyCode>() { KeyCode.P });
        _action2keyMappings.Add(EInputAction.FASTMODE, new List<KeyCode>() { KeyCode.LeftControl, KeyCode.RightControl });
        _action2keyMappings.Add(EInputAction.ATTACK, new List<KeyCode>() { KeyCode.Q });
        _action2keyMappings.Add(EInputAction.SKILL1, new List<KeyCode>() { KeyCode.Alpha1 });
        _action2keyMappings.Add(EInputAction.SKILL2, new List<KeyCode>() { KeyCode.Alpha2 });
        _action2keyMappings.Add(EInputAction.SKILL3, new List<KeyCode>() { KeyCode.Alpha3 });
        _action2keyMappings.Add(EInputAction.ULT_SKILL, new List<KeyCode>() { KeyCode.Alpha4 });

        foreach (EInputAction inputAction in Enum.GetValues(typeof(EInputAction)))
        {
            _lastPlayedActionTime.Add(inputAction, 0);
            _actions.Add(inputAction, null);
        }

    }

    public void FinishManager()
    {

    }

    public void StartManager()
    {

    }

    public void AddInputAction(EInputAction inputAction, Action action) { _actions[inputAction] += action; }
    public void AddInputAction(EInputAction inputAction, Action<Vector2> action) { _moveAction += action; }
    public void RemoveInputAction(EInputAction inputAction, Action action) { _actions[inputAction] -= action; }
    public void RemoveInputAction(EInputAction inputAction, Action<Vector2> action) { _moveAction -= action; }
    public void ClearInputAction(EInputAction inputAction) 
    {
        if (inputAction == EInputAction.MOVE)
            _moveAction = null;
        else
            _actions[inputAction] = null; 
    }

    public void UpdateManager()
    {
        //���� �������� ������ �ʸ� ���
        _time += Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        if (h != 0 || v != 0)
            InvokeMoveKeyAction(new Vector2(h, v));

        foreach(EInputAction key in _action2keyMappings.Keys)
        {
            for(int i=0; i< _action2keyMappings[key].Count; i++)
            {
                if (Input.GetKey(_action2keyMappings[key][i]))
                    InvokeKeyAction(key);
            }
        }
        
    }

    public void UpdatePausedManager()
    {
     
    }

    public void Pause(bool IsPause)
    {
       
    }

    public void InvokeKeyAction(EInputAction inputAction)
    {
        if (!CanInvokeAction(inputAction)) return;

        _actions[inputAction]?.Invoke();
        _lastPlayedActionTime[inputAction] = _time;
    }
    public void InvokeMoveKeyAction(Vector2 dir)
    {
        if (!CanInvokeAction(EInputAction.MOVE)) return;

        _moveAction?.Invoke(dir);
        _lastPlayedActionTime[EInputAction.MOVE] = _time;
    }

    private bool CanInvokeAction(EInputAction action)
    {
        
        if (_isStopped)
            if (!_actionsCanUsePause.Contains(action)) return false;

        if(action == EInputAction.MOVE)
            return true;
        

        float term = _time - _lastPlayedActionTime[action];
        if (_baseCoolTime > term)
            return false;
        return true;
    }

    public void SetStop(bool isStop)
    {
        _isStopped = isStop;
    }

}
