using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;
using FingerprintImageQualityNew;
using FingerprintImageQualityNew.ImageProcessing;

namespace FingerprintImageQualityNew.Algorithm.Analysis
{
    public class GlobalQualityAnalysis
    {
        #region atributos

        private Bitmap image;
        private RealFourierTransformation realFourierTransform;
        private TransformationConvention convencion;

        #endregion        

        #region constructores

        //TODO: Examinar que significa la convencion esa.
        public GlobalQualityAnalysis(TransformationConvention convencion, Bitmap image)
        {
            this.image = image;
            this.convencion = convencion;
            realFourierTransform = new RealFourierTransformation(convencion);
        }

        public GlobalQualityAnalysis(Bitmap image)
        {
            this.image = image;
            this.convencion = TransformationConvention.Matlab;
            realFourierTransform = new RealFourierTransformation(convencion);
        }

        #endregion

        #region publicos

        public double GlobalQuality()
        {
            Bitmap imageRedimencinada = RedimencionarImagen();

            //Calculamos el fourier de la huella.
            double[,] fourier = Fourier(imageRedimencinada);

            int imageMatrixHeightPowetOf2 = imageRedimencinada.Height;
            int imageMatrixWidthPowerOf2 = imageRedimencinada.Width;

            double mayor = 0;
            double suma = 0;

            //Calculando el pico para el Limited Ring-Wedge desde la frecuencia 30 hasta la 60 para cada dirección.
            //Para la direccion theta = pi;
            for (int x = imageMatrixWidthPowerOf2 / 2 - 60; x < imageMatrixWidthPowerOf2 / 2 - 30; x++)
            {
                if (mayor < fourier[imageMatrixHeightPowetOf2 / 2, x])
                    mayor = fourier[imageMatrixHeightPowetOf2 / 2, x];
            }
            suma += mayor;
            mayor = 0;

            //Para la direccion theta = 0;
            for (int x = imageMatrixWidthPowerOf2 / 2 + 30; x < imageMatrixWidthPowerOf2 / 2 + 60; x++)
            {
                if (mayor < fourier[imageMatrixHeightPowetOf2 / 2, x])
                    mayor = fourier[imageMatrixHeightPowetOf2 / 2, x];
            }
            suma += mayor;
            mayor = 0;

            //Para la direccion theta = pi/2;
            for (int y = imageMatrixHeightPowetOf2 / 2 + 30; y < imageMatrixHeightPowetOf2 / 2 + 60; y++)
            {
                if (mayor < fourier[y, imageMatrixWidthPowerOf2 / 2])
                    mayor = fourier[y, imageMatrixWidthPowerOf2 / 2];
            }
            suma += mayor;
            mayor = 0;

            //Para la direccion theta = pi/4;
            for (int y = imageMatrixHeightPowetOf2 / 2 - 30, x = imageMatrixWidthPowerOf2 / 2 + 30; y > imageMatrixHeightPowetOf2 / 2 - 60 && x < imageMatrixWidthPowerOf2 / 2 + 60; y--, x++)
            {
                if (mayor < fourier[y, x])
                    mayor = fourier[y, x];
            }
            suma += mayor;
            mayor = 0;

            //Para la direccion theta = 3*pi/4;
            for (int y = imageMatrixHeightPowetOf2 / 2 - 30, x = imageMatrixWidthPowerOf2 / 2 - 30; y > imageMatrixHeightPowetOf2 / 2 - 60 && x > imageMatrixWidthPowerOf2 / 2 - 60; y--, x--)
            {
                if (mayor < fourier[y, x])
                    mayor = fourier[y, x];
            }
            suma += mayor;

            double quality = suma / 5;

            return quality;
        }

        #endregion        

        #region privados

