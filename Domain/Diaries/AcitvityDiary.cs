using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Diaries
{
    [Description("diary3")]
    [Table("ActivityDiary")]
    public class AcitvityDiary : BaseDiary
    {
        [JsonPropertyName("column1")]
        public DateTime Time { get; set; }
        [JsonPropertyName("column2")]
        public string Work { get; set; }
        [JsonPropertyName("column3")]
        public string Pleasure { get; set; }
        [JsonPropertyName("column4")]
        public int Perfomance { get; set; }
    }
}