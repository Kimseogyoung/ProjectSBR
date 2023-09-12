using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Rule_InGame : ClassBase
{
    public enum ERuleState
    {
        NONE,
        PREPARE, // 초기 로딩 (플레이어 로드)
        PREPARE_COMPLATE, // 초기 로딩 끝

        START,
        PLAY, //이 안에 공격 패턴 State
        STOP,

        RESTART,
        
        FINISH_SUCCESS,
        FINISH_FAILURE,

        REWARD,// 보상 정산

        ERROR,
    }

    private ERuleState _prevState;
    private ERuleState _state;
    private List<int> _rewardItemIdList = new();
    private int _stageStarCnt = 1;

    private InGameManager _inGameManager;
    private BulletManager _bulletManager;
    private float _stateTime = 3.0f;

    private const int c_startWaitTime = 3;

    public StageProto StagePrt { get; private set; }
    

    protected override bool OnCreate()
    {
        return true;
    }

    protected override void OnDestroy()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.BOSS_DEAD);
        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_DEAD);

        APP.GAME.RemoveUpdatablePublicManager(_inGameManager);

        _inGameManager.Destroy();
    }

    public void StartFirst()
    {
        StagePrt = APP.GAME.Player.GetCurStagePrt();

        _inGameManager = new InGameManager();
        _inGameManager.Init();

        _bulletManager = new BulletManager();
        _bulletManager.Init();


        APP.GAME.AddUpdatablePublicManager(_inGameManager);
        APP.GAME.AddUpdatablePublicManager(_bulletManager);


        EnterState(ERuleState.PREPARE);
    }
    
    public void Notify_GameSuccess()
    {
        EnterState(ERuleState.FINISH_SUCCESS);
    }

    public void Notify_GameFailure()
    {
        EnterState(ERuleState.FINISH_FAILURE);
    }

    public void Notify_Reward(List<int> rewardItemList = null)
    {
        if(rewardItemList == null)
        {
            // 실패인 경우 null;
            
        }
        EnterState(ERuleState.REWARD);
    }

    private void EnterState(ERuleState ruleState)
    {
        _stateTime = 0;
        _prevState = _state;
        switch (_prevState)
        {
            case ERuleState.NONE:
                break;
            case ERuleState.PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
                break;
            case ERuleState.START:
                break;
            case ERuleState.PLAY:
                break;
            case ERuleState.STOP:
                break;
            case ERuleState.RESTART:
                break;
            case ERuleState.FINISH_SUCCESS:
                break;
            case ERuleState.FINISH_FAILURE:
                break;
            case ERuleState.REWARD:
                break;
            case ERuleState.ERROR:
                break;
        }

        _state = ruleState;
        LOG.I($"EnterState : {_state}");

        switch (ruleState)
        {

            case ERuleState.ERROR:
                LOG.E("ErrorGameState");
                break;
            case ERuleState.NONE:
                break;
            case ERuleState.PREPARE:
                Enter_Prepare();
                break;
            case ERuleState.PREPARE_COMPLATE:
                Enter_PrepareComplate();
                break;
            case ERuleState.START:
                Enter_Start();
                break;
            case ERuleState.PLAY:
                break;
            case ERuleState.STOP:
                break;
            case ERuleState.RESTART:
                Enter_Restart();
                break;
            case ERuleState.FINISH_SUCCESS:
                break;
            case ERuleState.FINISH_FAILURE:
                break;
            case ERuleState.REWARD:
                break;
            default:
                break;
        }
    }


    public void Update()
    {
        _stateTime += Time.fixedDeltaTime;
        switch (_state)
        {
            case ERuleState.NONE:
                break;
            case ERuleState.PREPARE:
                if(_stateTime > c_startWaitTime)
                    EnterState(ERuleState.PREPARE_COMPLATE);
                break;
            case ERuleState.PREPARE_COMPLATE:
                EnterState(ERuleState.START);
                break;
            case ERuleState.START:
                EnterState(ERuleState.PLAY);
                break;
            case ERuleState.PLAY:
                break;
            case ERuleState.STOP:
                break;
            case ERuleState.RESTART:
                break;
            case ERuleState.FINISH_SUCCESS:
                Enter_Finish(true);
                break;
            case ERuleState.FINISH_FAILURE:
                Enter_Finish(false);
                break;
            case ERuleState.REWARD:
                break;
            case ERuleState.ERROR:
                break;
        }
    }


    private void Enter_Prepare()
    {
        // 맵 로드
        SpawnMap();

        // 유닛 로드
        _inGameManager.SpawnUnit();
    }

    private void SpawnMap()
    {
        GameObject map = GameObject.FindObjectOfType<Terrain>()?.gameObject;
        if (map == null)
            map = UTIL.Instantiate(StagePrt.PrefabPath);

        if (map == null)
        {
            LOG.E($"No GameObject {StagePrt.PrefabPath}");
            return;
        }
        map.transform.position = new Vector3(-StagePrt.Width / 2, -1, -StagePrt.Height / 2);
    }

    private void Enter_PrepareComplate()
    {
        APP.GAME.InGame.UI.SetPlayer(_inGameManager.GetPlayer());
    }

    private void Enter_Start()
    {
        // 움직이기 시작
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BOSS_DEAD, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PLAYER_DEAD, FailGame);

        _inGameManager.StartUnit();

    }

    private void Enter_Restart()
    {
        // 보스 초기화하고 처음부터 시작
    }

    private void Enter_Finish(bool isSuccess)
    {
        // 끝난 시간 확인해서 별 계산.
        _stageStarCnt = 3;
    }

    private async void Enter_Reward()
    {

        APP.GAME.Player.SetStageStar(StagePrt.Id, _stageStarCnt);

        // 리워드 아이템 넣기
        for (int i = 0; i < _rewardItemIdList.Count; i++)
            APP.GAME.Player.AddRewardItem(_rewardItemIdList[i]);

        // 진행 상황 저장


        // 일시 정지 해제
        EventQueue.PushEvent(EEventActionType.PLAY, new PauseEvent(false));
        await APP.SceneManager.ChangeScene("LobbyScene");

        
    }

    private void SuccessGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent(EEventActionType.PAUSE, new PauseEvent(true));

        ItemProto[] prtRewards = RandomHelper.GetRandomThreeItem();
        if (prtRewards == null)
        {
            LOG.E("Rewards is Null");
            return;
        }

        APP.GAME.InGame.UI.ShowFinishPopup(true, prtRewards);
    }

    private void FailGame(CharacterDeadEvent deadEvent)
    {
        EventQueue.PushEvent(EEventActionType.PAUSE, new PauseEvent(true));
        APP.GAME.InGame.UI.ShowFinishPopup(false);
    }


}

