using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

    private ERuleState _prevState;
    private ERuleState _state;

    public override bool OnCreate()
    {
        return true;
    }

    public override void OnDestroy()
    {

    }

    public void StartFirst()
    {
        EnterState(ERuleState.PREPARE);
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
                break;
            case ERuleState.FIRST_PREPARE:
                break;
            case ERuleState.INGAME_RESULT_PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
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
    }

    private void Enter_Prepare()
    {
        //Player 있으면 FISRT_PREPARE

        // 없으면 FIRST_CREATE
        // 보상 대기 상태면 INGAME_RESULT_PREPARE
    }
}

