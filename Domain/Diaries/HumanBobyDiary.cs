using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Domain.Diaries
{
    [Description("diary2")]
    [Table("HumanBodyDiary")]
    public class HumanBobyDiary : BaseDiary
    {
        [JsonPropertyNameAttribute("column1")]
        public DateTime Day { get; set; }
        [JsonPropertyNameAttribute("column2")]
        public DateTime Time { get; set; }
        [JsonPropertyNameAttribute("column3")]
        public string Trigger { get; set; }
        [JsonPropertyNameAttribute("column4")]
        public string Thoughts { get; set; }
        [JsonPropertyNameAttribute("column5")]
        public string Emotions { get; set; }
        [JsonPropertyNameAttribute("column6")]
        public string Reactions { get; set; }
        [JsonPropertyNameAttribute("column7")]
        public string Situation { get; set; }
        [JsonPropertyNameAttribute("column8")]

        [Column(TypeName = "json")]
        public JsonObject HumanBody { get; set; }
    }
}