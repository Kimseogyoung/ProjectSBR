using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SceneBase 
{
    public string _sceneName { get; protected set; } = string.Empty;
    

    public void EnterBase()
    {
        GameLogger.Info($"{_sceneName} Enter!");
        Enter();
    }

    public void StartBase()
    {
        GameLogger.Info($"{_sceneName} Start!");
        Start();
    }

    public void UpdateBase()
    {
        Update();
    }
    public void ExitBase()
    {
        GameLogger.Info($"{_sceneName} Exit!");
        Exit();
    }

    abstract protected void Enter();

    abstract protected void Start();

    abstract protected void Update();

    abstract protected void Exit();

}
