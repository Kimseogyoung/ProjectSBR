using TMPro;
using UnityEngine.UI;

public class UI_InGamePausePopup : UI_Popup
{
    private void Awake()
    {
        EventQueue.PushEvent<PauseEvent>(EEventActionType.PAUSE, new PauseEvent(true));

        Bind<Button>(UI.PlayButton.ToString()).onClick.AddListener(() => { APP.GAME.InGame.Rule.Notify_Play(true); });
        Bind<Button>(UI.GiveUpButton.ToString()).onClick.AddListener(() => { APP.GAME.InGame.Rule.Notify_GiveUp(); });
        Bind<TMP_Text>(UI.GiveUpText.ToString()).text= "포기하시옹";
    }

    enum UI{
        GiveUpButton,
        GiveUpText,
        PlayButton
    }
}
