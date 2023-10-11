using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


public partial class Item : ClassBase
{
    public static bool Create(out Item item, ItemProto prt)
    {
        item = new Item();
        item.Num = prt.Id;
        if (!item.OnCreate())
        {
            Destroy(ref item);
            return false;
        }
        return true;
    }

    [JsonIgnore]
    public ItemProto Prt { get; set; }

    public int Num { get; set; } 
    public bool IsEquiped { get; set; }
    public int Amount { get; set; }


    protected override bool OnCreate()
    {
        Amount = 1;
        Refresh();
        return true;
    }

    protected override void OnDestroy()
    {
    }

    public void Refresh()
    {
        Prt = ProtoHelper.Get<ItemProto>(Num);
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
