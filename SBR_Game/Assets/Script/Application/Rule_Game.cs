using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Rule_Game
{
    public enum ERuleState
    {
        NONE,
        PREPARE, // 초기 로딩 (플레이어 로드)
        PREPARE_COMPLATE, // 초기 로딩 끝

        OUTGAME_PREPARE,
        OUTGAME_START,
        OUTGAME_PLAY, //로비 (선택 가능)

        INGAME_PREPARE, // 맵.캐릭터 로드
        INGAME_START, // 시작
        INGAME_PLAY, // 실제 조작 가능 상태
        INGAME_STOP, //멈춤
        INGAME_FINISH, //종료 (공략 성공, 실패)  -> 실패시  OUT_GAME_PREPARE        
        INGAME_FINISH_REWARD, // 보상 정산 시작(적용 시작) -> OUT_GAME_PREPARE        

        GAME_OVER,
        RESTART,

        ERROR
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
            case ERuleState.PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
                break;
            case ERuleState.OUTGAME_PREPARE:
                break;
            case ERuleState.OUTGAME_START:
                break;
            case ERuleState.OUTGAME_PLAY:
                break;
            case ERuleState.INGAME_PREPARE:
                break;
            case ERuleState.INGAME_START:
                break;
            case ERuleState.INGAME_PLAY:
                break;
            case ERuleState.INGAME_STOP:
                break;
            case ERuleState.INGAME_FINISH:
                break;
            case ERuleState.INGAME_FINISH_REWARD:
                break;
            case ERuleState.GAME_OVER:
                break;
            case ERuleState.RESTART:
                break;
            case ERuleState.ERROR:
                break;
        }

        _state = ruleState;
        GameLogger.I($"EnterState : {_state}");

        switch (ruleState)
        {
            case ERuleState.PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
                break;
            case ERuleState.OUTGAME_PREPARE:
                break;
            case ERuleState.OUTGAME_START:
                break;
            case ERuleState.OUTGAME_PLAY:
                break;
            case ERuleState.INGAME_PREPARE:
                break;
            case ERuleState.INGAME_START:
                break;
            case ERuleState.INGAME_PLAY:
                break;
            case ERuleState.INGAME_STOP:
                break;
            case ERuleState.INGAME_FINISH:
                break;
            case ERuleState.INGAME_FINISH_REWARD:
                break;
            case ERuleState.GAME_OVER:
                break;
            case ERuleState.RESTART:
                break;
            case ERuleState.ERROR:
                GameLogger.E("ErrorGameState");
                break;
            default:
                break;
        }
    }


    public void Update()
    {
        switch (_state)
        {
            case ERuleState.PREPARE:
                break;
            case ERuleState.PREPARE_COMPLATE:
                break;
            case ERuleState.OUTGAME_PREPARE:
                break;
            case ERuleState.OUTGAME_START:
                break;
            case ERuleState.OUTGAME_PLAY:
                break;
            case ERuleState.INGAME_PREPARE:
                break;
            case ERuleState.INGAME_START:
                break;
            case ERuleState.INGAME_PLAY:
                break;
            case ERuleState.INGAME_STOP:
                break;
            case ERuleState.INGAME_FINISH:
                break;
            case ERuleState.INGAME_FINISH_REWARD:
                break;
            case ERuleState.GAME_OVER:
                break;
            case ERuleState.RESTART:
                break;
            case ERuleState.ERROR:
                return;
        }
    }
}

