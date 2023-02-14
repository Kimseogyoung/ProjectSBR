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
        Util.CoroutineHelper.StartCoroutine(CoLoadScene(APP.GameConf.StartScene));
    }

    public void ChangeScene(string nextSceneName)
    {
        _currentScene.ExitBase();
        Util.CoroutineHelper.StartCoroutine(CoLoadScene(nextSceneName));
    }

    public void UpdateManager()
    {
        if (_currentScene == null) return;
        _currentScene.UpdateBase();
    }

    private IEnumerator CoLoadScene(string nextSceneName)
    {
        AsyncOperation async;
        if (UnitySceneManager.GetActiveScene().name != nextSceneName)
        {
            async = UnitySceneManager.LoadSceneAsync(nextSceneName);

            while (!async.isDone)
            {
                yield return null;
            }

        }

        switch (nextSceneName)
        {
            case "LobbyScene":
                _currentScene = new LobbyScene(nextSceneName);
                break;
            case "InGameScene":
                _currentScene = new InGameScene(nextSceneName);
                break;
            default:
                break;
        }
        _currentScene.EnterBase();
        _currentScene.StartBase();
    }
}

