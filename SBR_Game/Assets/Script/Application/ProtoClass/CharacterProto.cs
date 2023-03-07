public class CharacterProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public float SPD { get; set; }
    public float ATKSPD { get; set; }
    public float ATK { get; set; }
    public float MATK { get; set; }
    public float DEF { get; set; }
    public float CDR { get; set; }
    public float HPGEN { get; set; }
    public float CRT { get; set; }
    public float DRAIN { get; set; }
}
