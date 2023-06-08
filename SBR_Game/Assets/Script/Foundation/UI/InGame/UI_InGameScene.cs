using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameScene : UI_Scene
{
    private ObjectPool<HpBar> _hpBarPool;

    private Dictionary<EInputAction, SkillButton> _skillButtonDict = new();

    protected override void InitImp()
    {
        Bind<Button>(UI.PauseButton.ToString()).onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InGamePausePopup>(); });
        Bind<Image>(UI.PlayerIconImage.ToString());
        Bind<Image>(UI.StageIconImage.ToString());
        Bind<TMP_Text>(UI.StageNumText.ToString());
        Bind<TMP_Text>(UI.StageNameText.ToString());
        Bind<Slider>(UI.PlayerHPSlider.ToString());
        Bind<Slider>(UI.PlayerMPSlider.ToString());
        Bind<Slider>(UI.BossHPSlider.ToString());

        BindComponent<JoyStickPad>(UI.JoyStickPad.ToString()).Init();

        Bind<GameObject>(UI.GameHUD.ToString());
        Bind<GameObject>(UI.NormalEnemyHPPanel.ToString());
        Bind<BuffView>(UI.BuffView.ToString());

        BindSkillButton();

        _hpBarPool = new ObjectPool<HpBar>(10, Bind<GameObject>(UI.HpHUD.ToString()).transform, Resources.Load<GameObject>("UI/Object/HpSlider"));

        EventQueue.AddEventListener<HPEvent>(EEventActionType.PLAYER_HP_CHANGE, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.ENEMY_HP_CHANGE, UpdateHpBar);
    }

    public void SetPlayer(Character player)
    {
        var playerBuffView = Get<BuffView>(UI.BuffView.ToString());
        playerBuffView.Init();

        playerBuffView.AttachCharacter(player);
        SetSkillToButton(player.GetSkill(EInputAction.ATTACK));
        SetSkillToButton(player.GetSkill(EInputAction.SKILL1));
        SetSkillToButton(player.GetSkill(EInputAction.SKILL2));
        SetSkillToButton(player.GetSkill(EInputAction.SKILL3));
        SetSkillToButton(player.GetSkill(EInputAction.SKILL4));
        SetSkillToButton(player.GetSkill(EInputAction.ULT_SKILL));
    }

    public void ShowFinishPopup(bool isSuccess, ItemProto[] prtRewards = null)
    {

        UI_InGameFinishPopup popup = APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
        if (!isSuccess)
        {
            popup.ShowFailUI();
            return;
        }

        popup.ShowRewardUI(prtRewards);
    }

    public void SetSkillToButton(SkillBase skill)
    {
        _skillButtonDict[skill.MatchedInputAction].SetSkill(skill);
    }

    public void SetCharacterToHpBar(Character character)
    {
        var hpBar = _hpBarPool.Dequeue();
        
        hpBar.component.Init();
        hpBar.component.SetCharacter(character);
    }

    public void RemoveHpBar(int characterCreateNum)
    {
        var hpBarIndex = _hpBarPool.GetActiveList().FindIndex(x => x.component.CharacterCreateNum == characterCreateNum);
        if(hpBarIndex < 0)
        {
            GameLogger.Error($"Can not Found HpBar. CreateNum({characterCreateNum})");
            return;
        }

        _hpBarPool.Enqueue(_hpBarPool.GetActiveList()[hpBarIndex].gameObject);
    }

    public void Refresh()
    {
        Get<BuffView>(UI.BuffView.ToString()).Refresh();
        Get<JoyStickPad>(UI.JoyStickPad.ToString()).Refresh();
        RefreshSkillBtn();
        RefreshHpBar();
    }

    private void BindSkillButton()
    {
        var atkBtn = BindComponent<SkillButton>(UI.AttackButton.ToString());
        atkBtn.Init();
        _skillButtonDict.Add(EInputAction.ATTACK, atkBtn);

        var skillBtnList = BindManyComponent<SkillButton>(UI.SkillButton.ToString());
        for (int i = 0; i < skillBtnList.Count; i++)
        {
            skillBtnList[i].Init();
            _skillButtonDict.Add(EInputAction.SKILL1 + i, skillBtnList[i]);
        }
    }

    private void RefreshSkillBtn()
    {
        foreach(var skillButton in _skillButtonDict.Values)
        {
            skillButton.Refresh();
        }
    }

    private void RefreshHpBar()
    {
        var activeHpBarList = _hpBarPool.GetActiveList();
        for(int i=0; i< activeHpBarList.Count; i++)
        {
            if (activeHpBarList[i].gameObject.activeSelf && activeHpBarList[i].component.IsFar())
            {
                activeHpBarList[i].gameObject.SetActive(false);
            }
            else if(!activeHpBarList[i].gameObject.activeSelf && !activeHpBarList[i].component.IsFar())
            {
                activeHpBarList[i].gameObject.SetActive(true);
            }

            activeHpBarList[i].component.Refresh();
        }
    }

    private void UpdateHpBar(HPEvent evt)
    {
        switch (evt.eventActionType)
        {
            case EEventActionType.PLAYER_HP_CHANGE:
                GameLogger.Info("플레이어 체력감소" + evt.DeltaHP);
                var slider = Get<Slider>(UI.PlayerHPSlider.ToString());
                slider.maxValue = evt.FullHP;
                slider.value = evt.CurHP;
                break;
            case EEventActionType.ENEMY_HP_CHANGE:
                {
                    if (APP.InGame.GetBoss().Id == evt.CharacterId)
                    {
                        var bossSlider = Get<Slider>(UI.BossHPSlider.ToString());
                        bossSlider.maxValue = evt.FullHP;
                        bossSlider.value = evt.CurHP;
                        return;
                    }


                    //TODO : 쫄 몹 처리
                }
                break;

            default:
                GameLogger.Info("UpdateHpBar : {0}", evt.eventActionType);
                break;
        }

    }

    protected override void OnDestroyed()
    {
        _skillButtonDict.Clear();

        if(_hpBarPool != null)
        {
            _hpBarPool.Destroy();
        }

        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_HP_CHANGE);
        EventQueue.RemoveAllEventListener(EEventActionType.ENEMY_HP_CHANGE);
    }

    enum UI
    {
        //Top Panel
        PauseButton,

        //Image
        PlayerIconImage,
        StageIconImage,

        //Text
        StageNumText,
        StageNameText,

        //Slider
        PlayerHPSlider,
        PlayerMPSlider,
        BossHPSlider,

        //GameObject
        NormalEnemyHPPanel,
        GameHUD,
        HpHUD,

        //Button
        AttackButton,
        SkillButton,

        //Pad
        JoyStickPad,
       
        BuffView,
        BuffContent,

    }

}
