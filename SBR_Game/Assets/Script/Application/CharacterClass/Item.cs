using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Item
{
    public ItemProto Prt { get; set; }
    public bool IsEquiped { get; private set; }

    public int Amount { get; private set; }

    public Item(int num, int amount = 1)
    {
        Amount = amount;
        Prt = ProtoHelper.Get<ItemProto, int>(num);
    }

    public bool Equip()
    {
        if(Amount <= 0 || Prt.Type >= EItemType.NORMAL || IsEquiped )
        {
            GameLogger.Error($"CanNot Equip. TNum({Prt.Id}) ype({Prt.Type}) Amount({Amount}) IsEquip({IsEquiped})");
            return false;
        }
        IsEquiped = true;
        return true;
    }

    public bool Unequip()
    {
        if (Amount <= 0 || Prt.Type >= EItemType.NORMAL || !IsEquiped)
        {
            GameLogger.Error($"CanNot UnEquip. Num({Prt.Id}) Type({Prt.Type }) Amount({Amount}) IsEquip({IsEquiped})");
            return false;
        }
        IsEquiped = false;
        return true;
    }

    public bool UseOnce()
    {
        if (Amount <= 0 || Prt.Type < EItemType.ONCE)
        {
            GameLogger.Error($"CanNot Use. Num({Prt.Id}) Type({Prt.Type }) Amount({Amount}) ");
            return false;
        }
        return true;
    }

    public bool UseItemSkill()
    {
        return true;
    }
}
