using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StringListPerf
{
    public class IReadOnlyListConverter : JsonConverter<IReadOnlyList<string>>
    {
        public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonTokenType tokenType = reader.TokenType;

            if (tokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var list = new SimpleSegmentedList<string>();

            while (true)
            {
                reader.Read();
                tokenType = reader.TokenType;

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
