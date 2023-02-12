using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : IManager, IManagerUpdatable
{
    private SceneBase _currentScene;
    public void FinishManager()
    {

    }

    public void Init()
    {
        
    }

    public void PrepareManager()
    {

    }

    public void StartManager()
    {
        LoadNewScene(APP.GameConf.StartScene);
    }

    public void ChangeScene(string nextSceneName)
    {
        _currentScene.Exit();
        LoadNewScene(nextSceneName);
    }

    private void LoadNewScene(string nextSceneName)
    {
        UnitySceneManager.LoadScene(nextSceneName);

        switch (nextSceneName)
        {
            case "LobbyScene":
                _currentScene = new LobbyScene();
                break;
            case "InGameScene":
                _currentScene = new InGameScene();
                break;
            default:
                break;
        }
        _currentScene.Enter();
        _currentScene.Start();
    }

    public void UpdateManager()
    {
        if (_currentScene == null) return;
        _currentScene.Update();
    }
}

