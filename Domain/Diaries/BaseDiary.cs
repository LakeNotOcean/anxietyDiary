using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Diaries
{
    [Table("BaseDiary")]
    public class BaseDiary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("DateTime")]

        public DateTime Date { get; set; }

        [JsonPropertyName("ChangeDateTime")]
        public DateTime ChangeDate { get; set; }

        [Required]
        public int DiaryUserId { get; set; }
        public Domain.User.User DiaryUser { get; set; }
    }
}