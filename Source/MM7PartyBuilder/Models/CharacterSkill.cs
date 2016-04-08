using System.ComponentModel;

namespace MM7ClassCreatorWPF.Models
{
    public enum CharacterSkill
    {
        [Description("Air Magic")]
        MagicAir,
        [Description("Fire Magic")]
        MagicFire,
        [Description("Water Magic")]
        MagicWater,
        [Description("Earth Magic")]
        MagicEarth,
        [Description("Body Magic")]
        MagicBody,
        [Description("Spirit Magic")]
        MagicSpirit,
        [Description("Mind Magic")]
        MagicMind,
        [Description("Light Magic")]
        MagicLight,
        [Description("Dark Magic")]
        MagicDark,
        [Description("Sword")]
        WeaponSword,
        [Description("Axe")]
        WeaponAxe,
        [Description("Staff")]
        WeaponStaff,
        [Description("Spear")]
        WeaponSpear,
        [Description("Dagger")]
        WeaponDagger,
        [Description("Bow")]
        WeaponBow,
        [Description("Mace")]
        WeaponMace,
        [Description("Unarmed")]
        WeaponUnarmed,
        [Description("Leather Armor")]
        ArmorLeather,
        [Description("Chain Armor")]
        ArmorChain,
        [Description("Plate Armor")]
        ArmorPlate,
        [Description("Shield")]
        ArmorShield,
        [Description("Dodging")]
        ArmorDodge,
        [Description("Disarm Trap")]
        MiscDisarm,
        [Description("Perception")]
        MiscPerception,
        [Description("Merchant")]
        MiscMerchant,
        [Description("Learning")]
        MiscLearning,
        [Description("Meditation")]
        MiscMeditation,
        [Description("Body Building")]
        MiscBodyBuilding,
        [Description("Identify Item")]
        MiscIDItem,
        [Description("Repair Item")]
        MiscRepair,
        [Description("Identify Monster")]
        MiscIDMonster,
        [Description("Arms Master")]
        MiscArmsMaster,
        [Description("Stealing")]
        MiscStealing,
        [Description("Alchemy")]
        MiscAlchemy
    }
}
