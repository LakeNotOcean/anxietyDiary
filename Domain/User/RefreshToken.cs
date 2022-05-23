using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.User
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User TokenUser { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime? Revoked { get; set; }
        [NotMapped]
        public bool IsActive => Revoked == null && !IsExpired;

    }
}