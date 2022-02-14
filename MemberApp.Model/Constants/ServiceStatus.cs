using System.ComponentModel;

namespace MemberApp.Model.Constants
{
    public enum ServiceStatus
    {
        [Description("InService")]
        InService,

        [Description("Resigned")]
        Resigned,

        [Description("Retired")]
        Retired,

        [Description("Dismissed")]
        Dismissed,

        [Description("CDM")]
        CDM,

        [Description("Absence")]
        Absence,

        [Description("Casualty")]
        Casualty,

        [Description("Death")]
        Death,
    }
}
