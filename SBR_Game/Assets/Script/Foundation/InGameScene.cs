using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : SceneBase
{
    private InGameManager _characterManager;
    private BulletManager _bulletManager;
    private UI_InGameScene _inGameSceneUI;
    public InGameScene(string sceneName)
    {
        _sceneName = sceneName;
    }

    protected override void Enter()
    {
        _inGameSceneUI = APP.UI.ShowSceneUI<UI_InGameScene>("UI_InGameScene");

        SpawnMap(APP.CurrentStage);

        _characterManager = new InGameManager();
        _characterManager.Init();

        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.ATTACK));
        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.SKILL1));
        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.SKILL2));
        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.SKILL3));
        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.SKILL4));
        _inGameSceneUI.SetSkill(_characterManager.GetPlayer().GetSkill(EInputAction.ULT_SKILL));

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
        _inGameSceneUI.ShowFinishPopup(deadEvent);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));
        _inGameSceneUI.ShowFinishPopup(deadEvent);
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
        EventQueue.RemoveAllEventListener(EEventActionType.BOSS_DEAD);
        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_DEAD);

        APP.GameManager.RemoveUpdatablePublicManager(_characterManager);
        _characterManager.FinishManager();
    }

    protected override void Start()
    {
        TimeHelper.AddTimeEvent(1, () => { _characterManager.StartManager(); });
    }

    protected override void Update()
    {
        _inGameSceneUI.Refresh();
    }

    
}
