public class ItemProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public EItemType Type { get; set; }
    public string ElementSynergy { get; set; }
    public string StatSynergy { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public int SPD { get; set; }
    public int ATKSPD { get; set; }
    public int ATK { get; set; }
    public int MATK { get; set; }
    public int DEF { get; set; }
    public int CDR { get; set; }
    public int HPGEN { get; set; }
    public int CRI { get; set; }
    public float HPPercent { get; set; }
    public float MPPercent { get; set; }
    public float SPDPercent { get; set; }
    public float ATKSPDPercent { get; set; }
    public float ATKPercent { get; set; }
    public float MATKPercent { get; set; }
    public float DEFPercent { get; set; }
    public float CDRPercent { get; set; }
    public float HPGENPercent { get; set; }
    public float CRIPercent { get; set; }
    public int IncreasedElementProperty { get; set; }
    public float IncreasedElementPropertyMultiply { get; set; }
    public int IncreasedAttackProperty { get; set; }
    public float IncreasedAttackPropertyMultiply { get; set; }
    public int IncreasedSkillId { get; set; }
    public int ItemSkillId { get; set; }
}
