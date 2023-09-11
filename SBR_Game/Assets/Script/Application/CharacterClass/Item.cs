using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


public partial class Item
{
    [JsonIgnore]
    public ItemProto Prt { get; set; }

    public int Num { get; set; } 
    public bool IsEquiped { get; set; }
    public int Amount { get; set; }

    public void Refresh(int num)
    {
        Num = num;
        Amount = 1;
        Prt = ProtoHelper.Get<ItemProto, int>(num);
    }

    public void AddAmount(int amount = 1)
    {
        Amount += amount;
    }

    public bool Equip()
    {
        if(Amount <= 0 || Prt.Type >= EItemType.ONCE || IsEquiped )
        {
            LOG.E($"CanNot Equip. TNum({Prt.Id}) ype({Prt.Type}) Amount({Amount}) IsEquip({IsEquiped})");
            return false;
        }
        IsEquiped = true;
        return true;
    }

    public bool Unequip()
    {
        if (Amount <= 0 || Prt.Type >= EItemType.ONCE || !IsEquiped)
        {
            LOG.E($"CanNot UnEquip. Num({Prt.Id}) Type({Prt.Type }) Amount({Amount}) IsEquip({IsEquiped})");
            return false;
        }
        IsEquiped = false;
        return true;
    }

    public bool UseOnce()
    {
        if (Amount <= 0 || Prt.Type < EItemType.ONCE)
        {
            LOG.E($"CanNot Use. Num({Prt.Id}) Type({Prt.Type }) Amount({Amount}) ");
            return false;
        }
        return true;
    }

    public bool UseItemSkill()
    {
        return true;
    }
}
