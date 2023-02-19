using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GizmoHelper : MonoBehaviour
{
    private static Queue<Action> drawQueue = new Queue<Action>();

    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(GizmoHelper)}]").AddComponent<GizmoHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    static public void PushDrawQueue(Action action) { drawQueue.Enqueue(action); }

    private void OnDrawGizmos()
    {
        while(drawQueue.Count > 0)
        {
            Action action = drawQueue.Dequeue();
            action.Invoke();
        }
    }


}
