using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Character
{
    public List<Item> Inventory { get; private set; } = new();
    public Dictionary<EEquipType, int> EquipItemDict { get; private set; } = new();

    private List<int> _updateItemIdList = new();

    public void Refresh()
    {
        // TODO : 변경된 아이템 스탯 적용
        for(int i=0; i< _updateItemIdList.Count; i++)
        {
            
        }
    }

    public void AddItem(Item item)
    {
        Inventory.Add(item);

        if(item.Prt.Type == EItemType.NORMAL)
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
            GameLogger.Error("EquipItem Failed");
            return;
        }

        var equipType = (EEquipType)item.Prt.Type;
        if (EquipItemDict.ContainsKey(equipType))
        {
            EquipItemDict[equipType] = item.Prt.Id;
        }
        else
        {
            EquipItemDict.Add(equipType, item.Prt.Id);
        }
        _updateItemIdList.Add(item.Prt.Id);
        Refresh();
        
    }
}
