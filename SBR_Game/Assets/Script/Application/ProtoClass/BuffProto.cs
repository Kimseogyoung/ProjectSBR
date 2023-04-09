public class BuffProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Class { get; set; }
    public EBuffType BuffType { get; set; }
    public float TickInterval { get; set; }
    public float Duration { get; set; }
    public int HPInc { get; set; }
    public int MPInc { get; set; }
    public int SPDInc { get; set; }
    public int ATKSPDInc { get; set; }
    public int ATKInc { get; set; }
    public int DEFInc { get; set; }
    public int CDRInc { get; set; }
    public int HPGENInc { get; set; }
    public int CRTInc { get; set; }
    public int RANGEInc { get; set; }
    public int DRAINInc { get; set; }
    public float HPPer { get; set; }
    public float MPPer { get; set; }
    public float SPDPer { get; set; }
    public float ATKSPDPer { get; set; }
    public float ATKPer { get; set; }
    public float DEFPer { get; set; }
    public float CDRPer { get; set; }
    public float HPGENPer { get; set; }
    public float CRTPer { get; set; }
    public float RANGEPer { get; set; }
    public float DRAINPer { get; set; }
}
