using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Rule_InGame
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

        GAME_OVER,

        ERROR,
    }

    private ERuleState _prevState;
    private ERuleState _state;

    public void StartFirst()
    {

    }

    private void EnterState(ERuleState ruleState)
    {
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
            case ERuleState.GAME_OVER:
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
            case ERuleState.GAME_OVER:
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
            case ERuleState.GAME_OVER:
                break;
            case ERuleState.ERROR:
                break;
        }
    }
}

