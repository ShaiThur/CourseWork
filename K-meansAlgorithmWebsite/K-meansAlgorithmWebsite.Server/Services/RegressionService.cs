namespace K_meansAlgorithmWebsite.Server.Services
{
    public class RegressionService
    {
        public double[] FindCoefficients(double[][] rawData)
        {
            int numRows = rawData.Length;
            int numCols = rawData[0].Length;

            double[,] matrixX = new double[numRows, numCols];
            double[] vectorY = new double[numRows];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols - 1; j++)
                {
                    matrixX[i, j] = rawData[i][j];
                }
                vectorY[i] = rawData[i][numCols - 1];
            }

            for (int i = 0; i < numRows; i++)
            {
                matrixX[i, numCols - 1] = 1;
            }

            double[,] transposeX = TransposeMatrix(matrixX);

            double[,] transposeX_X = MultiplyMatrices(transposeX, matrixX);

            double[] transposeX_Y = MultiplyMatrixVector(transposeX, vectorY);

            double[] coefficients = SolveLinearEquation(transposeX_X, transposeX_Y);

            return coefficients;
        }

        private double[,] TransposeMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[,] result = new double[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        private double[,] MultiplyMatrices(double[,] matrixA, double[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int rowsB = matrixB.GetLength(0);
            int colsB = matrixB.GetLength(1);

            if (colsA != rowsB)
            {
                throw new InvalidOperationException("Матрицы не могут быть перемножены.");
            }

            double[,] result = new double[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return result;
        }

        private double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (cols != vector.Length)
            {
                throw new InvalidOperationException("Матрица и вектор не могут быть перемножены.");
            }

            double[] result = new double[rows];

            for (int i = 0; i < rows; i++)
            {
                result[i] = 0;
                for (int j = 0; j < cols; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }

            return result;
        }

        private double[] SolveLinearEquation(double[,] matrix, double[] vector)
        {
            int n = vector.Length;
            double[,] augmentedMatrix = new double[n, n + 1];

            // Создаем расширенную матрицу, добавляя вектор Y как последний столбец
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, n] = vector[i];
            }

            // Прямой ход метода Гаусса
            for (int k = 0; k < n; k++)
            {
                // Находим максимальный элемент в текущем столбце
                int maxRow = k;
                for (int i = k + 1; i < n; i++)
                {
                    if (Math.Abs(augmentedMatrix[i, k]) > Math.Abs(augmentedMatrix[maxRow, k]))
                    {
                        maxRow = i;
                    }
                }

                // Переставляем строки, чтобы максимальный элемент был на диагонали
                if (maxRow != k)
                {
                    for (int j = k; j <= n; j++)
                    {
                        double temp = augmentedMatrix[k, j];
                        augmentedMatrix[k, j] = augmentedMatrix[maxRow, j];
                        augmentedMatrix[maxRow, j] = temp;
                    }
                }

                // Обнуляем элементы под диагональю
                for (int i = k + 1; i < n; i++)
                {
                    double factor = augmentedMatrix[i, k] / augmentedMatrix[k, k];
                    for (int j = k; j <= n; j++)
                    {
                        augmentedMatrix[i, j] -= factor * augmentedMatrix[k, j];
                    }
                }
            }

            // Обратный ход метода Гаусса
            double[] coefficients = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += augmentedMatrix[i, j] * coefficients[j];
                }
                coefficients[i] = (augmentedMatrix[i, n] - sum) / augmentedMatrix[i, i];
            }

            return coefficients;
        }
    }
}
