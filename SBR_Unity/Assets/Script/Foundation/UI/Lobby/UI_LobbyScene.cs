using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    private void Awake()
    {
        Bind<Button>(UI.TmpButton.ToString());
        Bind<Button>(UI.RecordButton.ToString());
        Bind<Button>(UI.SettingButton.ToString());
        Bind<Button>(UI.EventButton.ToString());
        Bind<Button>(UI.MissionButton.ToString());
        Bind<Button>(UI.InvenButton.ToString());
        Bind<Button>(UI.ShopButton.ToString());
        Bind<Button>(UI.NoticeButton.ToString());
        Bind<TMP_Text>(UI.EnergyText.ToString());
        Bind<TMP_Text>(UI.CashText.ToString());
        Bind<TMP_Text>(UI.GoldText.ToString());


        Get<TMP_Text>(UI.EnergyText.ToString()).text = "Energy";
        Get<TMP_Text>(UI.CashText.ToString()).text = "Cash";
        Get<TMP_Text>(UI.GoldText.ToString()).text = "Gold";

        Get<Button>(UI.SettingButton.ToString()).
            onClick.AddListener(()=>{APP.UI.ShowPopupUI<UI_SettingPopup>();});
        Get<Button>(UI.InvenButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InvenPopup>(); });
        Get<Button>(UI.ShopButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_ShopPopup>(); });

    }
    enum UI
    {
        TmpButton,
        RecordButton,
        SettingButton,
        EventButton,
        MissionButton,
        InvenButton,
        ShopButton,
        NoticeButton,

        EnergyText,
        CashText,
        GoldText
    }
    
}
