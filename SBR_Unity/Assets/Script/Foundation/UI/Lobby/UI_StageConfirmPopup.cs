
using TMPro;
using UnityEngine.UI;

public class UI_StageConfirmPopup : UI_Popup
{
    private int _stageNum = 0;
    private void Awake()
    {
        Bind<TMP_Text>(UI.StageText.ToString());
        Bind<Button>(UI.StartButton.ToString());
    }

    public void SetStageData(int stageNum)
    {
        _stageNum = stageNum;
        Get<TMP_Text>(UI.StageText.ToString()).text = $"Stage {_stageNum}";
        Get<Button>(UI.StartButton.ToString()).onClick.AddListener(() => { OnClickStageStartButton(); });
    }

    private void OnClickStageStartButton()
    {
        APP.SceneManager.ChangeScene("InGameScene");
    }
    enum UI
    {
        StageText,
        StartButton
    }
}
