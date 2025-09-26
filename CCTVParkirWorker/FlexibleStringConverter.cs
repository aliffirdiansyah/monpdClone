using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CCTVParkirWorker
{
    public class FlexibleStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Kalau value memang string
            if (reader.TokenType == JsonTokenType.String)
                return reader.GetString();

            // Kalau value angka, convert ke string
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetInt32().ToString();

            // Kalau null / tipe lain
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
