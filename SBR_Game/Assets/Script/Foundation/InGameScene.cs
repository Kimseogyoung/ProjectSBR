using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : SceneBase
{
    private InGameManager _inGameManager;
    private BulletManager _bulletManager;
    private UI_InGameScene _inGameSceneUI;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
      
        SpawnMap(APP.CurrentStage);

        _inGameManager = new InGameManager();
        _inGameManager.Init();

        _inGameSceneUI = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");
        _inGameManager.OnCreateCharacter = _inGameSceneUI.SetCharacterToHpBar;
        _inGameManager.OnDieCharacter = _inGameSceneUI.RemoveHpBar;
       
        _bulletManager = new BulletManager();
        _bulletManager.Init();

        APP.GameManager.AddUpdatablePublicManager(_inGameManager);
        APP.GameManager.AddUpdatablePublicManager(_bulletManager);


        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BOSS_DEAD, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PLAYER_DEAD, FailGame);
    }


    private void SuccessGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));

        ItemProto[] prtRewards = RandomHelper.GetRandomThreeItem();
        if (prtRewards == null)
        {
            GameLogger.E("Rewards is Null");
            return;
        }

        _inGameSceneUI.ShowFinishPopup(true, prtRewards);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));
        _inGameSceneUI.ShowFinishPopup(false);
    }

    private void SpawnMap(StageProto currentStage)
    {
        GameObject map = GameObject.FindObjectOfType<Terrain>()?.gameObject;
        if(map == null)
            map = SG.UTIL.Instantiate(currentStage.PrefabPath);

        if (map == null)
        {
            GameLogger.E($"No GameObject {currentStage.PrefabPath}");
            return;
        }
        map.transform.position = new Vector3(-currentStage.Width / 2, -1, -currentStage.Height / 2);
    }

    protected override void Exit()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.BOSS_DEAD);
        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_DEAD);

        APP.GameManager.RemoveUpdatablePublicManager(_inGameManager);
        _inGameManager.FinishManager();
    }

    protected override void Start()
    {
        TimeHelper.AddTimeEvent("ingame-scene", 1, () => 
        { 
            _inGameManager.StartManager();

            _inGameSceneUI.SetPlayer(_inGameManager.GetPlayer());
        });
    }

    protected override void Update()
    {
        _inGameSceneUI.Refresh();
    }

    
}
