using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Player
{

    public void AddItem(Item item)
    {
        if (Inventory.ContainsKey(item.Prt.Id))
        {
            LOG.I($"Add Existed Item Id({item.Prt.Id}) Name({item.Prt.Name}) Amount({item.Amount})");
            Inventory[item.Prt.Id].AddAmount(item.Amount);
            return;
        }

        LOG.I($"Add New Item Id({item.Prt.Id}) Name({item.Prt.Name}) Amount({item.Amount})");
        Inventory.Add(item.Prt.Id, item);

        if (item.Prt.Type == EItemType.NORMAL)
        {
            if (!item.Equip())
            {
                return;
            }
            _updateItemIdList.Add(item.Prt.Id);
            RefreshStat();
        }
    }

    public void EquipItem(Item item)
    {
        if (!item.Equip())
        {
            LOG.E("EquipItem Failed");
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
        RefreshStat();

    }
}

