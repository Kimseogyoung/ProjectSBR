using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : SceneBase
{
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        APP.CharacterManager = new CharacterManager();
        APP.CharacterManager.Init();
    }

    protected override void Exit()
    {
        APP.CharacterManager.FinishManager();
        APP.CharacterManager = null;
    }

    protected override void Start()
    {
        APP.CharacterManager.StartManager();

    }

    protected override void Update()
    {
        APP.CharacterManager.UpdateManager();
    }

    
}
