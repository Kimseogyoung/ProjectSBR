using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TimeHelper : MonoBehaviour
{ 
    private static bool _isStopped = false;
    private static float _currentTime = 0;
    private static List<TimeAction> _timeActionList = new List<TimeAction>();

    public static void AddTimeEvent(float time, Action action, string name = "")
    {
        if(time <= 0)
        {
            action.Invoke();
            return;
        }
        _timeActionList.Add(new TimeAction(_currentTime + time, action, name));
    }

    public static void Stop(bool isStopped)
    {
        _isStopped = isStopped;
    }

    public static void RemoveTimeEvent(string name)
    {
        TimeAction timeEvent = _timeActionList.Find(x => x.Name == name);
        if (timeEvent == null)
            return;

        _timeActionList.Remove(timeEvent);
    }

    private void FixedUpdate()
    {
        if (_isStopped)
            return;

        _currentTime += Time.fixedDeltaTime;

        for (int i = 0; i < _timeActionList.Count; i++)
        {
            if (_timeActionList[i].Time < _currentTime) 
            {
                _timeActionList[i].Action.Invoke();
                _timeActionList.RemoveAt(i);
                i--;
            }
        }
    }

    public class TimeAction
    {
        public float Time { get; private set; }
        public Action Action { get; private set; }
        public string Name { get; private set; }
        public TimeAction(float time, Action action, string name = "")
        {
            Time = time;
            Action = action;
            Name = name;
        }
    }


    private static MonoBehaviour monoInstance;
    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(TimeHelper)}]").AddComponent<TimeHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

}

