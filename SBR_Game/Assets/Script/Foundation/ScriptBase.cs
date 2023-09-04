using System;
using UnityEngine;

public abstract class ScriptBase : MonoBehaviour
{
    public static bool CreateInstance<T>(out T outScript, GameObject resGO, GameObject rootGO, string name) where T : ScriptBase
    {
        if (!UTIL.TryCreateInstance<T>(out outScript, resGO, rootGO, name))
            return false;
        return true;
    }

    public static bool CreateInstance<T>(out T outScript, GameObject rootGO, string name) where T : ScriptBase
    {
        if (!UTIL.TryCreateEmptyInstance<T>(out outScript, rootGO, name))
            return false;
        return true;
    }

    public void OnEnable()
    {
        OnCreateScript();
    }

    public void OnDestroy()
    {
        OnDestroyScript();
    }

    protected abstract bool OnCreateScript();
    protected abstract bool OnDestroyScript();

}

