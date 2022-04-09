using TCP.IP.Client.Benchmarking;

namespace TCP.IP.Client
{
    internal class ClientPCBenchmarkScore
    {
        public BenchmarkTestType TaskType { get; set; }
        public AcceleratorType AcceleratorType { get; set; }
        public CalculationAlgorithmType CalculationAlgorithmType { get; set; }
        public long Elapsed_Milliseconds { get; set; }
    }
}
