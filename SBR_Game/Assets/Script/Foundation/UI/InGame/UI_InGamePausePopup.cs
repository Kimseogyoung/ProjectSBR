using TMPro;
using UnityEngine.UI;

public class UI_InGamePausePopup : UI_Popup
{
    private void Awake()
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));

        Bind<Button>(UI.PlayButton.ToString()).onClick.AddListener(() => { 
            EventQueue.PushEvent<PauseEvent>(EEventActionType.PLAY, new PauseEvent(false));
            ClosePopupUI();
        });
        Bind<Button>(UI.GiveUpButton.ToString()).onClick.AddListener(() => { GiveUp(); });
        Bind<TMP_Text>(UI.GiveUpText.ToString()).text= "포기하시옹";
    }

    private void GiveUp()
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PLAY, new PauseEvent(false));
        APP.SceneManager.ChangeScene("LobbyScene");
    }

    enum UI{
        GiveUpButton,
        GiveUpText,
        PlayButton
    }
}
