using TCP.IP.Client;

namespace PC.Calculation.Performance.Test
{
    public class BenchmarkContext
    {
        private static Random random = new Random();

        public BenchmarkBuilder CreateDefaultBenchmark() => Create(builder => builder.DefaultBenchmark());

        private BenchmarkBuilder Create(Action<BenchmarkBuilder> builder)
        {
            var benchmark = new BenchmarkBuilder();
            builder(benchmark);
            return benchmark;
        }

        public List<IterationInfo> Run()
        {
            var resultedData = new List<IterationInfo>();

            var currentIteration = MatrixBenchmark.Naive();
            resultedData.Add(SaveIterationData(nameof(MatrixBenchmark.Naive), currentIteration.data, currentIteration.maxMatrixSize));

            var benchmarkContext = new BenchmarkContext().CreateDefaultBenchmark();
            using var context = ILGPU.Context.CreateDefault();
            foreach (var method in benchmarkContext.BenchmarksIterator())
            {
                foreach (var device in context)
                {
                    using var accelerator = device.CreateAccelerator(context);

                    currentIteration = MatrixBenchmark.IterationsExperiment(method, accelerator);
                    resultedData.Add(SaveIterationData($"{device.Name} {method.Method.Name}", currentIteration.data, currentIteration.maxMatrixSize));
                }
            }
            return resultedData;
        }

        private IterationInfo SaveIterationData(string currentRunName, Dictionary<int, long> currentIteration, int maxMatrixSize)
        {
            string color = $"{random.Next(0, 255).ToString()},{random.Next(0, 255).ToString()},{random.Next(0, 255).ToString()},{random.Next(0, 255).ToString()}";
            var itereation = new IterationInfo(currentRunName, currentIteration, color, maxMatrixSize);

            return itereation;
        }
    }
}
