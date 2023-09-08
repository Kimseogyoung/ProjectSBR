using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameFailedPanel : UI_Panel
{
    private void Awake()
    {
        Bind<Button>(UI.GoLobbyButton.ToString()).onClick.AddListener(() =>
        {
            EventQueue.PushEvent<PauseEvent>(EEventActionType.PLAY, new PauseEvent(false));
            _ = APP.SceneManager.ChangeScene("LobbyScene");
        });
        Bind<TMP_Text>(UI.GameResultText.ToString()).text = "Failed";
        Bind<TMP_Text>(UI.StageText.ToString());
    }

    public void ShowFailUI()
    {
        Get<TMP_Text>(UI.StageText.ToString()).text = $"you reached stage {APP.GAME.Player.TopOpenStageNum}";
        LOG.I($"TODO :  Make Fail PopUp");
    }

    enum UI
    {
        GameResultText,
        StageText,
        GoLobbyButton
    }
}
