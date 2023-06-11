using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameFinishPopup : UI_Popup
{
    private UI_InGameSuccessPanel _successPanel;
    private UI_InGameFailedPanel _failedPanel;
    private void Awake()
    {
        _successPanel = BindComponent<UI_InGameSuccessPanel>(UI.SuccessPanel.ToString());
        _failedPanel = BindComponent<UI_InGameFailedPanel>(UI.FailedPanel.ToString());

        _successPanel.gameObject.SetActive(false);
        _failedPanel.gameObject.SetActive(false);
    }

    public void ShowRewardUI(ItemProto[] prtRewards)
    {
        _failedPanel.gameObject.SetActive(false);
        _successPanel.gameObject.SetActive(true);
        _successPanel.ShowRewardUI(prtRewards); 
    }

    public void ShowFailUI()
    {
        _failedPanel.gameObject.SetActive(true);
        _successPanel.gameObject.SetActive(false);
        _failedPanel.ShowFailUI();
    }

    enum UI
    {
        FailedPanel,
        SuccessPanel
    }
}
