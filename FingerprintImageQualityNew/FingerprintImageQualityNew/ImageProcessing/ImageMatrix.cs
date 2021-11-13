/*
 * Created by: Miguel Angel Medina Pérez (miguel.medina.perez@gmail.com)
 * Created:
 * Comments by: Miguel Angel Medina Pérez (miguel.medina.perez@gmail.com) 
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FingerprintImageQualityNew.ImageProcessing
{
    /// <summary>
    ///     A class to represent a gray scale image using a matrix.
    /// </summary>
    public class ImageMatrix
    {
        #region public

        /// <summary>
        ///     Initialize the <see cref="ImageMatrix"/> from the specied image.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The specified image must contain 24 bits per pixel.
        ///     </para>
        ///     <para>
        ///         The new object is built using the image in gray scale.
        ///     </para>
        /// </remarks>
        /// <param name="bmp">The image to convert to gray scale matrix.</param>
        public ImageMatrix(Bitmap bmp)
        {
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr scan0 = bmData.Scan0;
            Height = bmp.Height;
            Width = bmp.Width;
            pixels = new int[bmp.Height, bmp.Width];
            unsafe
            {
                byte* p = (byte*)(void*)scan0;

                int nOffset = stride - bmp.Width * 3;

                byte red, green, blue;

                for (int y = 0; y < bmp.Height; ++y)
                {
                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        pixels[y, x] = (byte)(.299 * red
                            + .587 * green
                            + .114 * blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            bmp.UnlockBits(bmData);
        }

        /// <summary>
        ///     Initialize a new <see cref="ImageMatrix"/> with the specified width and height.
        /// </summary>
        /// <param name="width">
        ///     The width of the new <see cref="ImageMatrix"/>.
        /// </param>
        /// <param name="height">
        ///     The height of the new <see cref="ImageMatrix"/>.
        /// </param>
        public ImageMatrix(int width, int height)
        {
            Height = height;
            Width = width;
            pixels = new int[height, width];
        }

        /// <summary>
        ///     Gets or sets the gray scale value in the specified pixel.
        /// </summary>
        /// <param name="row">
        ///     The vertical component of the pixel coordinate.
        /// </param>
        /// <param name="column">
        ///     The horizontal component of the pixel coordinate.
        /// </param>
        /// <returns>
        ///     The gray scale value in the specified pixel.
        /// </returns>
        public int this[int row, int column]
        {
            get { return pixels[row, column]; }
            set { pixels[row, column] = value; }
        }

        /// <summary>
        ///     The height of the current <see cref="ImageMatrix"/>.
        /// </summary>
        public int Height { private set; get; }

        /// <summary>
        ///     The width of the current <see cref="ImageMatrix"/>.
        /// </summary>
        public int Width { private set; get; }

        /// <summary>
        ///     Convert the <see cref="ImageMatrix"/> to Bitmap.
        /// </summary>
        /// <remarks>
        ///     The returned image is in gray scale with 24 bits per pixel.
        /// </remarks>
        /// <returns>The computed Bitmap</returns>
        public Bitmap ToBitmap()
        {
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)scan0;
                int nOffset = stride - bmp.Width * 3;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        p[0] = p[1] = p[2] = (byte)(pixels[y, x] < 0 ? 0 : pixels[y, x] > 255 ? 255 : pixels[y, x]);
                        p += 3;
                    }
                    p += nOffset;
                }
            }

            bmp.UnlockBits(bmData);
            return bmp;
        }

        /// <summary>
        /// Obtiene la matriz de la imagen.
        /// </summary>
        /// <param name="image">Imagen a la que se le va a obtener la matrix</param>
        /// <returns>Matriz de reales de la imagen</returns>
        public unsafe static double[,] GetImageMatrix(Bitmap image, bool converterADouble)
        {
            double[,] imageMat = new double[image.Height, image.Width];
            BitmapData bmData = image.LockBits(new Rectangle(0, 0,
            image.Width, image.Height), ImageLockMode.ReadWrite,
            PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            byte* p = (byte*)Scan0;
            //int nWidth = curImage.Width; 
            int nOffset = stride - image.Width * 3;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    imageMat[i, j] = (double)p[0];
                    p += 3;
                }
                p += nOffset;
            }

            image.UnlockBits(bmData);
            return imageMat;
        }

        /// <summary>
        /// Obtiene la matriz de la imagen.
        /// </summary>
        /// <param name="image">Imagen a la que se le va a obtener la matrix</param>
        /// <returns>Matriz de enteros de la imagen</returns>
        public unsafe static int[,] GetImageMatrix(Bitmap image)
        {
            int[,] imageMat = new int[image.Height, image.Width];
            BitmapData bmData = image.LockBits(new Rectangle(0, 0,
            image.Width, image.Height), ImageLockMode.ReadWrite,
            PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            byte* p = (byte*)Scan0;
            //int nWidth = curImage.Width; 
            int nOffset = stride - image.Width * 3;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    imageMat[i, j] = p[0];
                    p += 3;
                }
                p += nOffset;
            }

            image.UnlockBits(bmData);
            return imageMat;
        }

        /// <summary>
        /// Convierte de un pixelformat a otro.
        /// </summary>
        /// <param name="binImg">Imagen que se analizará</param>
        /// <param name="pxFormat">Formato de pixel al que se va a convertir</param>
        /// <returns>Imagen convertida</returns>
        public static unsafe Bitmap Convert(Bitmap binImg, PixelFormat pxFormat)
        {
            if (binImg.PixelFormat == pxFormat)
                return binImg;

            Bitmap Bmap = new Bitmap(binImg.Width, binImg.Height, pxFormat);

            if (pxFormat == PixelFormat.Format8bppIndexed)
            {
                Bitmap gscale = binImg;

                /* Data from BMAP */
                BitmapData data = Bmap.LockBits(new Rectangle(0, 0, Bmap.Width, Bmap.Height), ImageLockMode.ReadWrite, pxFormat);
                int offset = data.Stride - Bmap.Width;
                byte* p = (byte*)data.Scan0;

                /* Data from gscale (Source) */
                BitmapData data2 = gscale.LockBits(new Rectangle(0, 0, gscale.Width, gscale.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                int offset2 = data2.Stride - (gscale.Width * 3);
                byte* p2 = (byte*)data2.Scan0;

                for (int y = 0; y < Bmap.Height; y++)
                {
                    for (int x = 0; x < Bmap.Width; x++)
                    {
                        p[0] = p2[0];
                        p++;
                        p2 += 3;
                    }
                    p += offset;
                    p2 += offset2;
                }
                Bmap.UnlockBits(data); gscale.UnlockBits(data2);
            }
            else
            {
                try
                {
                    Graphics graph = Graphics.FromImage(Bmap);
                    graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graph.DrawImage(binImg, new Rectangle(0, 0, binImg.Width, binImg.Height));
                }
                catch
                {
                    throw new Exception("No se puede convertir a este formato");
                }
            }
            return Bmap;
        }        

        #endregion

        #region private

        private int[,] pixels;

        #endregion
    }
}
