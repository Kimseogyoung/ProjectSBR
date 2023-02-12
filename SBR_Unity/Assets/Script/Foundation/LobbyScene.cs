using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : SceneBase
{

    public LobbyScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        APP.UI.ShowSceneUI<UI_Scene>("Canvas");

    }

    protected override void Exit()
    {
       
    }

    protected override void Start()
    {
       
    }

    protected override void Update()
    {
        
    }
}
