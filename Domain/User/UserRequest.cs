using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.User
{
    public class UserRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public RequestsEnum Request { get; set; }
        [Required]
        public int UserSourceId { get; set; }
        public User UserSource { get; set; }

        public int? UserTargetId { get; set; }
        public User UserTarget { get; set; }
    }
}