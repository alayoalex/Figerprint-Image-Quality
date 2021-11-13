using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace FingerprintImageQualityNew.Algorithm.Analysis
{
    public class ProcessingBlock
    {
        /// <summary>
        /// Calcula la media de un bloque de la imagen.
        /// </summary>
        /// <param name="blockMatrix">Bloque</param>
        /// <returns>Media del bloque</returns>
        public static float Mean(int[,] blockMatrix)
        {
            int sum = 0;
            float mean = 0;

            for (int i = 0; i < blockMatrix.GetLength(0); i++)
                for (int j = 0; j < blockMatrix.GetLength(1); j++)
                    sum += blockMatrix[i, j];

            return mean = sum / (blockMatrix.GetLength(0) * blockMatrix.GetLength(1));
        }

        /// <summary>
        /// Calcula la varianza de un bloque de la imagen
        /// </summary>
        /// <param name="blockMatrix">Bloque</param>
        /// <param name="blockMean">Media del bloque</param>
        /// <returns>Varianza del bloque</returns>
        public static float Variance(int[,] blockMatrix, float blockMean)
        {
            float variance = 0;
            float sum = 0;

            for (int i = 0; i < blockMatrix.GetLength(0); i++)
                for (int j = 0; j < blockMatrix.GetLength(1); j++)
                    sum += (float)Math.Pow(blockMatrix[i, j] - blockMean, 2);

            return variance = sum / (blockMatrix.GetLength(0) * blockMatrix.GetLength(1));
        }

        /// <summary>
        /// Calcula el contraste del bloque
        /// </summary>
        /// <param name="blockMean">Mean del bloque</param>
        /// <param name="blockVariance">Varianza del bloque</param>
        /// <returns>Contraste del bloque</returns>
        public static float Contrast(float blockMean, float blockVariance)
        {
            if (blockVariance == 0)
                return 0;
            return blockVariance / blockMean;
        }

        /// <summary>
        /// Calcula la media Superior del bloque;
        /// </summary>
        /// <param name="blockMatrix">Bloque</param>
        /// <param name="blockMean">Media del bloque</param>
        /// <returns>UpperMean</returns>
        public static float UpperMean(int[,] blockMatrix, float blockMean /*float blockVariance*/)
        {
            float upperMean = 0;
            float sum = 0;
            float varianceModif = 0;

            for (int i = 0; i < blockMatrix.GetLength(0); i++)
                for (int j = 0; j < blockMatrix.GetLength(1); j++)
                    if (blockMatrix[i, j] > blockMean)
                        sum += (float)Math.Pow(blockMatrix[i, j] - blockMean, 2);

            varianceModif = sum / (blockMatrix.GetLength(0) * blockMatrix.GetLength(1));
            return upperMean = blockMean + varianceModif;
        }

        /// <summary>
        /// Calcula la media inferior del bloque.
        /// </summary>
        /// <param name="blockMatrix">Bloque</param>
        /// <param name="blockMean">media del Bloque</param>
        /// <returns>UnderMean</returns>
        public static float UnderMean(int[,] blockMatrix, float blockMean /*float blockVariance*/)
        {
            float underMean = 0;
            float sum = 0;
            float varianceModif = 0;

            for (int i = 0; i < blockMatrix.GetLength(0); i++)
                for (int j = 0; j < blockMatrix.GetLength(1); j++)
                    if (blockMatrix[i, j] < blockMean)
                        sum += (float)Math.Pow(blockMatrix[i, j] - blockMean, 2);

            varianceModif = sum / (blockMatrix.GetLength(0) * blockMatrix.GetLength(1));
            return underMean = blockMean - varianceModif;
        }

        /// <summary>
        /// Calcula el histograma de un bloque.
        /// </summary>
        /// <param name="block">Bloque</param>
        /// <returns>Histograma</returns>
        public static int[] Histogram(int[,] block)
        {
            int[] hist = new int[256];

            for (int i = 0; i < block.GetLength(0); i++)
                for (int j = 0; j < block.GetLength(1); j++)
                    hist[block[i, j]]++;

            return hist;
        }

        /// <summary>
        /// Calula la media del histograma
        /// </summary>
        /// <param name="histogram">Histograma del bloque</param>
        /// <param name="block">bloque</param>
        /// <returns>Mean</returns>
        public static float HistMean(int[] histogram, int[,] block)
        {
            int sum = 0;
            float mean = 0;

            for (int i = 0; i < block.GetLength(0); i++)
                for (int j = 0; j < block.GetLength(1); j++)
                    sum += block[i, j];

            if (block.GetLength(1) == 0)
                mean = sum / (block.GetLength(0) * 1);

            if (block.GetLength(0) == 0)
                mean = sum / (block.GetLength(1) * 1);

            if (block.GetLength(0) != 0 && block.GetLength(1) != 0)
                mean = sum / (block.GetLength(0) * block.GetLength(1));

            return mean;
        }

        /// <summary>
        /// Calcula la desviación estándar.
        /// </summary>
        /// <param name="hist">Histograma</param>
        /// <param name="mean">Media</param>
        /// <returns>Desviación Estándar</returns>
        public static float HistStdDev(int[] hist, float mean)
        {
            float stdDev = 0;
            float temp = 0;

            for (int i = 0; i < 256; i++)
                temp += ((i - mean) * (i - mean)) * hist[i];

            return stdDev = (float)Math.Sqrt(temp);
        }

        /// <summary>
        /// Calcula la suavidad del bloque;
        /// </summary>
        /// <param name="stdDev">Desviación estándar del bloque</param>
        /// <returns>Suavidad</returns>
        public static float Smoothness(float stdDev)
        {
            return 1 - (1 / (1 + (stdDev * stdDev)));
        }

        /// <summary>
        /// Calcula la uniformidad del bloque.
        /// </summary>
        /// <param name="hist">Histograma del bloque</param>
        /// <returns>Uniformidad</returns>
        public static float Uniformity(int[] hist)
        {
            float unif = 0;

            for (int i = 0; i < hist.Length; i++)
                unif += hist[i] * hist[i];

            return unif;
        }

        /// <summary>
        /// Medida de cuan no homogeneo es el bloque.
        /// </summary>
        /// <param name="mean">Media</param>
        /// <param name="stdDev">Desviación estándar</param>
        /// <param name="unif">Uniformidad</param>
        /// <param name="smooth">Suavidad</param>
        /// <returns>Métrica de calidad</returns>
        public static float Inhomogeneity(float mean, float stdDev, float unif, float smooth)
        {
            return (mean * unif) / (stdDev * smooth);
        }
    }
}
