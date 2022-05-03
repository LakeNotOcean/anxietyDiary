using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.DiaryExpensions;
using Domain.Enums;

#nullable enable

namespace Domain.DiaryExpensions
{
    public class DiaryColumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = DatabaseConstants.standartStringType)]
        [MaxLength(DatabaseConstants.standartStringLength)]
        public string ShortName { get; set; }

        public string Name { get; set; }

        [ForeignKey("DiaryDescription")]
        public int DescriptionId { get; set; }

        public ColumnValueType ValueType { get; set; }

        public bool isOptional { get; set; }
        public ColumnPosition? Position { get; set; }
    }

    public class ArbitraryColumn : DiaryColumn
    {

    }

    public class NonArbitraryColumn : DiaryColumn
    {
        
    }
}