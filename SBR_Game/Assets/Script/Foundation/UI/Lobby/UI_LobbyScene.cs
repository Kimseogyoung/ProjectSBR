using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    private List<GameObject> _stageButtonPoints;
    private List<TMP_Text> _stageButtonTexts;
    private List<UI_Star> _stageButtonStars;
    private List<Button> _stageButtons;
    private List<Image> _stageButtonImages;
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

        _stageButtonPoints = BindMany<GameObject>(UIs.Point.ToString());

        foreach(GameObject point in _stageButtonPoints)
        {
            GameObject stageButton = Util.Resource.Instantiate(AppPath.StageButton);
            stageButton.transform.SetParent(point.transform,false);
        }

        _stageButtonTexts = BindMany<TMP_Text>(UIs.StageButtonText.ToString());
        _stageButtonStars = BindMany<UI_Star>(UIs.StageStars.ToString());
        _stageButtons = BindMany<Button>(UIs.StageButton.ToString());
        _stageButtonImages = BindMany<Image>(UIs.StageButtonImage.ToString());
        RefreshStageButton();

        //È®ÀÎ¿ë ÅØ½ºÆ®
        Get<TMP_Text>(UI.EnergyText.ToString()).text = "Energy";
        Get<TMP_Text>(UI.CashText.ToString()).text = "Cash";
        Get<TMP_Text>(UI.GoldText.ToString()).text = "Gold";

        //ÆË¾÷ ¿ÀÇÂ
        Get<Button>(UI.SettingButton.ToString()).
            onClick.AddListener(()=>{APP.UI.ShowPopupUI<UI_SettingPopup>();});
        Get<Button>(UI.InvenButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InvenPopup>(); });
        Get<Button>(UI.ShopButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_ShopPopup>(); });

    }

    private void RefreshStageButton()
    {
        for(int i=0; i< _stageButtons.Count; i++)
        {
            int stageNum = i;
            _stageButtons[i].onClick.AddListener(() => { OnClickStageButton(stageNum); });
            _stageButtonStars[i].SetStarLevel(2);
            _stageButtonTexts[i].text = $"Stage {stageNum}";
            
        }
    }

    private void OnClickStageButton(int stageNum)
    {
        UI_StageConfirmPopup popup =  APP.UI.ShowPopupUI<UI_StageConfirmPopup>();
        popup.SetStageData(stageNum);
        GameLogger.Info("Click Stage {0} Button.", stageNum);
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
        GoldText,

        StageButtonLine,
    }
    enum UIs
    {
        Point,
        StageButton,
        StageButtonImage,
        StageButtonText,
        StageStars
    }
    
}
