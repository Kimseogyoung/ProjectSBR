using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Player : ClassBase
{
    public bool FirstCreate { get; set; } = true;
    public bool HasStageFinishResult { get; set; }
    public int Energy { get; set; }
    public float Gold { get; set; }
    public float Cash { get; set; }
    public int TopOpenStageNum { get; set; }
    public Dictionary<int, int> StageStarDict { get; set; } = new();
    public Dictionary<int, Item> Inventory { get; set; } = new();
    public List<int> EquipOnceItemIdList { get; set; } = new();
    public Dictionary<EEquipType, int> EquipItemIdDict { get; set; } = new();

}

