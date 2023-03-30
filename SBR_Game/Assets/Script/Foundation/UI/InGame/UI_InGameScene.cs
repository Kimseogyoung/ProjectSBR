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

        EventQueue.AddEventListener<HPEvent>(EEventActionType.PLAYER_HP_CHANGE, UpdateHpBar);
        EventQueue.AddEventListener<HPEvent>(EEventActionType.ENEMY_HP_CHANGE, UpdateHpBar);
    }

    private void Update()
    {
        
    }

    public void ShowFinishPopup(CharacterDeadEvent characterDeadEvent)
    {
        APP.UI.ShowPopupUI<UI_InGameFinishPopup>();
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
                    if(APP.InGame.GetBoss().Id == evt.CharacterId)
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

    private void OnDestroy()
    {
        EventQueue.RemoveAllEventListener(EEventActionType.PLAYER_HP_CHANGE);
        EventQueue.RemoveAllEventListener(EEventActionType.ENEMY_HP_CHANGE);
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
