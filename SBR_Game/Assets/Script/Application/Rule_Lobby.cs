using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Rule_Lobby : ClassBase
{
    public enum ERuleState
    {
        NONE,
        
        PREPARE,
        FIRST_CREATE_PREPARE, // 초기 플레이어 생성 (Player가 없으면)
        FIRST_PREPARE, //

        INGAME_RESULT_PREPARE,

        PREPARE_COMPLATE, // 초기 로딩 끝

        
        LOBBY_PLAY, //로비 (선택 가능)


        ERROR
    }

    private ERuleState _prevState = ERuleState.NONE;
    private ERuleState _state = ERuleState.NONE;

    protected override bool OnCreate()
    {
        return true;
    }

    protected override void OnDestroy()
    {
    }

    public void StartFirst()
    {
        APP.GAME.Lobby.UI.SetShowHidePanel(true);
        EnterState(ERuleState.PREPARE);
    }


    public void LoadInGame(int stageNum)
    {
        APP.GAME.Player.ChangeCurStage(stageNum);
        _ = APP.SceneManager.ChangeScene("InGameScene");
    }

    private void EnterState(ERuleState ruleState)
    {
        _prevState = _state;
        switch (_prevState)
        {
            case ERuleState.NONE:
                break;
            case ERuleState.FIRST_CREATE_PREPARE:
                break;
            case ERuleState.FIRST_PREPARE:
                break;
            case ERuleState.INGAME_RESULT_PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
                break;
            case ERuleState.LOBBY_PLAY:
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
            case ERuleState.FIRST_CREATE_PREPARE:
                Enter_CreatePrepare();
                break;
            case ERuleState.FIRST_PREPARE:
                Enter_FirstPrepare();
                break;
            case ERuleState.INGAME_RESULT_PREPARE:
                Enter_InGameResultPrepare();
                break;
            case ERuleState.PREPARE_COMPLATE:
                Enter_PrepareComplate();
                break;
            case ERuleState.LOBBY_PLAY:
                break;
            default:
                break;
        }
    }


    public void Update()
    {
        switch (_state)
        {
            case ERuleState.NONE:
                break;
            case ERuleState.FIRST_CREATE_PREPARE:
            case ERuleState.FIRST_PREPARE:
                EnterState(ERuleState.PREPARE_COMPLATE);
                break;
            case ERuleState.INGAME_RESULT_PREPARE:

                break;
            case ERuleState.PREPARE_COMPLATE:
                EnterState(ERuleState.LOBBY_PLAY);
                break;
            case ERuleState.LOBBY_PLAY:
                break;
            case ERuleState.ERROR:
                break;
        }
    }

    private void Enter_Prepare()
    {
        //Player 있으면 FISRT_PREPARE
        if (APP.GAME.HasStageClearInfo())
        {
            EnterState(ERuleState.INGAME_RESULT_PREPARE);
            return;
        }

        if (!APP.GAME.Player.FirstCreate)
        {
            //기존 LocalPlayerPref읽음. 데이터 가공 및 리프레시 필요
            EnterState(ERuleState.FIRST_PREPARE);
            return;
        }
        // 없으면 FIRST_CREATE
        //리프레시 필요//리프레시 필요
        EnterState(ERuleState.FIRST_CREATE_PREPARE);
    }

    private void Enter_PrepareComplate()
    {
        APP.GAME.Player.RefreshAll();
        APP.GAME.Lobby.UI.SetShowHidePanel(false);
        APP.GAME.Lobby.UI.RefreshStageList();
    }

    private void Enter_InGameResultPrepare()
    {
        LOG.I("Lobby.Enter_InGameResultPrepare : 스테이지 변경사항 적용 ");

        // 보상이랑 변경점 가져와서 표시
        StageClearInfo stageClearInfo = APP.GAME.GetStageClearInfo();

        // 다음 스테이지 설정
        StageProto nextStagePrt = ProtoHelper.GetNext(stageClearInfo.ClearedStagePrt);
        APP.GAME.Player.SetStageStar(stageClearInfo.ClearedStagePrt.Id, stageClearInfo.StarCnt);
        APP.GAME.Player.OpenNewStage(nextStagePrt.Id);

        // 스테이지 이동 연출
        APP.GAME.Lobby.UI.ShowStageClearResult(stageClearInfo.ClearedStagePrt, nextStagePrt, stageClearInfo.StarCnt,
            () => EnterState(ERuleState.PREPARE_COMPLATE));

        // 아이템 보상 지급
        stageClearInfo.StageRewardList.ForEach(x => APP.GAME.Player.AddItem(x));

        LOG.W("TODO: 스탯 변경 애니메이션");

        //LOG.W("TODO: 팝업 클릭하면 넘어가기 PREPARE_COMPLATE State로  ");
    }

    private void Enter_FirstPrepare()
    {
        LOG.I("Lobby.Enter_FirstPrepare : 기존 캐릭터 사용하여 게임 첫 접속");
        //기존 LocalPlayerPref읽음. 데이터 가공 및 리프레시 필요
    }

    private void Enter_CreatePrepare()
    {
        LOG.I("Lobby.Enter_CreatePrepare : 게임 첫 접속. 캐릭터 생성 ");
        //리프레시 필요
        APP.GAME.Player.Init();
    }
}

