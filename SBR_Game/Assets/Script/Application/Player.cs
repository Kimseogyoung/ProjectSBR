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

    public void AddItem(Item item)
    {
        if (Inventory.ContainsKey(item.Prt.Id))
        {
            GameLogger.I($"Add Existed Item Id({item.Prt.Id}) Name({item.Prt.Name}) Amount({item.Amount})");
            Inventory[item.Prt.Id].AddAmount(item.Amount);
            return;
        }

        GameLogger.I($"Add New Item Id({item.Prt.Id}) Name({item.Prt.Name}) Amount({item.Amount})");
        Inventory.Add(item.Prt.Id, item);

        if (item.Prt.Type == EItemType.NORMAL)
        {
            if (!item.Equip())
            {
                return;
            }
            _updateItemIdList.Add(item.Prt.Id);
            Refresh();
        }
    }

    public void EquipItem(Item item)
    {
        if (!item.Equip())
        {
            GameLogger.E("EquipItem Failed");
            return;
        }

        var equipType = (EEquipType)item.Prt.Type;
        if (EquipItemIdDict.ContainsKey(equipType))
        {
            EquipItemIdDict[equipType] = item.Prt.Id;
        }
        else
        {
            EquipItemIdDict.Add(equipType, item.Prt.Id);
        }
        _updateItemIdList.Add(item.Prt.Id);
        Refresh();

    }

}

