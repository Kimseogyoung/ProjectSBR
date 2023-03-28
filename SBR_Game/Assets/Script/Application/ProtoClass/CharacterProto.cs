public class CharacterProto : ProtoItem
{
    public int Id { get; set; }// pk 
    public string Name { get; set; }
    public ECharacterTeamType TeamType { get; set; }
    public ECharacterType Type { get; set; }
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
    public float RANGE { get; set; }
    public float DRAIN { get; set; }
    public string Prefab { get; set; }
    public int AttackSkill { get; set; }
    public int DodgeSkill { get; set; }
    public int Skill1 { get; set; }
    public int Skill2 { get; set; }
    public int Skill3 { get; set; }
    public int UltSkill { get; set; }
}
