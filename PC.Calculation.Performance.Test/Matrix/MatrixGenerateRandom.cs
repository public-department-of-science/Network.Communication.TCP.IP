namespace PC.Calculation.Performance.Test
{
    internal static class MatrixGenerateRandom
    {
        internal static float[,] CreateRandomMatrix(int rows, int columns)
        {
            var rnd = new Random();
            var matrix = new float[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                    matrix[i, j] = rnd.Next(minValue: -100, maxValue: 100);
            }

            return matrix;
        }
    }
}