using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int UserDoctorId { get; set; }

        public UserDoctor UserDoctor { get; set; }
        public DateTime LastViewDate { get; set; }
    }
}