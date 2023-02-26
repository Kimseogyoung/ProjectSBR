using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : SceneBase
{
    private CharacterManager characterManager;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");
        characterManager = new CharacterManager();
        characterManager.Init();
    }

    protected override void Exit()
    {
        characterManager.FinishManager();
        characterManager = null;
    }

    protected override void Start()
    {
        characterManager.StartManager();

    }

    protected override void Update()
    {
        characterManager.UpdateManager();
    }

    
}
