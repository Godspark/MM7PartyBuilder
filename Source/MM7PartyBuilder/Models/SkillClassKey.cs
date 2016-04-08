namespace MM7ClassCreatorWPF.Models
{
    public struct SkillClassKey
    {
        public readonly CharacterSkill CSkill;
        public readonly CharacterClass CClass;

        public SkillClassKey(CharacterSkill cSkill, CharacterClass cClass)
        {
            CSkill = cSkill;
            CClass = cClass;
        }
    }
}
