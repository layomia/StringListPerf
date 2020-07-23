using System;
using System.Collections.Generic;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StringListPerfBDN
{
    [MemoryDiagnoser]
    public class StringListPerfBDN
    {
        public string _json;
        private readonly int _count = 1000;

        public JsonSerializerOptions _options;
        public JsonSerializerSettings _settings;

        [GlobalSetup]
        public void Setup()
        {
            _options = new JsonSerializerOptions { Converters = { new IReadOnlyListConverter() } };
            _settings = new JsonSerializerSettings();
            _settings.ContractResolver = SegmentedContractResolver.Instance;

            SimpleSegmentedList<string> list = new SimpleSegmentedList<string>();

            int count = 8 * 1024 + 1;

            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }

            var data = new JsonData();
            data.Words = list;

            _json = JsonConvert.SerializeObject(data);
        }

        [Benchmark]
        public void SystemTextJson_Unoptimized()
        {
            for (int i = 0; i < _count; i++)
            {
                JsonSerializer.Deserialize<JsonData0>(_json);
            }
        }

        [Benchmark]
        public void SystemTextJson_Optimized()
        {
            for (int i = 0; i < _count; i++)
            {
                JsonSerializer.Deserialize<JsonData>(_json, _options);
            }
        }

        [Benchmark]
        public void NewtonsoftJson_Unoptimized()
        {
            for (int i = 0; i < _count; i++)
            {
                JsonConvert.DeserializeObject<JsonData0>(_json);
            }
        }

        [Benchmark]
        public void NewtonsoftJson_Optimized()
        {
            for (int i = 0; i < _count; i++)
            {
                JsonConvert.DeserializeObject<JsonData>(_json, _settings);
            }
        }

        class JsonData0
        {
            public List<string> Words { get; set; }
        }

        class JsonData
        {
            public IReadOnlyList<string> Words { get; set; }
        }
    }

    public class SegmentedContractResolver : DefaultContractResolver
    {
        public static SegmentedContractResolver Instance = new SegmentedContractResolver();

        protected override JsonArrayContract CreateArrayContract(Type objectType)
        {
            JsonArrayContract result = base.CreateArrayContract(objectType);

            if (objectType == typeof(IReadOnlyList<string>))
            {
                result.OverrideCreator = (args) => new SimpleSegmentedList<string>();
            }

            return result;
        }
    }
}
