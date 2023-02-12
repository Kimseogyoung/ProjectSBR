using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogger
{

    static public void Info(string msg, params object[] args)
    {
        Debug.Log(StringUtils.color.ToString("black", string.Format(msg,args)));
    }
    static public void Strong(string msg, params object[] args)
    {
        Debug.Log(StringUtils.color.ToString("lime", string.Format(msg, args)));
    }
    static public void Error(string msg, params object[] args)
    {
        Debug.LogError(StringUtils.color.ToString("red", string.Format(msg, args)));
    }
    static public void Network(string msg, params object[] args)
    {
        Debug.Log(StringUtils.color.ToString("blue", string.Format(msg, args)));
    }
}
