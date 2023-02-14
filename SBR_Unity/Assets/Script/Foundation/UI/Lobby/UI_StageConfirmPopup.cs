
using TMPro;

public class UI_StageConfirmPopup : UI_Popup
{
    private void Awake()
    {
        Bind<TMP_Text>(UI.StageText.ToString());
        
    }

    public void SetStageData(int stageNum)
    {
        Get<TMP_Text>(UI.StageText.ToString()).text = $"Stage {stageNum}";
    }

    enum UI
    {
        StageText
    }
}
