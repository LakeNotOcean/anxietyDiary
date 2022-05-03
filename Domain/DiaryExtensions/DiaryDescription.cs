using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.DiaryExpensions
{
    public class DiaryDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        [IndexColumn(IsUnique = true)]
        public string ShortName { get; set; }
        public string Name { get; set; }

        [ForeignKey("DiaryCategory")]
        public int CategoryId { get; set; }
        public DiaryCategory Category { get; set; }

        public DiaryType Type { get; set; }

        public string Description { get; set; }
        [JsonPropertyName("Columns")]
        public ICollection<NonArbitraryColumn> NonArbitraryColumns { get; set; }
        [JsonPropertyName("ArbitraryColumns")]
        public ICollection<ArbitraryColumn> ArbitraryColumns { get; set; }
    }
}