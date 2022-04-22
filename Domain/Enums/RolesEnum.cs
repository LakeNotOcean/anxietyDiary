using System.ComponentModel;

namespace Domain.Enums
{
    public enum RolesEnum : int
    {
        [Description("Guest")]
        Guest = 1,
        [Description("Patient")]
        Patient = 2,
        [Description("Doctor")]
        Doctor = 3,
        [Description("Administrator")]
        Administrator = 4,

        [Description("Moderator")]
        Moderator = 5,

        [Description("Banned")]
        Banned = 6,
    }
}