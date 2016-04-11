using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM7ClassCreatorWPF.Models
{
    public enum NumberOfCharacters
    {
        [Description("None")]
        None,
        [Description("At least 1")]
        AtLeast1,
        [Description("At least 2")]
        AtLeast2,
        [Description("At least 3")]
        AtLeast3,
        [Description("Exactly 1")]
        Exactly1,
        [Description("Exactly 2")]
        Exactly2,
        [Description("Exactly 3")]
        Exactly3,
        [Description("All")]
        All,
    }
}
