using MM7ClassCreatorWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MM7ClassCreatorWPF
{
    public class ClassSuggestionGenerator
    {
        private Dictionary<SkillClassKey, MasteryLevel> _masteryTable;

        public ClassSuggestionGenerator()
        {
            Initialize();
        }

        public List<ClassSuggestion> SuggestClasses(IEnumerable<FilterItem> searchFilters)
        {
            var allSuggestions = GetAllSuggestions();
            var resultingClassSuggestions = GetAllSuggestions();
            foreach (var filter in searchFilters.Where(p => p.AndOr == AndOr.And))
            {
                var classesNeeded = ClassesMeetFilter(filter);
                resultingClassSuggestions = FindSuggestions(resultingClassSuggestions, filter.NumberOfCharacters, classesNeeded);
            }
            foreach (var filter in searchFilters.Where(p => p.AndOr == AndOr.Or))
            {
                var classesNeeded = ClassesMeetFilter(filter);
                resultingClassSuggestions.AddRange(FindSuggestions(allSuggestions, filter.NumberOfCharacters, classesNeeded));
                resultingClassSuggestions = resultingClassSuggestions.Distinct().ToList();
            }

            return resultingClassSuggestions;
        }

        private List<ClassSuggestion> FindSuggestions(List<ClassSuggestion> possibleSuggestions, NumberOfCharacters numberOfCharacters, List<CharacterClass> classesNeeded)
        {
            List<ClassSuggestion> suggestions;
            switch (numberOfCharacters)
            {
                case NumberOfCharacters.None:
                    suggestions = possibleSuggestions.Where(p => !p.CharacterClasses.Any(q => classesNeeded.Contains(q))).ToList();
                    break;
                case NumberOfCharacters.AtLeast1:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Any(q => classesNeeded.Contains(q))).ToList();
                    break;
                case NumberOfCharacters.AtLeast2:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Count(q => classesNeeded.Contains(q)) >= 2).ToList();
                    break;
                case NumberOfCharacters.AtLeast3:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Count(q => classesNeeded.Contains(q)) >= 3).ToList();
                    break;
                case NumberOfCharacters.Exactly1:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Count(q => classesNeeded.Contains(q)) == 1).ToList();
                    break;
                case NumberOfCharacters.Exactly2:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Count(q => classesNeeded.Contains(q)) == 2).ToList();
                    break;
                case NumberOfCharacters.Exactly3:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.Count(q => classesNeeded.Contains(q)) == 3).ToList();
                    break;
                case NumberOfCharacters.All:
                    suggestions = possibleSuggestions.Where(p => p.CharacterClasses.All(q => classesNeeded.Contains(q))).ToList();
                    break;
                default:
                    suggestions = new List<ClassSuggestion>();
                    break;
            }
            return suggestions;
        }

        private List<ClassSuggestion> GetAllSuggestions()
        {
            var allClasses = Enum.GetValues(typeof(CharacterClass)).Cast<CharacterClass>();
            var allClassSuggestions = new List<ClassSuggestion>();

            foreach (var class1 in allClasses)
            {
                foreach (var class2 in allClasses)
                {
                    foreach (var class3 in allClasses)
                    {
                        foreach (var class4 in allClasses)
                        {
                            var suggestion = new ClassSuggestion(new List<CharacterClass>() { class1, class2, class3, class4 });
                            allClassSuggestions.Add(suggestion);
                        }
                    }
                }
            }
            allClassSuggestions = allClassSuggestions.Distinct().ToList();
            return allClassSuggestions;
        }

        private List<CharacterClass> ClassesMeetFilter(FilterItem filter)
        {
            var classesCovered = new List<CharacterClass>();
            foreach(var cClass in Enum.GetValues(typeof(CharacterClass)).Cast<CharacterClass>())
            {
                var classMastery = _masteryTable[new SkillClassKey(filter.Skill, cClass)];
                var classIsCovered = IsMasteryCovered(filter.Mastery, classMastery);
                if (classIsCovered)
                    classesCovered.Add(cClass);
            }
            return classesCovered;
        }

        private bool IsMasteryCovered(MasteryLevel mastery, MasteryLevel cover)
        {
            if (cover == MasteryLevel.GrandMaster)
                return true;

            var returnValue = false;
            switch(mastery)
            {
                case MasteryLevel.None:
                    returnValue = true;
                    break;
                case MasteryLevel.Basic:
                    if (cover != MasteryLevel.None)
                        returnValue = true;
                    break;
                case MasteryLevel.Expert:
                    if (cover != MasteryLevel.None && cover != MasteryLevel.Basic)
                        returnValue = true;
                    break;
                case MasteryLevel.Master:
                    if (cover == MasteryLevel.Master || cover == MasteryLevel.GrandMaster)
                        returnValue = true;
                    break;
                case MasteryLevel.GrandMaster:
                    if (cover == MasteryLevel.GrandMaster)
                        returnValue = true;
                    break;                   
            }
            return returnValue;
        }

        private void Initialize()
        {
            _masteryTable = new Dictionary<SkillClassKey, MasteryLevel>();

            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Knight), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Knight), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Knight), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Knight), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Knight), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Knight), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Knight), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Knight), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Knight), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Knight), MasteryLevel.None);
                       

            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Thief), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Thief), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Thief), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Thief), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Thief), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Thief), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Thief), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Thief), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Thief), MasteryLevel.Expert);


            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Monk), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Monk), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Monk), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Monk), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Monk), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Monk), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Monk), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Monk), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Monk), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Monk), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Monk), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Monk), MasteryLevel.None);


            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Paladin), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Paladin), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Paladin), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Paladin), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Paladin), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Paladin), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Paladin), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Paladin), MasteryLevel.None);

            
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Archer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Archer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Archer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Archer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Archer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Archer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Archer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Archer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Archer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Archer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Archer), MasteryLevel.None);
            

            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Ranger), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Ranger), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Ranger), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Ranger), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Ranger), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Ranger), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Ranger), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Ranger), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Ranger), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Ranger), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Ranger), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Ranger), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Ranger), MasteryLevel.Basic);
            

            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Cleric), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Cleric), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Cleric), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Cleric), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Cleric), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Cleric), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Cleric), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Cleric), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Cleric), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Cleric), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Cleric), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Cleric), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Cleric), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Cleric), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Cleric), MasteryLevel.Expert);


            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Druid), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Druid), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Druid), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Druid), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Druid), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Druid), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Druid), MasteryLevel.GrandMaster);


            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicAir, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicFire, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicWater, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicEarth, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicBody, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicSpirit, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicMind, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicLight, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MagicDark, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSword, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponAxe, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponStaff, CharacterClass.Sorcerer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponSpear, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponDagger, CharacterClass.Sorcerer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponBow, CharacterClass.Sorcerer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponMace, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.WeaponUnarmed, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorLeather, CharacterClass.Sorcerer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorChain, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorPlate, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorShield, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.ArmorDodge, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscDisarm, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscPerception, CharacterClass.Sorcerer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMerchant, CharacterClass.Sorcerer), MasteryLevel.Basic);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscLearning, CharacterClass.Sorcerer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscMeditation, CharacterClass.Sorcerer), MasteryLevel.Master);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscBodyBuilding, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDItem, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscRepair, CharacterClass.Sorcerer), MasteryLevel.Expert);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscIDMonster, CharacterClass.Sorcerer), MasteryLevel.GrandMaster);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscArmsMaster, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscStealing, CharacterClass.Sorcerer), MasteryLevel.None);
            _masteryTable.Add(new SkillClassKey(CharacterSkill.MiscAlchemy, CharacterClass.Sorcerer), MasteryLevel.Master);
        }
    }
}
