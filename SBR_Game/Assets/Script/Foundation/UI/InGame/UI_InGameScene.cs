using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameScene : UI_Scene
{
    private void Awake()
    {
        GameLogger.Info("hihi");
        Bind<Button>(UI.PauseButton.ToString()).onClick.AddListener(() => { APP.UI.ShowPopupUI<UI_InGamePausePopup>(); });
        Bind<Image>(UI.PlayerIconImage.ToString());
        Bind<Image>(UI.StageIconImage.ToString());
        Bind<TMP_Text>(UI.StageNumText.ToString());
        Bind<TMP_Text>(UI.StageNameText.ToString());
        Bind<Slider>(UI.PlayerHPSlider.ToString());
        Bind<Slider>(UI.PlayerMPSlider.ToString());
        Bind<Slider>(UI.BossHPSlider.ToString());
        Bind<GameObject>(UI.GameHUD.ToString());
        Bind<GameObject>(UI.NormalEnemyHPPanel.ToString());

        EventQueue.AddEventListener<HPEvent>(EEventActionType.PlayerHpChange, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.BossHpChange, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.ZzolHpChange, UpdateHpBar);

        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.BossDead, SuccessGame);
        EventQueue.AddEventListener<CharacterDeadEvent>(EEventActionType.PlayerDead, FailGame);
    }

    private void Update()
    {
        
    }

    private void SuccessGame(CharacterDeadEvent characterDeadEvent)
    {
        APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
    }

    private void FailGame(CharacterDeadEvent characterDeadEvent)
    {
        APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
    }


    private void UpdateHpBar(HPEvent evt)
    {

        float value = evt.CurHP / evt.FullHP;

        switch (evt.eventActionType)
        {
            case EEventActionType.PlayerHpChange:
                Get<Slider>(UI.PlayerHPSlider.ToString()).value = value;
                break;
            case EEventActionType.BossHpChange:
                Get<Slider>(UI.BossHPSlider.ToString()).value = value;
                break;
            case EEventActionType.ZzolHpChange:
                //Get<Slider>(UI.PlayerHPSlider.ToString()).value = value;
                break;
            default:
                GameLogger.Info("UpdateHpBar : {0}", evt.eventActionType);
                break;
        }

    }

    private void OnDestroy()
    {
        EventQueue.RemoveEventListener<HPEvent>(EEventActionType.PlayerHpChange, UpdateHpBar);
        EventQueue.RemoveEventListener<HPEvent>(EEventActionType.BossHpChange, UpdateHpBar);
        EventQueue.RemoveEventListener<HPEvent>(EEventActionType.ZzolHpChange, UpdateHpBar);

        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.BossDead, SuccessGame);
        EventQueue.RemoveEventListener<CharacterDeadEvent>(EEventActionType.PlayerDead, FailGame);
    }

    enum UI{
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
    }
}
