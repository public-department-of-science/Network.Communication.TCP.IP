using ILGPU.Runtime;
using System.Diagnostics;

namespace PC.Calculation.Performance.Test
{
    public class MatrixBenchmark
    {
        private const int twentySecs = 20_000;
        private const int fourHundred = 400;
        private const int zero = 0;
        private const int one = 1;
        private const int two = 2;
        private const int three = 3;
        private const int five = 5;
        private const int twentyFive = 25;
        private const int fifty = 50;
        private const double dotOfive = 0.05;
        private const double oneDotTwo = 1.2;

        private const int initialPoint = 4;
        private static long iterationRunTimeMlSec = 60_000;
        private const double dotOne = 0.1;
        private const double ten = dotOne;
        private const double thirtyPercents = 0.3;
        private const int computeAvailability = 100_000;

        public static (Dictionary<int, long> data, int maxMatrixSize) Naive(int userIterationRunTimeMlSec)
        {
            iterationRunTimeMlSec = userIterationRunTimeMlSec;
            var matrixSize = initialPoint;

            Stopwatch iterationsStopwatch = new Stopwatch();
            var previousIteration = iterationsStopwatch.ElapsedTicks;
            var thisIteration = iterationsStopwatch.ElapsedTicks;

            long timeDifference = one;
            double iterationTimeRatio = dotOne;

            int stuckCounter = zero;
            int maxComputeAvailability = zero;
            int iterationsCount = zero;

            var iterations = new Dictionary<int, long>();
            var totalRunTime = new Stopwatch();

            do
            {
                GC.Collect();
                totalRunTime.Start();
                previousIteration = thisIteration;

                int m = matrixSize, n = matrixSize, k = matrixSize;
                var aMatrix = MatrixGenerateRandom.CreateRandomMatrix(m, k);
                var bMatrix = MatrixGenerateRandom.CreateRandomMatrix(k, n);

                iterationsStopwatch.Restart();

                try
                {
                    var cMatrix = MatrixMultiply.MatrixMultiplyNaive(aMatrix, bMatrix);
                }
                catch (Exception ex)
                {
                    break;
                }

                iterationsStopwatch.Stop();

                thisIteration = iterationsStopwatch.ElapsedTicks;
                timeDifference = thisIteration - previousIteration;

                if (previousIteration == zero)
                {
                    previousIteration = one;
                }

                iterationTimeRatio = ((double)previousIteration / thisIteration);

                if (iterationTimeRatio < ten)
                {
                    matrixSize = matrixSize + twentyFive;
                    stuckCounter++;
                }
                if (iterationTimeRatio > thirtyPercents)
                {
                    matrixSize = matrixSize - two;
                }

                if (timeDifference == zero || timeDifference < fifty && matrixSize < fourHundred)
                {
                    matrixSize = matrixSize * three;
                }
                else
                {
                    matrixSize = (int)(matrixSize * oneDotTwo);
                }

                if (stuckCounter >= five && iterationTimeRatio < dotOfive)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }
                if (stuckCounter > five && iterationsStopwatch.ElapsedMilliseconds > twentySecs)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }

                maxComputeAvailability = matrixSize;
                iterationsCount++;

                iterations.Add(matrixSize, iterationsStopwatch.ElapsedTicks);
            } while (totalRunTime.ElapsedMilliseconds < iterationRunTimeMlSec || matrixSize >= computeAvailability);

            totalRunTime.Stop();
            stuckCounter = 0;

            return (data: iterations, maxMatrixSize: maxComputeAvailability);
        }

        public static (Dictionary<int, long> data, int maxMatrixSize)
            IterationsExperiment(Func<Accelerator, float[,], float[,], float[,]> method, Accelerator cpuAccelerator, int userIterationRunTimeMlSec)
        {
            iterationRunTimeMlSec = userIterationRunTimeMlSec;
            var matrixSize = initialPoint;

            Stopwatch iterationsStopwatch = new Stopwatch();
            var previousIteration = iterationsStopwatch.ElapsedTicks;
            var thisIteration = iterationsStopwatch.ElapsedTicks;

            long timeDifference = one;
            double iterationTimeRatio = dotOne;

            int stuckCounter = zero;
            int maxComputeAvailability = zero;
            int iterationsCount = zero;

            var iterations = new Dictionary<int, long>();
            var totalRunTime = new Stopwatch();

            do
            {
                GC.Collect();
                totalRunTime.Start();
                previousIteration = thisIteration;

                int m = matrixSize, n = matrixSize, k = matrixSize;
                var aMatrix = MatrixGenerateRandom.CreateRandomMatrix(m, k);
                var bMatrix = MatrixGenerateRandom.CreateRandomMatrix(k, n);

                iterationsStopwatch.Restart();

                try
                {
                    var cMatrix = method(cpuAccelerator, aMatrix, bMatrix);
                }
                catch (Exception ex)
                {
                    break;
                }

                iterationsStopwatch.Stop();

                thisIteration = iterationsStopwatch.ElapsedTicks;
                timeDifference = thisIteration - previousIteration;

                if (previousIteration == zero)
                {
                    previousIteration = one;
                }

                iterationTimeRatio = ((double)previousIteration / thisIteration);

                if (iterationTimeRatio < ten)
                {
                    matrixSize = matrixSize + twentyFive;
                    stuckCounter++;
                }
                if (iterationTimeRatio > thirtyPercents)
                {
                    matrixSize = matrixSize - two;
                }

                if (timeDifference == zero || timeDifference < fifty && matrixSize < fourHundred)
                {
                    matrixSize = matrixSize * three;
                }
                else
                {
                    matrixSize = (int)(matrixSize * oneDotTwo);
                }

                if (stuckCounter >= five && iterationTimeRatio < dotOfive)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }
                if (stuckCounter > five && iterationsStopwatch.ElapsedMilliseconds > twentySecs)
                {
                    maxComputeAvailability = matrixSize;
                    break;
                }

                maxComputeAvailability = matrixSize;
                iterationsCount++;

                iterations.Add(matrixSize, iterationsStopwatch.ElapsedTicks);
            } while (totalRunTime.ElapsedMilliseconds < iterationRunTimeMlSec || matrixSize >= computeAvailability);

            totalRunTime.Stop();
            stuckCounter = 0;

            return (data: iterations, maxMatrixSize: computeAvailability);
        }

        static float[,] RunMatrixMultiplyAccelerated(Accelerator accelerator, float[,] a, float[,] b)
        {
            var expectedResult = MatrixMultiply.Accelerated(accelerator, a, b);
            return expectedResult;
        }

        static float[,] RunMatrixMultiplyTiled(Accelerator accelerator, float[,] a, float[,] b)
        {
            var expectedResult = MatrixMultiply.AcceleratedTiled(accelerator, a, b);
            return expectedResult;
        }
    }
}