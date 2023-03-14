using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameFinishPopup : UI_Popup
{
    private void Awake()
    {
        Bind<Button>(UI.GoLobbyButton.ToString()).onClick.AddListener(() =>
        {
            EventQueue.PushEvent<PauseEvent>(EEventActionType.PLAY, new PauseEvent(false));
            APP.SceneManager.ChangeScene("LobbyScene");
        });
        Bind<TMP_Text>(UI.GameResultText.ToString());
    }
    enum UI
    {
        GameResultText,
        GoLobbyButton
    }
}
