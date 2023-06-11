using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGameSuccessPanel : UI_Panel
{
    private TMP_Text _stageText;
    private TMP_Text _rewardItemNameText;
    private TMP_Text _rewardItemDetailText;
    private List<Image> _rewardSlotImageList = new();
    private List<Button> _rewardSlotButtonList = new();
    private int _currentSelectReward = -1;
    private Color _unselectedColor = new Color(0.8f,0.8f,0.8f);
    private ItemProto[] rewardItemList = new ItemProto[3];

    private void Awake()
    {
        Bind<Button>(UI.GoLobbyButton.ToString()).onClick.AddListener(() =>
        {
            EventQueue.PushEvent<PauseEvent>(EEventActionType.PLAY, new PauseEvent(false));
            APP.SceneManager.ChangeScene("LobbyScene");
        });
        Bind<TMP_Text>(UI.GameResultText.ToString()).text = "Cleared!";

        _stageText = Bind<TMP_Text>(UI.StageText.ToString());
        _rewardItemNameText = Bind<TMP_Text>(UI.RewardItemNameText.ToString());
        _rewardItemDetailText = Bind<TMP_Text>(UI.RewardItemDetailText.ToString());
        _rewardItemNameText.text = "";
        _rewardItemDetailText.text = "";

        var slotList = BindMany<GameObject>(UI.RewardSlot.ToString());
        for ( int i=0; i< slotList.Count; i++ )
        {
            var rewardSlot = slotList[i];
            var idx = i;
            var button = rewardSlot.GetComponentInChildren<Button>();

            _rewardSlotImageList.Add(rewardSlot.GetComponentInChildren<Image>());
            _rewardSlotButtonList.Add(button);
            button.onClick.AddListener(() =>
                {
                    _currentSelectReward = idx;
                    UpdateSelectedReward();
                    GameLogger.Info($"Select RewardItem ({_currentSelectReward})");
                });
        } 
    }

    public void ShowRewardUI(ItemProto[] prtRewards)
    {
        rewardItemList = prtRewards;
        _stageText.text = $"you cleared stage {APP.CurrentStage}\nchoose reword.";
        UpdateSelectedReward();
    }


    private void UpdateSelectedReward()
    {
        if(_currentSelectReward == -1)
        {
            for (int i = 0; i < _rewardSlotImageList.Count; i++)
            {
                _rewardSlotImageList[i].color = Color.white;
            }
            _rewardItemNameText.text = "";
            _rewardItemDetailText.text = "";
            return;
        }

        for(int i=0; i< _rewardSlotImageList.Count; i++)
        {
            if(i == _currentSelectReward)
            {
                _rewardSlotImageList[i].color = Color.white;
            }
            else
            {
                _rewardSlotImageList[i].color = _unselectedColor;
            }
        }

        _rewardItemNameText.text = ProtoHelper.Get<ItemProto, int>(rewardItemList[_currentSelectReward].Id).Name;
        _rewardItemDetailText.text = ProtoHelper.Get<ItemProto, int>(rewardItemList[_currentSelectReward].Id).Desc;
    }

    protected override void OnDestroyed()
    {
        _rewardSlotImageList = null;
        _rewardSlotButtonList = null;
    }

    enum UI
    {
        GameResultText,
        StageText,
        RewardSlot,
        RewardItemNameText,
        RewardItemDetailText,
        GoLobbyButton
    }
}
