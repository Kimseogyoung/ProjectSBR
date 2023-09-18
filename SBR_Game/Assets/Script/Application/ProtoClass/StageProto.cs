public class StageProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public int OrderNum { get; set; }
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string PrefabPath { get; set; }
    public int LimitTime { get; set; }
    public int StarTimeA { get; set; }
    public int StarTimeB { get; set; }
}
