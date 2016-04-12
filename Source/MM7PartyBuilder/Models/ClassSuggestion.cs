using System;
using System.Collections.Generic;
using System.Linq;

namespace MM7ClassCreatorWPF.Models
{
    public class ClassSuggestion : IEquatable<ClassSuggestion>
    {
        private List<CharacterClass> _classSuggestions;
        public List<CharacterClass> CharacterClasses
        {
            get { return _classSuggestions; }
            private set { _classSuggestions = value; }
        }
        
        public ClassSuggestion()
        {
            CharacterClasses = new List<CharacterClass>();
        }

        public ClassSuggestion(List<CharacterClass> classes)
        {
            CharacterClasses = classes;
        }


        public void Add(CharacterClass cClass)
        {
            CharacterClasses.Add(cClass);
        }

        public void Order()
        {
            CharacterClasses = CharacterClasses.OrderBy(p => p).ToList();
        }

        public bool Equals(ClassSuggestion other)
        {
            CharacterClasses = CharacterClasses.OrderBy(p => p).ToList();
            other.CharacterClasses = other.CharacterClasses.OrderBy(p => p).ToList();

            if (Enumerable.SequenceEqual(CharacterClasses, other.CharacterClasses))
            {
                return true;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            var code = (int)CharacterClasses[0] * (int)CharacterClasses[1] * (int)CharacterClasses[2] * (int)CharacterClasses[3];
            return code;
        }

    }
}
