using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.User
{
    public class UserRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserTargetId { get; set; }
        public User UserTarget { get; set; }
        public int UserSourceId { get; set; }
        public User UserSource { get; set; }
    }
}