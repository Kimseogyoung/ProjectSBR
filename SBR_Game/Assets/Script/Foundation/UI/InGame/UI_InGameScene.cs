﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameScene : UI_Scene
{
    private ObjectPool<HpBar> _hpBarPool;
    private List<HpBar> _activeHpBarList = new List<HpBar>();

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

        var atkBtn = BindComponent<SkillButton>(UI.AttackButton.ToString());
        atkBtn.Init();
        _skillButtonDict.Add(EInputAction.ATTACK, atkBtn);

        var skillBtnList = BindManyComponent<SkillButton>(UI.SkillButton.ToString());
        for (int i = 0; i < skillBtnList.Count; i++)
        {
            skillBtnList[i].Init();
            _skillButtonDict.Add(EInputAction.SKILL1 + i, skillBtnList[i]);
        }

        var playerBuffView = Bind<BuffView>(UI.BuffView.ToString());
        playerBuffView.Init();
        playerBuffView.AttachCharacter(APP.InGame.GetPlayer());

        _hpBarPool = new ObjectPool<HpBar>(10, Bind<GameObject>(UI.HpHUD.ToString()).transform, Resources.Load<GameObject>("UI/Object/HpSlider"));

        EventQueue.AddEventListener<HPEvent>(EEventActionType.PLAYER_HP_CHANGE, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.ENEMY_HP_CHANGE, UpdateHpBar);
    }

    public void ShowFinishPopup(CharacterDeadEvent characterDeadEvent)
    {
        APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
    }

    public void SetSkillToButton(SkillBase skill)
    {
        _skillButtonDict[skill.MatchedInputAction].SetSkill(skill);
    }

    public void SetCharacterToHpBar(CharacterBase character)
    {
        var hpBar = _hpBarPool.Dequeue();
        
        hpBar.component.Init();
        hpBar.component.SetCharacter(character);

        _activeHpBarList.Add(hpBar.component);
    }

    public void RemoveHpBar(int characterCreateNum)
    {
        var hpBar = _activeHpBarList.Find(x => x.CharacterCreateNum == characterCreateNum);
        if(hpBar == null)
        {
            GameLogger.Error($"Can not Found HpBar. CreateNum({characterCreateNum})");
            return;
        }

        _activeHpBarList.Remove(hpBar);
        _hpBarPool.Enqueue(hpBar.gameObject);
    }

    public void Refresh()
    {
        Get<BuffView>(UI.BuffView.ToString()).Refresh();
        Get<JoyStickPad>(UI.JoyStickPad.ToString()).Refresh();
        RefreshSkillBtn();
        RefreshHpBar();
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
        for(int i=0; i<_activeHpBarList.Count; i++)
        {
            if (_activeHpBarList[i].gameObject.activeSelf && _activeHpBarList[i].IsFar())
            {
                _activeHpBarList[i].gameObject.SetActive(false);
            }
            else if(!_activeHpBarList[i].gameObject.activeSelf && !_activeHpBarList[i].IsFar())
            {
                _activeHpBarList[i].gameObject.SetActive(true);
            }

            _activeHpBarList[i].Refresh();
        }
    }

    private void UpdateHpBar(HPEvent evt)
    {

        float value = evt.CurHP / evt.FullHP;

        switch (evt.eventActionType)
        {
            case EEventActionType.PLAYER_HP_CHANGE:
                Get<Slider>(UI.PlayerHPSlider.ToString()).value = value;
                break;
            case EEventActionType.ENEMY_HP_CHANGE:
                {
                    if (APP.InGame.GetBoss().Id == evt.CharacterId)
                    {
                        Get<Slider>(UI.BossHPSlider.ToString()).value = value;
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
        _activeHpBarList.Clear();
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
