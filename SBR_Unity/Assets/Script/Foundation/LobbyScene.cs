using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : SceneBase
{
    public override void Enter()
    {
        GameLogger.Info("Enter");
        _sceneName = "LobbyScene";
        APP.UI.ShowSceneUI<UIScene>("Canvas");

    }

    public override void Exit()
    {
       
    }

    public override void Start()
    {
       
    }

    public override void Update()
    {
        
    }
}
