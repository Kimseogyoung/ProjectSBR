using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class InGameScene : SceneBase
{
    private CharacterManager _characterManager;
    private BulletManager _bulletManager;
    private UI_InGameScene _inGameScene;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        _inGameScene = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");

        SpawnMap(APP.CurrentStage);

        _characterManager = new CharacterManager();
        _characterManager.Init();

        _bulletManager = new BulletManager();
        _bulletManager.Init();

        APP.GameManager.AddUpdatablePublicManager(_characterManager);
        APP.GameManager.AddUpdatablePublicManager(_bulletManager);


        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BOSS_DEAD, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PLAYER_DEAD, FailGame);
    }

    private void SuccessGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));
        _inGameScene.ShowFinishPopup(deadEvent);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));
        _inGameScene.ShowFinishPopup(deadEvent);
    }

    private void SpawnMap(StageProto currentStage)
    {
        GameObject map = GameObject.FindObjectOfType<Terrain>()?.gameObject;
        if(map == null)
            map = Util.Resource.Instantiate(currentStage.PrefabPath);

        if (map == null)
        {
            GameLogger.Error($"No GameObject {currentStage.PrefabPath}");
            return;
        }
        map.transform.position = new Vector3(-currentStage.Width / 2, -1, -currentStage.Height / 2);
    }

    protected override void Exit()
    {
        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.BOSS_DEAD, SuccessGame);
        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.PLAYER_DEAD, FailGame);

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
