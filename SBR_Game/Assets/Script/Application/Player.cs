using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Player
{
    public int Energy { get; set; }
    public float Gold { get; set; }
    public float Cash { get; private set; }
    public int TopOpenStageNum { get;private set; }
    public Dictionary<int, int> StageStarDict { get; private set; } = new();
    public Dictionary<int, Item> Inventory { get; private set; } = new();
    public List<int> EquipOnceItemIdList { get; private set; } = new();
    public Dictionary<EEquipType, int> EquipItemIdDict { get; private set; } = new();

    private List<int> _updateItemIdList = new();


    public void Create()
    {
        Energy = APP.GameConf.MaxEnergy;
        Gold = 0;
        Cash = 0;
        TopOpenStageNum = ProtoHelper.GetUsingIndex<StageProto>(0).Id;

        foreach(StageProto prtStage in ProtoHelper.GetEnumerable<StageProto>())
        {
            StageStarDict.Add(prtStage.Id, 0);
        }

#if DEBUG

        for(int i=0; i< APP.DebugConf.StartItemNumList.Count; i++)
        {
            Item item = new Item(APP.DebugConf.StartItemNumList[i]);
            AddItem(item);
        }
#endif


    }

    public void Destroy()
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

    public void Refresh()
    {
        // TODO : 변경된 아이템 스탯 적용
        for (int i = 0; i < _updateItemIdList.Count; i++)
        {

        }
    }
}

