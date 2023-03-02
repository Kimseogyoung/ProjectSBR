using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProtoItem
{
    public int Idx { get; set; }
}

public class StageProto : ProtoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string PrefabPath { get; set; }
    public int LimitTime { get; set; }

    public StageProto()
    {

    }
}

