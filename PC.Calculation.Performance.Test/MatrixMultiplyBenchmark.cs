using ILGPU;
using System.Diagnostics;

namespace PC.Calculation.Performance.Test
{
    public partial class MatrixMultiplyBenchmark
    {
        static void MathKernel(
               Index1D index,                  // The global thread index (1D in this case)
               ArrayView<float> singleView,    // A view of floats to store float results from GPUMath
               ArrayView<double> doubleView,   // A view of doubles to store double results from GPUMath
               ArrayView<double> doubleView2)  // A view of doubles to store double results from .Net Math
        {
            // Note the different returns type of GPUMath.Sqrt and Math.Sqrt.
            singleView[index] = IntrinsicMath.Abs(index);
            doubleView[index] = IntrinsicMath.Clamp(index, 0.0, 12.0);

            // Note that use can safely use functions from the Math class as long as they have a counterpart
            // in the IntrinsicMath class.
            doubleView2[index] = Math.Min(0.2, index);
        }

        /// <summary>
        /// Launches a simple math kernel.
        /// </summary>
        static void Main()
        {
            // using var context1 = Context.Create(builder => builder.Cuda().Profiling());

            // Create main context
            using var context = Context.CreateDefault();

            // For each available device...
            var mappedDevicesByType = context.Devices.GroupBy(x => x.AcceleratorType);

            Stopwatch stopwatch = new Stopwatch();

            int initialPoint = 4;
            long twoMinutes = 120_000;
            double twentyPercents = 0.2;
            double thirtyPercents = 0.3;
            int maxMatrixSize = 100_000;

            //List<(ProcessingUnitType, AlgoType, int, int)> naiveExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> cpuExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> openClExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> gpuExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();

            foreach (var devicesInGroup in mappedDevicesByType)
            {
                foreach (var device in devicesInGroup)
                {
                    var matrixSize = initialPoint;
                    var previousIteration = stopwatch.ElapsedMilliseconds;
                    var nextIteration = stopwatch.ElapsedMilliseconds;

                    do
                    {
                        previousIteration = nextIteration;
                        // place to prepare context and thing around matrix
                        {

                        }

                        stopwatch.Restart();
                        // the place where to call our 
                        {
                            int m = matrixSize, n = matrixSize, k = matrixSize;
                            var aMatrix = MatrixGenerateRandom.CreateRandomMatrix(m, k);
                            var bMatrix = MatrixGenerateRandom.CreateRandomMatrix(k, n);
                            var cMatrix = MatrixMultiply.MatrixMultiplyNaive(aMatrix, bMatrix);

                            RunMatrixMultiply(aMatrix, bMatrix, cMatrix);
                        }
                        stopwatch.Stop();

                        nextIteration = stopwatch.ElapsedMilliseconds;
                    } while (nextIteration - previousIteration > twoMinutes);
                }
            }
        }

        /// <summary>
        /// Performs matrix multiplication using each of the various implementations.
        /// </summary>
        static void RunMatrixMultiply(float[,] a, float[,] b, float[,] expectedResult)
        {
            var m = a.GetLength(0);
            var ka = a.GetLength(1);
            var kb = b.GetLength(0);
            var n = b.GetLength(1);

            Console.WriteLine($"Running matrix multiplication on [{m}x{ka}] * [{kb}x{n}]");
            var sw = new Stopwatch();

            // Naive implementation
            sw.Restart();
            var naiveResult = MatrixMultiply.MatrixMultiplyNaive(a, b);
            sw.Stop();
            Debug.Assert(MatrixHelper.MatrixEqual(naiveResult, expectedResult));
            Console.WriteLine($"- Naive implementation: {sw.ElapsedMilliseconds}ms");

            // Accelerated implementations
            using var context = Context.CreateDefault();

            foreach (var device in context)
            {
                using var accelerator = device.CreateAccelerator(context);

                sw.Restart();
                var acceleratedResult = MatrixMultiply.MatrixMultiplyAccelerated(accelerator, a, b);
                sw.Stop();
                Debug.Assert(MatrixHelper.MatrixEqual(acceleratedResult, expectedResult));
                Console.WriteLine($"- Accelerated implementation on {accelerator}: {sw.ElapsedMilliseconds}ms");

                sw.Restart();
                var acceleratedTiledResult = MatrixMultiply.MatrixMultiplyTiled(accelerator, a, b);
                sw.Stop();
                Debug.Assert(MatrixHelper.MatrixEqual(acceleratedTiledResult, expectedResult));
                Console.WriteLine($"- Tiled implementation on {accelerator}: {sw.ElapsedMilliseconds}ms");
            }
        }
    }
}