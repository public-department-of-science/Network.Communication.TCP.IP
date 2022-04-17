namespace PC.Calculation.Performance.Test
{
    public class BenchmarkBuilder
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private List<Func<ILGPU.Runtime.Accelerator, float[,], float[,], float[,]>> methods = new List<Func<ILGPU.Runtime.Accelerator, float[,], float[,], float[,]>>();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BenchmarkBuilder DefaultBenchmark()
        {
            methods.AddRange(new List<Func<ILGPU.Runtime.Accelerator, float[,], float[,], float[,]>>()
            {
                MatrixMultiply.Accelerated,
                MatrixMultiply.AcceleratedTiled,
            });

            return this;
        }

        public IEnumerable<Func<ILGPU.Runtime.Accelerator, float[,], float[,], float[,]>> BenchmarksIterator()
        {
            foreach (var val in methods)
            {
                yield return val;
            }
        }
    }
}
