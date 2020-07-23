using BenchmarkDotNet.Running;

namespace MiscBenchmarks
{
    public class Program
    {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
