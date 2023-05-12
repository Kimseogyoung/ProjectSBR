using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameScene : UI_Scene
{
    private Dictionary<EInputAction, SkillButton> _skillButtonDict = new();

    protected override void InitImp()
    {
        GameLogger.Strong("Init!!!!!!!!!!!!!!!!!!!");

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

        EventQueue.AddEventListener<HPEvent>(EEventActionType.PLAYER_HP_CHANGE, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.ENEMY_HP_CHANGE, UpdateHpBar);
    }

    public void ShowFinishPopup(CharacterDeadEvent characterDeadEvent)
    {
        APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
    }

    public void SetSkill(SkillBase skill)
    {
        _skillButtonDict[skill.MatchedInputAction].SetSkill(skill);
    }

    public void Refresh()
    {
        Get<JoyStickPad>(UI.JoyStickPad.ToString()).Refresh();
        RefreshSkillBtn();
    }

    private void RefreshSkillBtn()
    {
        foreach(var skillButton in _skillButtonDict.Values)
        {
            skillButton.Refresh();
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

        //Button
        AttackButton,
        SkillButton,

        //Pad
        JoyStickPad,


    }

}
