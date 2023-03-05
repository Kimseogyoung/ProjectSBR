public class StageProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string PrefabPath { get; set; }
    public int LimitTime { get; set; }
}
