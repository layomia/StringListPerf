using System;
using System.Collections.Generic;
using System.Text.Json;

namespace StringListPerf
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleSegmentedList<string> list = new SimpleSegmentedList<string>();

            int count = 8 * 1024 + 1;

            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }

            var data = new JsonData();

            data.Words = list;

            string text = JsonSerializer.Serialize(data);

            Console.WriteLine(data.Words);

#if DEBUG
            Console.WriteLine(text);
#endif
            {
                var data1 = JsonSerializer.Deserialize<JsonData0>(text);

                Console.WriteLine(data1.Words);
            }

            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new IReadOnlyListConverter() }
                };

                var data2 = JsonSerializer.Deserialize<JsonData>(text, options);

                Console.WriteLine(data2.Words);

#if DEBUG
                foreach (var w in data2.Words)
                {
                    Console.WriteLine(w);
                }

                Console.WriteLine("done");
#endif
            }

            PerfTest(text);
        }

        public static void PerfTest(string package)
        {
            int count = 1000;

            Console.WriteLine("data length: {0:N0}", package.Length);

            PerfTestHelper.StartRun("List");

            JsonData0 data0 = null;

            for (int i = 0; i < count; i++)
            {
                data0 = JsonSerializer.Deserialize<JsonData0>(package);
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new IReadOnlyListConverter() }
            };


            PerfTestHelper.StopRun(count);

            Console.WriteLine(data0.Words);

            PerfTestHelper.StartRun("SimpleSegmentedList");

            JsonData data1 = null;

            for (int i = 0; i < count; i++)
            {
                data1 = JsonSerializer.Deserialize<JsonData>(package, options);
            }

            PerfTestHelper.StopRun(count);

            Console.WriteLine(data1.Words);
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
