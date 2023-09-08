using System;
using UnityEngine;

public abstract class ScriptBase : MonoBehaviour
{
    public static void DestroyInstance<T>(ref T script) where T : ScriptBase
    {
        if (script == null)
            return;
        script.OnDestroy();
        GameObject.Destroy(script);
    }

    public static bool CreateInstance<T>(out T outScript, GameObject resGO, GameObject rootGO, string name) where T : ScriptBase
    {
        if (!UTIL.TryCreateInstance<T>(out outScript, resGO, rootGO, name))
        {
            DestroyInstance(ref outScript);
            return false;
        }
        return true;
    }

    public static bool CreateInstance<T>(out T outScript, GameObject rootGO, string name) where T : ScriptBase
    {
        if (!UTIL.TryCreateEmptyInstance<T>(out outScript, rootGO, name))
        {
            DestroyInstance(ref outScript);
            return false;
        }
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

