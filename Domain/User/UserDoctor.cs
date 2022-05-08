using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.User
{
    public class UserDoctor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Patient)), Column(Order = 0)]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey(nameof(Doctor)), Column(Order = 1)]
        public int DoctorId { get; set; }

        public User Patient { get; set; }
        public User Doctor { get; set; }
    }
}