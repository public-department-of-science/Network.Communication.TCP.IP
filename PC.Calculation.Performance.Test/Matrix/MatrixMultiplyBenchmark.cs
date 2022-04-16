using ILGPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using System.Diagnostics;

namespace PC.Calculation.Performance.Test
{
    public class MatrixMultiplyBenchmark
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
        public static Dictionary<int, (long ms, long ticks)> Run()
        {
            // using var context1 = Context.Create(builder => builder.Cuda().Profiling());

            // Create main context
            //  using var context = Context.CreateDefault();

            // For each available device...
            //    var mappedDevicesByType = context.Devices.GroupBy(x => x.AcceleratorType);

            Stopwatch stopwatch = new Stopwatch();

            int initialPoint = 4;
            long totalRunTime_2min = 12_000;
            double ten = 0.1;
            double thirtyPercents = 0.3;
            int maxMatrixSize = 100_000;

            //List<(ProcessingUnitType, AlgoType, int, int)> naiveExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> cpuExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> openClExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();
            //List<(ProcessingUnitType, AlgoType, int, int)> gpuExecution = new List<(ProcessingUnitType, AlgoType, int, int)>();

            //   foreach (var devicesInGroup in mappedDevicesByType)
            //   {
            // foreach (var device in devicesInGroup)
            //     {

            var matrixSize = initialPoint;
            var previousIteration = stopwatch.ElapsedMilliseconds;
            var thisIteration = stopwatch.ElapsedMilliseconds;

            long timeDifference = 1;
            double iterationTimeRatio = 0.1;

            int stackCounter = 0;
            int maxComputeAvailability = 0;
            int iterationsCount = 0;

            Dictionary<int, (long, long)> iterations = new Dictionary<int, (long, long)>();
            Stopwatch totalTime = new Stopwatch();
            using var context = Context.Create(builder => builder.Cuda());
            using var cudaAccelerator = context.GetCudaDevice(0).CreateAccelerator(context);

            do
            {
                GC.Collect();
                totalTime.Start();
                previousIteration = thisIteration;

                // place to prepare context and thing around matrix
                int m = matrixSize, n = matrixSize, k = matrixSize;
                var aMatrix = MatrixGenerateRandom.CreateRandomMatrix(m, k);
                var bMatrix = MatrixGenerateRandom.CreateRandomMatrix(k, n);

                stopwatch.Restart();
                // var cMatrix = MatrixMultiply.MatrixMultiplyNaive(aMatrix, bMatrix);
                // 30_000*30_000 тыс умножения двух матриц хватает 14Гб памяти

                try
                {
                    var cMatrix = MatrixMultiply.MatrixMultiplyAccelerated(cudaAccelerator, aMatrix, bMatrix);
                }
                catch (Exception ex)
                {
                    break;
                }

                stopwatch.Stop();

                thisIteration = stopwatch.ElapsedTicks;
                timeDifference = thisIteration - previousIteration;

                if (previousIteration == 0)
                {
                    previousIteration = 1;
                }

                iterationTimeRatio = ((double)previousIteration / thisIteration);

                if (iterationTimeRatio < ten)
                {
                    matrixSize = matrixSize + 25;
                    stackCounter++;
                }
                if (iterationTimeRatio > thirtyPercents)
                {
                    matrixSize = matrixSize - 2;
                }

                if (timeDifference == 0 || timeDifference < 50 && matrixSize < 400)
                {
                    matrixSize = matrixSize * 3;
                }
                else
                {
                    matrixSize = (int)(matrixSize * 1.2);
                }

                if (stackCounter >= 5 && iterationTimeRatio < 0.05)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }
                if (stackCounter > 5 && stopwatch.ElapsedMilliseconds > 20_000)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }

                maxComputeAvailability = matrixSize;
                iterationsCount++;

                iterations.Add(matrixSize, (stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks));
            } while (totalTime.ElapsedMilliseconds < totalRunTime_2min || matrixSize >= maxMatrixSize);

            totalTime.Stop();
            Console.WriteLine(maxComputeAvailability);

            return iterations;
            //       }
            //    }
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