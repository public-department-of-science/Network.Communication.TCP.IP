using System.Diagnostics;

namespace PC.Calculation.Performance.Test
{
    internal static class MatrixHelper
    {

        /// <summary>
        /// Compares two matrices for equality.
        /// </summary>
        /// <param name="a">A dense MxN matrix</param>
        /// <param name="b">A dense MxN matrix</param>
        /// <returns>True if the matrices are equal</returns>
        internal static bool MatrixEqual(float[,] a, float[,] b)
        {
            var ma = a.GetLength(0);
            var na = a.GetLength(1);
            var mb = b.GetLength(0);
            var nb = b.GetLength(1);

            if (ma != mb || na != nb)
            {
                Debug.WriteLine($"Matrix dimensions do not match: [{ma}x{na}] vs [{mb}x{nb}]");
                return false;
            }

            for (var i = 0; i < ma; i++)
            {
                for (var j = 0; j < na; j++)
                {
                    var actual = a[i, j];
                    var expected = b[i, j];
                    if (actual != expected)
                    {
                        Debug.WriteLine($"Error at element location [{i}, {j}]: {actual} found, {expected} expected");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}