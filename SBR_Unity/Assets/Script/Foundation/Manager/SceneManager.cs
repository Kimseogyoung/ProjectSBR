using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour, IManager, IManagerUpdatable
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
        CoroutineHelper.StartCoroutine(CLoadScene("LobbyScene"));
    }

    public void ChangeScene(string nextSceneName)
    {
        _currentScene.Exit();
        CoroutineHelper.StartCoroutine(CLoadScene(nextSceneName));
    }

    public void UpdateManager()
    {
        if (_currentScene == null) return;
        _currentScene.Update();
    }

    private IEnumerator CLoadScene(string nextSceneName)
    {
        AsyncOperation async = UnitySceneManager.LoadSceneAsync(nextSceneName);

        while (!async.isDone)
        {
            yield return null;
        }

        GameLogger.Info("Load");
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
}

