public class SkillProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public int Type { get; set; }
    public int property { get; set; }
    public float multiplier { get; set; }
    public int MPCost { get; set; }
    public float CoolTime { get; set; }
    public float radius { get; set; }
    public float arrivalOfTime { get; set; }
}
