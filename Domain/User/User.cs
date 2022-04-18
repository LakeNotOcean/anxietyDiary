using System.ComponentModel;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.User
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        [IndexColumn(IsUnique = true)]
        public string UserName { set; get; }
        [Required]
        [ForeignKey("Role")]
        public int RoleId { set; get; }
        public Role Role { get; set; }
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string FirstName { set; get; }
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string SecondName { set; get; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string Email { set; get; }

        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.passwordMaxLength)]
        public string PasswordHash { get; set; }

        [InverseProperty("Patient")]
        public virtual ICollection<LastUserView> Patients { get; set; }

        [InverseProperty("Doctor")]
        public virtual ICollection<LastUserView> Doctors { set; get; }

    }
}