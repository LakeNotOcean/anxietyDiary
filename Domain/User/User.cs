using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.User
{
    public class User : IdentityUser<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { set; get; }

        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        [IndexColumn(IsUnique = true)]
        public override string UserName { set; get; }

        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.passwordMaxLength)]
        public override string PasswordHash { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public override string Email { set; get; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { set; get; }
        public Role Role { get; set; }
        [RegularExpression(DatabaseConstants.namePattern)]
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string FirstName { set; get; }
        [RegularExpression(DatabaseConstants.namePattern)]
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string SecondName { set; get; }

        public DateTime LastRecordModify { set; get; }
        public bool isSearching { set; get; }

        public string Description { set; get; }

        [InverseProperty("Patient")]
        public virtual ICollection<UserDoctor> Patients { get; set; }

        [InverseProperty("Doctor")]
        public virtual ICollection<UserDoctor> Doctors { set; get; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}