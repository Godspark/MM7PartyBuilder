using System.ComponentModel;

namespace MM7ClassCreatorWPF.Models
{
    public enum NumericFilter
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
