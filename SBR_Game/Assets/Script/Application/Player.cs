using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Player : ClassBase
{
    private List<int> _stageRewardItemIdList = new();
    private List<int> _updateItemIdList = new();

    protected override bool OnCreate()
    {
#if DEBUG
        for (int i = 0; i < APP.DebugConf.StartItemNumList.Count; i++)
        {
            Item item = new Item();
            item.Refresh(APP.DebugConf.StartItemNumList[i]);
            AddItem(item);
        }
#endif
        FirstCreate = true;
        return true;
    }

    protected override void OnDestroy()
    {
        Energy = 0;
        Gold = 0;
        Cash = 0;
        TopOpenStageNum = 0;

        Inventory.Clear();
        EquipOnceItemIdList.Clear();
        EquipItemIdDict.Clear();
        _updateItemIdList.Clear();
        StageStarDict.Clear();
    }

    public void Init()
    {
        Energy = APP.GameConf.MaxEnergy;
        Gold = 0;
        Cash = 0;
        TopOpenStageNum = ProtoHelper.GetUsingIndex<StageProto>(0).Id;

        FirstCreate = false;
    }

    public void RefreshAll() //TODO:  위치 확인 , UI보다 시점 빨라야 함.
    {
        RefreshStat();
        RefreshStageStar();
    }

    public void RefreshStat()
    {
        // TODO : 변경된 아이템 스탯 적용
        for (int i = 0; i < _updateItemIdList.Count; i++)
        {

        }
    }
    
    public void RefreshStageStar()
    {

        foreach (StageProto prtStage in ProtoHelper.GetEnumerable<StageProto>())
        {
            StageStarDict.Add(prtStage.Id, 0);
        }
    }

    public void SetStageStar(int stageId, int starCnt)
    {
        if (StageStarDict.ContainsKey(stageId))
            StageStarDict[stageId] = starCnt;
        else
            StageStarDict.Add(stageId, starCnt);
    }
}

