using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.User
{
    public class LastUserView
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }


        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string DiaryName { get; set; }

        [Required]
        [ForeignKey("UserDoctor"), Column(Order = 1)]
        public int UserDoctorId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastViewDate { get; set; }
    }
}