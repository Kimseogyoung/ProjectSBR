using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class InGameScene : SceneBase
{
    private CharacterManager _characterManager;
    private UI_InGameScene _inGameScene;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        _inGameScene = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");

        _characterManager = new CharacterManager();
        _characterManager.Init();
        APP.GameManager.AddUpdatablePublicManager(_characterManager);


        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BossDead, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PlayerDead, FailGame);
    }

    private void SuccessGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.Pause, new PauseEvent(true));
        _inGameScene.ShowFinishPopup(deadEvent);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.Pause, new PauseEvent(true));
    }

    protected override void Exit()
    {
        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.BossDead, SuccessGame);
        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.PlayerDead, FailGame);

        APP.GameManager.RemoveUpdatablePublicManager(_characterManager);
        _characterManager.FinishManager();
    }

    protected override void Start()
    {
        _characterManager.StartManager();
    }

    protected override void Update()
    {

    }

    
}
