using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class GameLogger
{
    
    static public void Info(string msg, params object[] args)
    {
        Debug.Log(RichText.color.ToString("black", string.Format(msg,args)));
    }
    static public void NotImp(string msg, params object[] args)
    {
        Debug.Log(RichText.color.ToString("gray", string.Format(msg, args)));
    }
    static public void Strong(string msg, params object[] args)
    {
        Debug.Log(RichText.color.ToString("lime", string.Format(msg, args)));
    }
    static public void Error(string msg, params object[] args)
    {
        Debug.LogError(RichText.color.ToString("red", string.Format(msg, args)));
    }
    static public void Network(string msg, params object[] args)
    {
        Debug.Log(RichText.color.ToString("blue", string.Format(msg, args)));
    }
}
