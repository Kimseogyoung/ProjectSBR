using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : SceneBase
{
    private Rule_InGame _rule;
    private InGameManager _inGameManager;
    private BulletManager _bulletManager;
    private UI_InGameScene _ui;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override bool Enter()
    {
      
        SpawnMap(APP.CurrentStage);

        _inGameManager = new InGameManager();
        _inGameManager.Init();

        _ui = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");
        _inGameManager.OnCreateCharacter = _ui.SetCharacterToHpBar;
        _inGameManager.OnDieCharacter = _ui.RemoveHpBar;
       
        _bulletManager = new BulletManager();
        _bulletManager.Init();

        APP.GAME.AddUpdatablePublicManager(_inGameManager);
        APP.GAME.AddUpdatablePublicManager(_bulletManager);


        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BOSS_DEAD, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PLAYER_DEAD, FailGame);

        return true;
    }


    private void SuccessGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));

        ItemProto[] prtRewards = RandomHelper.GetRandomThreeItem();
        if (prtRewards == null)
        {
            LOG.E("Rewards is Null");
            return;
        }

        _ui.ShowFinishPopup(true, prtRewards);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));
        _ui.ShowFinishPopup(false);
    }

    private void SpawnMap(StageProto currentStage)
    {
        GameObject map = GameObject.FindObjectOfType<Terrain>()?.gameObject;
        if(map == null)
            map = UTIL.Instantiate(currentStage.PrefabPath);

        if (map == null)
        {
            LOG.E($"No GameObject {currentStage.PrefabPath}");
            return;
        }
        map.transform.position = new Vector3(-currentStage.Width / 2, -1, -currentStage.Height / 2);
    }

    protected override void Exit()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.BOSS_DEAD);
        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_DEAD);

        APP.GAME.RemoveUpdatablePublicManager(_inGameManager);
        _inGameManager.Destroy();
    }

    protected override void Start()
    {
        TimeHelper.AddTimeEvent("ingame-scene", 1, () => 
        { 
            _inGameManager.StartManager();

            _ui.SetPlayer(_inGameManager.GetPlayer());
        });
    }

    protected override void Update()
    {
        _ui.Refresh();
    }

    
}
