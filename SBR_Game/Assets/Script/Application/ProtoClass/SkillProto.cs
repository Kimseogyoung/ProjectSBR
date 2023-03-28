public class SkillProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public string ClassType { get; set; }
    public int Type { get; set; }
    public int property { get; set; }
    public ECharacterTeamType TargetTeam { get; set; }
    public EHitSKillType HitTargetType { get; set; }
    public int TargetCnt { get; set; }
    public EHitTargetSelectType HitTargetSelectType { get; set; }
    public EHitStyleType HitStyleType { get; set; }
    public string ProjectilePrefab { get; set; }
    public float CoolTime { get; set; }
    public bool CanCancel { get; set; }
    public float CanNotMoveTime { get; set; }
    public float ApplyPointTime { get; set; }
    public int Cnt { get; set; }
    public float PeriodTime { get; set; }
    public float StartTime { get; set; }
    public float DurationTime { get; set; }
    public EHitShapeType HitShapeType { get; set; }
    public float MultiplierValue { get; set; }
    public float FixedValue { get; set; }
    public int MPCost { get; set; }
    public float Angle { get; set; }
    public float Range { get; set; }
    public float HitWidth { get; set; }
    public float HitHeight { get; set; }
    public float PushPower { get; set; }
    public float Speed { get; set; }
    public int ParentSkillId { get; set; }
    public bool IsNormalAttack { get; set; }
}
