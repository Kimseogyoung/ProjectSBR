public class ItemProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public string IconImg { get; set; }
    public EItemType Type { get; set; }
    public string Desc { get; set; }
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
    public int CRT { get; set; }
    public int RANGE { get; set; }
    public int DRAIN { get; set; }
    public float HPPer { get; set; }
    public float MPPer { get; set; }
    public float SPDPer { get; set; }
    public float ATKSPDPer { get; set; }
    public float ATKPer { get; set; }
    public float MATKPer { get; set; }
    public float DEFPer { get; set; }
    public float CDRPer { get; set; }
    public float HPGENPer { get; set; }
    public float CRTPer { get; set; }
    public float RANGEPer { get; set; }
    public float DRAINPer { get; set; }
    public int IncreasedElementProperty { get; set; }
    public float IncreasedElementPropertyMultiply { get; set; }
    public int IncreasedAttackProperty { get; set; }
    public float IncreasedAttackPropertyMultiply { get; set; }
    public int IncreasedSkillId { get; set; }
    public int ItemSkillId { get; set; }
}
