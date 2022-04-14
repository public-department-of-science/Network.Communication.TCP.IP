using ILGPU;
using ILGPU.Runtime;

namespace PC.Calculation.Performance.Test
{
    internal static class MatrixMultiply
    {

        #region Naive
        /// <summary>
        /// Multiplies two dense matrices and returns the resultant matrix.
        /// </summary>
        /// <param name="accelerator">The Accelerator to run the multiplication on</param>
        /// <param name="a">A dense MxK matrix</param>
        /// <param name="b">A dense KxN matrix</param>
        /// <returns>A dense MxN matrix</returns>
        internal static float[,] MatrixMultiplyNaive(float[,] a, float[,] b)
        {
            var m = a.GetLength(0);
            var ka = a.GetLength(1);
            var kb = b.GetLength(0);
            var n = b.GetLength(1);

            if (ka != kb)
                throw new ArgumentException($"Cannot multiply {m}x{ka} matrix by {n}x{kb} matrix", nameof(b));

            var c = new float[m, n];

            for (var x = 0; x < m; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    c[x, y] = 0;

                    for (var z = 0; z < ka; z++)
                        c[x, y] += a[x, z] * b[z, y];
                }
            }

            return c;
        }
        #endregion

        #region Tiled

        /// <summary>
        /// Size of the tile (NxN).
        /// </summary>
        const int TILE_SIZE = 2;

