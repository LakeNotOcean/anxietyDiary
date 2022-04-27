using System.ComponentModel;

namespace Domain.Enums
{
    public enum RequestsEnum : int
    {
        [Description("BecomeDoctor")]
        BecomeDoctor = 1,
        [Description("ViewAsDoctor")]
        ViewAsDoctor = 2,
        [Description("InviteDoctor")]
        InviteDoctor = 3,
    }
}