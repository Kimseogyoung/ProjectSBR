using TMPro;
using UnityEngine.UI;

public class UI_InGamePausePopup : UI_Popup
{
    private void Awake()
    {
        Bind<Button>(UI.GiveUpButton.ToString()).onClick.AddListener(() => { GiveUp(); });
        Bind<TMP_Text>(UI.GiveUpText.ToString()).text= "포기하시옹";
    }

    private void GiveUp()
    {
        APP.SceneManager.ChangeScene("LobbyScene");
    }

    enum UI{
        GiveUpButton,
        GiveUpText
    }
}
