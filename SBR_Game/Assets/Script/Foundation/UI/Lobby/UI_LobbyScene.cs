using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    private List<GameObject> _stageButtonPoints;
    private List<TMP_Text> _stageButtonTexts;
    private List<UI_Star> _stageButtonStars;
    private List<int> _stageButtonNumList;
    private List<Button> _stageButtons;


    private GameObject _effRootGO;
    private AnimBase _starChangeAnimEff;
    private TMP_Text _starStageClearText;
    private TMP_Text _starStageOpenText;
    private UI ui;

    protected override void OnDestroyed()
    {

    }

    protected override void InitImp()
    {
        base.InitImp();
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
            GameObject stageButton = UTIL.Instantiate(AppPath.StageButton);
            stageButton.transform.SetParent(point.transform,false);
        }

        _stageButtonTexts = BindMany<TMP_Text>(UIs.StageButtonText.ToString());
        _stageButtonStars = BindMany<UI_Star>(UIs.StageStars.ToString());
        _stageButtons = BindMany<Button>(UIs.StageButton.ToString());
        InitStageButtons();

        //확인용 텍스트
        Get<TMP_Text>(UI.EnergyText.ToString()).text = "Energy";
        Get<TMP_Text>(UI.CashText.ToString()).text = "Cash";
        Get<TMP_Text>(UI.GoldText.ToString()).text = "Gold";

        _effRootGO = Bind<GameObject>(UI.EffRoot.ToString());
        GameObject starChangeRes = UTIL.LoadRes<GameObject>("Effect/Lobby/StarChangeEff");
        if (!AnimBase.CreateInstance(out _starChangeAnimEff, starChangeRes, _effRootGO, "StarChangeEff"))
        {
            LOG.E("Failed Create StageChangeEff");
        }
        if (!UTIL.TryGetComponent(out _starStageClearText, _starChangeAnimEff.gameObject, "StageClearText"))
        {
            LOG.E("Failed Get StarChangeText");
        }

        if (!UTIL.TryGetComponent(out _starStageOpenText, _starChangeAnimEff.gameObject, "StageOpenText"))
        {
            LOG.E("Failed Get StarChangeText");
        }

        //팝업 오픈
        Get<Button>(UI.SettingButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_SettingPopup>(); });
        Get<Button>(UI.InvenButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InvenPopup>(); });
        Get<Button>(UI.ShopButton.ToString()).
            onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_ShopPopup>(); });

        RefreshStageButton();
    }

    public void ShowStageClearResult(StageProto clearedStagePrt, StageProto nextStagePrt, int starCnt, Action finishAction)
    {
        _starStageClearText.text = clearedStagePrt.Name + "스테이지 완료";
        _starChangeAnimEff.PlayAnim($"StarChange{starCnt}", ()=>
        {
            _starChangeAnimEff.PlayAnim("StageChange", finishAction);
            _starStageOpenText.text = nextStagePrt.Name + "스테이지 해금";
            LOG.I("다음 스테이지 해금 연출 시작");
        });
        LOG.W($"TODO: 별 {starCnt}개 {clearedStagePrt.Id}스테이지 획득 연출 추가");

        LOG.W($"TODO: {nextStagePrt.Id}스테이지 오픈 연출 추가");

    }

    private void InitStageButtons()
    {
        _stageButtonNumList = new();
        
        for (int i = 0; i < _stageButtons.Count; i++)
        {
            int stageNum = ProtoHelper.GetByIndex<StageProto>(i).Id;
            _stageButtons[i].onClick.AddListener(() => { OnClickStageButton(stageNum); });
            _stageButtonTexts[i].text = $"Stage {stageNum}";
            _stageButtonNumList.Add(stageNum);
        }
    }

    private void RefreshStageButton()
    {
        int playerTopOpenStage = APP.GAME.Player.TopOpenStageNum;
        Dictionary<int, int> playerStageStarDict = APP.GAME.Player.StageStarDict;
        for(int i=0; i< _stageButtons.Count; i++)
        {
            int stageNum = _stageButtonNumList[i];
            int stageStar = playerStageStarDict[stageNum];
            if (playerTopOpenStage < stageNum)
            {
                _stageButtons[i].interactable = false;
            }
            else
            {
                _stageButtons[i].interactable = true;
            }
            _stageButtonStars[i].SetStarLevel(stageStar);       
        }
    }

    private void OnClickStageButton(int stageNum)
    {
        UI_StageConfirmPopup popup =  APP.UI.ShowPopupUI<UI_StageConfirmPopup>();
        popup.SetStageData(stageNum);
        LOG.I("Click Stage {0} Button.", stageNum);
    }

    enum UI
    {
        EffRoot,

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