        private double[,] Fourier(Bitmap imageRedimencionada)
        {
            double[,] imageMatrixDouble = ImageMatrix.GetImageMatrix(imageRedimencionada, true);

            int imageMatrixHeightPowetOf2 = imageMatrixDouble.GetLength(0);
            int imageMatrixWidthPowerOf2 = imageMatrixDouble.GetLength(1);

            double[,] imageMatrixDeFlotantesAbsFourierShiftLog = new double[imageMatrixHeightPowetOf2, imageMatrixWidthPowerOf2];
            double[] samples = new double[imageMatrixHeightPowetOf2 * imageMatrixWidthPowerOf2];
            double[] fftReal, fftImag;

            //Poniendo todos los datos de la matrix bidimencional en un solo vector.
            int contador = 0;
            for (int i = 0; i < imageMatrixHeightPowetOf2; i++)
                for (int j = 0; j < imageMatrixWidthPowerOf2; j++)
                    samples[contador++] = imageMatrixDouble[i, j];

            //Esta es la transformada.
            realFourierTransform.TransformForward(samples, out fftReal, out fftImag, imageMatrixHeightPowetOf2, imageMatrixWidthPowerOf2);

            int contador1 = 0, contador2 = 0;
            for (int i = 0; i < imageMatrixHeightPowetOf2; i++)
                for (int j = 0; j < imageMatrixWidthPowerOf2; j++)
                    imageMatrixDeFlotantesAbsFourierShiftLog[i, j] = Math.Log10(1 + Math.Sqrt((Math.Pow(fftReal[contador1++], 2) + Math.Pow(fftImag[contador2++], 2))));

            //El fftshift es dividir la matrix en cuatro cuadrantes y cambiar el primero con el tercero y el segundo cn el cuarto.
            double swap = 0;
            for (int i = 0, x = imageMatrixHeightPowetOf2 / 2; i < imageMatrixHeightPowetOf2 / 2 && x < imageMatrixHeightPowetOf2; i++, x++)
                for (int j = 0, y = imageMatrixWidthPowerOf2 / 2; j < imageMatrixWidthPowerOf2 / 2 && y < imageMatrixWidthPowerOf2; j++, y++)
                {
                    swap = imageMatrixDeFlotantesAbsFourierShiftLog[x, y];
                    imageMatrixDeFlotantesAbsFourierShiftLog[x, y] = imageMatrixDeFlotantesAbsFourierShiftLog[i, j];
                    imageMatrixDeFlotantesAbsFourierShiftLog[i, j] = swap;
                }

            for (int i = 0, x = imageMatrixHeightPowetOf2 / 2; i < imageMatrixHeightPowetOf2 / 2 && x < imageMatrixHeightPowetOf2; i++, x++)
                for (int j = imageMatrixWidthPowerOf2 / 2, y = 0; j < imageMatrixWidthPowerOf2 && y < imageMatrixWidthPowerOf2 / 2; j++, y++)
                {
                    swap = imageMatrixDeFlotantesAbsFourierShiftLog[x, y];
                    imageMatrixDeFlotantesAbsFourierShiftLog[x, y] = imageMatrixDeFlotantesAbsFourierShiftLog[i, j];
                    imageMatrixDeFlotantesAbsFourierShiftLog[i, j] = swap;
                }

            return imageMatrixDeFlotantesAbsFourierShiftLog;
        }
        
        //Redimencionando las imagenes a dimenciones potencias de dos.
        private Bitmap RedimencionarImagen()
        {
            int imageMatrixHeight = image.Height;
            int imageMatrixWidth = image.Width;

            int imageMatrixHeightPowetOf2, imageMatrixWidthPowerOf2;

            int smallerPowerOf2Height = Fn.CeilingToPowerOf2(imageMatrixHeight);
            int smallerPowerOf2Width = Fn.CeilingToPowerOf2(imageMatrixWidth);

            if ((smallerPowerOf2Height - imageMatrixHeight) > (imageMatrixHeight - smallerPowerOf2Height / 2))
                imageMatrixHeightPowetOf2 = smallerPowerOf2Height / 2;
            else
                imageMatrixHeightPowetOf2 = smallerPowerOf2Height;

            if ((smallerPowerOf2Width - imageMatrixWidth) > (imageMatrixWidth - smallerPowerOf2Width / 2))
                imageMatrixWidthPowerOf2 = smallerPowerOf2Width / 2;
            else
                imageMatrixWidthPowerOf2 = smallerPowerOf2Width;

            //Nuevo Mapa de  Bits con dimenciones potencia de dos.
            Bitmap imageNewDimention = new Bitmap(imageMatrixWidthPowerOf2, imageMatrixHeightPowetOf2);

            //Copiando la imagen original a un nuevo Mapa de Bits con dimenciones potencia de dos.           
            for (int i = 0; i < imageNewDimention.Height && i < image.Height; i++)
                for (int j = 0; j < imageNewDimention.Width && j < image.Width; j++)
                    imageNewDimention.SetPixel(j, i, image.GetPixel(j, i));

            //Rellenando los sobrantes con pixeles blancos, a lo alto.
            if (imageNewDimention.Height > image.Height)
                for (int i = image.Height; i < imageNewDimention.Height; i++)
                    for (int j = 0; j < imageNewDimention.Width; j++)
                        imageNewDimention.SetPixel(j, i, Color.White);

            //Rellenando los sobrantes con pixeles blancos, a lo ancho.
            if (imageNewDimention.Width > image.Width)
                for (int i = 0; i < imageNewDimention.Height; i++)
                    for (int j = image.Width; j < imageNewDimention.Width; j++)
                        imageNewDimention.SetPixel(j, i, Color.White);

            return imageNewDimention;
        }

        #endregion        
    }
}
