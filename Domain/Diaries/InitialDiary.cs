using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Diaries
{
    [Description("diary1")]
    [Table("InitialDiary")]
    public class InitialDiary : BaseDiary
    {
        [JsonPropertyName("column1")]
        public string Column1 { get; set; }
        [JsonPropertyName("column2")]
        public string Column2 { get; set; }
        [JsonPropertyName("column3")]
        public string Column3 { get; set; }
        [JsonPropertyName("column4")]
        public string Column4 { get; set; }
        [JsonPropertyName("column5")]
        public string Column5 { get; set; }
        [JsonPropertyName("column6")]
        public string Column6 { get; set; }
        [JsonPropertyName("column7")]
        public string Column7 { get; set; }

    }
}