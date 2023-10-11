using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Player : ClassBase
{
    private StageProto _curStagePrt = null; 
    private List<int> _updateItemIdList = new();

    protected override bool OnCreate()
    {
#if DEBUG
        for (int i = 0; i < APP.DebugConf.StartItemNumList.Count; i++)
        {
            if (ProtoHelper.TryGet<ItemProto>(APP.DebugConf.StartItemNumList[i], out var itemPrt)) 
            {
                if (!Item.Create(out Item item, itemPrt))
                {
                    LOG.E($"Failed to Create Item Num({itemPrt.Id})");
                    return false;
                }
                AddItem(item);
            }
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
        TopOpenStageNum = ProtoHelper.GetByIndex<StageProto>(0).Id;

        FirstCreate = false;
    }

    public void RefreshAll() //TODO:  위치 확인 , UI보다 시점 빨라야 함.
    {
        RefreshItemList();
        RefreshStat();
        RefreshStageStar();
    }

    public void RefreshItemList()
    {
        foreach (Item item in Inventory.Values)
        {
            item.Refresh();
        }
    }

    public void RefreshStat()
    {
        // TODO : 변경된 아이템 스탯 적용
        LOG.W("TODO : 변경된 아이템 스탯 적용");
        for (int i = 0; i < _updateItemIdList.Count; i++)
        {

        }
    }
    
    public void RefreshStageStar()
    {

        foreach (StageProto prtStage in ProtoHelper.GetEnumerable<StageProto>())
        {
            if (!StageStarDict.ContainsKey(prtStage.Id))
                StageStarDict.Add(prtStage.Id, 0);
        }
    }

    public void SetStageStar(int stageId, int starCnt)
    {
        if (StageStarDict.ContainsKey(stageId))
        {
            if (StageStarDict[stageId] < starCnt)
                StageStarDict[stageId] = starCnt;
        }
        else
        {
            StageStarDict.Add(stageId, starCnt);
        }
    }

    public void OpenNewStage(int stageId)
    {
        if (stageId < TopOpenStageNum)
            return;
        TopOpenStageNum = stageId;
    }

    public void ChangeCurStage(int stageId)
    {
        _curStagePrt = ProtoHelper.Get<StageProto>(stageId);
    }


    public StageProto GetCurStagePrt()
    {
        if(_curStagePrt == null)
        {
            LOG.W("CurStagePrt Is Null");
            return ProtoHelper.GetByIndex<StageProto>(0);
        }
        return _curStagePrt;
    }
}

