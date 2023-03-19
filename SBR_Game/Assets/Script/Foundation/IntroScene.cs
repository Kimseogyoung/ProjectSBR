using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IntroScene : SceneBase
{
    private double _introTime = 1;
    private double _currentTime = 0;
    private bool _isLoadingNextScene = false;
    public IntroScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
    }

    protected override void Exit()
    {

    }

    protected override void Start()
    {

    }

    protected override void Update()
    {
        _currentTime += Time.fixedDeltaTime;
        if(_currentTime > _introTime && _isLoadingNextScene == false)
        {
            _isLoadingNextScene = true;
            APP.SceneManager.ChangeScene("LobbyScene");
        }
    }

}
