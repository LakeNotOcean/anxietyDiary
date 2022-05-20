using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.User;

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
        [ForeignKey("User")]
        public int DiaryUserId { get; set; }
    }
}