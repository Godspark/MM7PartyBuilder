namespace MM7ClassCreatorWPF.Models
{
    public class FilterItem
    {
        public bool IsFirstElementInList { get; set; }
        public CharacterSkill Skill { get; set; }
        public MasteryLevel Mastery { get; set; }
        public NumberOfCharacters NumberOfCharacters { get; set; }
        public AndOr AndOr { get; set; }
    }
}
