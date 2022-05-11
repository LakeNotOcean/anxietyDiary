using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Extensions
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string format = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.ParseExact(reader.GetString().Trim('"'), format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString(format));
        }
    }
}