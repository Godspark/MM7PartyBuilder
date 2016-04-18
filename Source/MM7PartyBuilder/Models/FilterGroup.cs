using System.Collections.Generic;

namespace MM7ClassCreatorWPF.Models
{
    public class FilterGroup
    {
        public List<FilterItem> FilterItems { get; set; }
        public List<ClassSuggestion> ClassSuggestions { get; set; }

        public FilterGroup()
        {
            FilterItems = new List<FilterItem>();
            ClassSuggestions = new List<ClassSuggestion>();
        }        
    }
}
