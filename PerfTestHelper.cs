using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace StringListPerf
{
    public static class PerfTestHelper
    {
        static Stopwatch stopWatch;
        static string message;
        static internal int Stop = 0;

        static List<double> perCallStat = new List<double>();

        public static void ForceGC(int gen, int sleep)
        {
            GC.Collect(gen);
            GC.WaitForPendingFinalizers();
            GC.Collect(gen);

            Thread.Sleep(sleep);
        }

        public static void StartRun(string message, Action warmup = null)
        {
            PerfTestHelper.message = message;

            Console.WriteLine(message);

            double warmupTime = 0;

            if (warmup != null)
            {
                PerfTestHelper.stopWatch = new Stopwatch();

                PerfTestHelper.stopWatch.Start();

                warmup();
                warmup();

                PerfTestHelper.stopWatch.Stop();

                Console.WriteLine("Warmup {0:N3} ms", warmupTime = PerfTestHelper.stopWatch.Elapsed.TotalMilliseconds);
            }

            ForceGC(2, 500);

            PerfTestHelper.stopWatch = new Stopwatch();

            PerfTestHelper.stopWatch.Start();

            PerfTestHelper.Stop--;

            if (PerfTestHelper.Stop == 0)
            {
                Debugger.Break();
            }
        }

        static int seq = 0;

        public static void Reset()
        {
            PerfTestHelper.perCallStat.Clear();
        }

        public static void StopRun(int count, bool reset = false)
        {
            PerfTestHelper.stopWatch.Stop();

            Thread.Sleep(200);

            PerfTestHelper.seq++;

            double perCall = PerfTestHelper.stopWatch.Elapsed.TotalMilliseconds * 1000 / count;

            if (reset)
            {
                PerfTestHelper.perCallStat.Clear();
            }

            PerfTestHelper.perCallStat.Add(perCall);

            string metric = string.Format("{0,2} {1:N3} µs per call, {2:N2} %", seq, perCall, perCall * 100 / PerfTestHelper.perCallStat[0]);

            Console.WriteLine("{0:N3} ms x {1:N0} {2}", PerfTestHelper.stopWatch.Elapsed.TotalMilliseconds, count, message);

            ConsoleColor old = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(metric);
            Console.ForegroundColor = old;
            Console.WriteLine();
        }
    }
}
