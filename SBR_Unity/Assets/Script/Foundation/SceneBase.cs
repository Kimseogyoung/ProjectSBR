using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SceneBase 
{
    public string _sceneName { get; protected set; } = string.Empty;
    abstract public void Enter();

    abstract public void Start();

    abstract public void Update();

    abstract public void Exit();

}