        /// <summary>
        /// Multiplies two dense matrices and returns the resultant matrix (using tiling).
        /// </summary>
        /// <param name="accelerator">The Accelerator to run the multiplication on</param>
        /// <param name="a">A dense MxK matrix</param>
        /// <param name="b">A dense KxN matrix</param>
        /// <returns>A dense MxN matrix</returns>
        internal static float[,] MatrixMultiplyTiled(Accelerator accelerator, float[,] a, float[,] b)
        {
            var m = a.GetLength(0);
            var ka = a.GetLength(1);
            var kb = b.GetLength(0);
            var n = b.GetLength(1);

            if (ka != kb)
                throw new ArgumentException($"Cannot multiply {m}x{ka} matrix by {n}x{kb} matrix", nameof(b));

            var kernel = accelerator.LoadStreamKernel<
                ArrayView2D<float, Stride2D.DenseX>,
                ArrayView2D<float, Stride2D.DenseX>,
                ArrayView2D<float, Stride2D.DenseX>>(
                MatrixMultiplyTiledKernel);
            var groupSize = new Index2D(TILE_SIZE, TILE_SIZE);
            var numGroups = new Index2D((m + TILE_SIZE - 1) / TILE_SIZE, (n + TILE_SIZE - 1) / TILE_SIZE);

            using var aBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(m, ka));
            using var bBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(ka, n));
            using var cBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(m, n));
            aBuffer.CopyFromCPU(a);
            bBuffer.CopyFromCPU(b);

            kernel((numGroups, groupSize), aBuffer, bBuffer, cBuffer);

            // Reads data from the GPU buffer into a new CPU array.
            // Implicitly calls accelerator.DefaultStream.Synchronize() to ensure
            // that the kernel and memory copy are completed first.
            return cBuffer.GetAsArray2D();
        }

        /// <summary>
        /// The tiled matrix multiplication kernel that runs on the accelerated device.
        /// </summary>
        /// <param name="aView">An input matrix of size MxK</param>
        /// <param name="bView">An input matrix of size KxN</param>
        /// <param name="cView">An output matrix of size MxN</param>
        static void MatrixMultiplyTiledKernel(
            ArrayView2D<float, Stride2D.DenseX> aView,
            ArrayView2D<float, Stride2D.DenseX> bView,
            ArrayView2D<float, Stride2D.DenseX> cView)
        {
            var global = Grid.GlobalIndex.XY;
            var x = Group.IdxX;
            var y = Group.IdxY;

            var aTile = SharedMemory.Allocate2D<float, Stride2D.DenseX>(new Index2D(TILE_SIZE, TILE_SIZE), new Stride2D.DenseX(TILE_SIZE));
            var bTile = SharedMemory.Allocate2D<float, Stride2D.DenseX>(new Index2D(TILE_SIZE, TILE_SIZE), new Stride2D.DenseX(TILE_SIZE));
            var sum = 0.0f;

            for (var i = 0; i < aView.IntExtent.X; i += TILE_SIZE)
            {
                if (global.X < aView.IntExtent.X && y + i < aView.IntExtent.Y)
                    aTile[x, y] = aView[global.X, y + i];
                else
                    aTile[x, y] = 0;

                if (x + i < bView.IntExtent.X && global.Y < bView.IntExtent.Y)
                    bTile[x, y] = bView[x + i, global.Y];
                else
                    bTile[x, y] = 0;
                Group.Barrier();

                for (var k = 0; k < TILE_SIZE; k++)
                    sum += aTile[new Index2D(x, k)] * bTile[new Index2D(k, y)];
                Group.Barrier();
            }

            if (global.X < cView.IntExtent.X && global.Y < cView.IntExtent.Y)
                cView[global] = sum;
        }

        #endregion

        #region Accelerated

        /// <summary>
        /// Multiplies two dense matrices and returns the resultant matrix.
        /// </summary>
        /// <param name="accelerator">The Accelerator to run the multiplication on</param>
        /// <param name="a">A dense MxK matrix</param>
        /// <param name="b">A dense KxN matrix</param>
        /// <returns>A dense MxN matrix</returns>
        internal static float[,] MatrixMultiplyAccelerated(Accelerator accelerator, float[,] a, float[,] b)
        {
            var m = a.GetLength(0);
            var ka = a.GetLength(1);
            var kb = b.GetLength(0);
            var n = b.GetLength(1);

            if (ka != kb)
                throw new ArgumentException($"Cannot multiply {m}x{ka} matrix by {n}x{kb} matrix", nameof(b));

            var kernel = accelerator.LoadAutoGroupedStreamKernel<
                Index2D,
                ArrayView2D<float, Stride2D.DenseX>,
                ArrayView2D<float, Stride2D.DenseX>,
                ArrayView2D<float, Stride2D.DenseX>>(
                MatrixMultiplyAcceleratedKernel);

            using var aBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(m, ka));
            using var bBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(ka, n));
            using var cBuffer = accelerator.Allocate2DDenseX<float>(new Index2D(m, n));
            aBuffer.CopyFromCPU(a);
            bBuffer.CopyFromCPU(b);

            kernel(cBuffer.Extent.ToIntIndex(), aBuffer.View, bBuffer.View, cBuffer.View);

            // Reads data from the GPU buffer into a new CPU array.
            // Implicitly calls accelerator.DefaultStream.Synchronize() to ensure
            // that the kernel and memory copy are completed first.
            return cBuffer.GetAsArray2D();
        }

        /// <summary>
        /// The matrix multiplication kernel that runs on the accelerated device.
        /// </summary>
        /// <param name="index">Current matrix index</param>
        /// <param name="aView">An input matrix of size MxK</param>
        /// <param name="bView">An input matrix of size KxN</param>
        /// <param name="cView">An output matrix of size MxN</param>
        static void MatrixMultiplyAcceleratedKernel(
            Index2D index,
            ArrayView2D<float, Stride2D.DenseX> aView,
            ArrayView2D<float, Stride2D.DenseX> bView,
            ArrayView2D<float, Stride2D.DenseX> cView)
        {
            var x = index.X;
            var y = index.Y;
            var sum = 0.0f;

            for (var i = 0; i < aView.IntExtent.Y; i++)
                sum += aView[new Index2D(x, i)] * bView[new Index2D(i, y)];

            cView[index] = sum;
        }

        #endregion
    }
}