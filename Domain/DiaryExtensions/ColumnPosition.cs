using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.DiaryExpensions;

namespace Domain.DiaryExpensions
{
    public class ColumnPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Column")]
        public int ColumnId { get; set; }
        public DiaryColumn column { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}