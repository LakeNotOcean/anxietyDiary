using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Diaries
{
    [Description("diary5")]
    [Table("emotions_diary")]
    public class emotionsDiary : BaseDiary
    {
        [JsonPropertyNameAttribute("column1")]
        public DateTime Column1 { get; set; }
        [JsonPropertyNameAttribute("column2")]
        public string Column2 { get; set; }
        [JsonPropertyNameAttribute("column3")]
        public string Column3 { get; set; }
        [JsonPropertyNameAttribute("column4")]
        public string Column4 { get; set; }
    }
}