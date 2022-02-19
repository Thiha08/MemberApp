using System.ComponentModel;

namespace MemberApp.Model.Constants
{
    public enum ServiceStatus
    {
        [Description("InService")]
        InService = 1,

        [Description("Retired")]
        Retired,

        [Description("Resigned")]
        Resigned,

        [Description("Dismissed")]
        Dismissed,

        [Description("Absence")]
        Absence,

        [Description("CDM")]
        CDM,

        [Description("Casualty")]
        Casualty,

        [Description("Death")]
        Death,
    }
}
