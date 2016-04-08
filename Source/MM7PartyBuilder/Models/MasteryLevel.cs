using System.ComponentModel;

namespace MM7ClassCreatorWPF.Models
{
    public enum MasteryLevel
    {
        [Description("None")]
        None,
        [Description("Basic")]
        Basic,
        [Description("Expert")]
        Expert,
        [Description("Master")]
        Master,
        [Description("GrandMaster")]
        GrandMaster
    }
}
