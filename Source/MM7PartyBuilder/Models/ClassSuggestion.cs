using System;
using System.Collections.Generic;
using System.Linq;

namespace MM7ClassCreatorWPF.Models
{
    public class ClassSuggestion : IEquatable<ClassSuggestion>
    {
        private List<CharacterClass> _classSuggestions;
        public List<CharacterClass> ClassSuggestions
        {
            get { return _classSuggestions; }
            private set { _classSuggestions = value; }
        }

        public ClassSuggestion()
        {
            ClassSuggestions = new List<CharacterClass>();
        }

        public ClassSuggestion(List<CharacterClass> classes)
        {
            ClassSuggestions = classes;
        }


        public void Add(CharacterClass cClass)
        {
            ClassSuggestions.Add(cClass);
        }

        public void Order()
        {
            ClassSuggestions = ClassSuggestions.OrderBy(p => p).ToList();
        }

        public bool Equals(ClassSuggestion other)
        {
            ClassSuggestions = ClassSuggestions.OrderBy(p => p).ToList();
            other.ClassSuggestions = other.ClassSuggestions.OrderBy(p => p).ToList();

            if (Enumerable.SequenceEqual(ClassSuggestions, other.ClassSuggestions))
            {
                return true;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            var code = (int)ClassSuggestions[0] * (int)ClassSuggestions[1] * (int)ClassSuggestions[2] * (int)ClassSuggestions[3];
            return code;
        }

    }
}
