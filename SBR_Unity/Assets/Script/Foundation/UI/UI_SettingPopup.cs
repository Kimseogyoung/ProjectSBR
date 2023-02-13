using TMPro;
using UnityEngine.UI;

public class UI_SettingPopup : UI_Popup
{
    private void Awake()
    {
        Bind<Button>(UI.ExitButton.ToString());

        Get<Button>(UI.ExitButton.ToString()).
            onClick.AddListener(() => { ClosePopupUI(); });

    }
    enum UI
    {
        ExitButton
    }
}
