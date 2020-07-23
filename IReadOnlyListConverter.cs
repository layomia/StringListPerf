using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StringListPerfBDN
{
    public class IReadOnlyListConverter : JsonConverter<IReadOnlyList<string>>
    {
        public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var list = new SimpleSegmentedList<string>();

            while (true)
            {
                reader.Read();

                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                list.Add(reader.GetString());
            }

            return list;
        }

        public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}