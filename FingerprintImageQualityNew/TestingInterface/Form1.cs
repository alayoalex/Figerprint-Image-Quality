using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using FingerprintImageQualityNew;
using System.Drawing.Imaging;

namespace TestingInterface
{
    public partial class Form1 : Form
    {
        private Controler _controler;
        private Bitmap _image;   
        
        public Form1()
        {
            InitializeComponent();

            _controler = new Controler();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttonQuality.Enabled = false;
            buttonQualityMap.Enabled = false;

            calidadToolStripMenuItem1.Enabled = false;
            mapaToolStripMenuItem.Enabled = false;            
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = openFileDialog1.FileName;
                    _image = (Bitmap)Bitmap.FromFile(fileName);
                    pictureBox1.Image = _image;

                    buttonQuality.Enabled = true;
                    calidadToolStripMenuItem1.Enabled = true;

                    buttonQualityMap.Enabled = false;
                    mapaToolStripMenuItem.Enabled = false;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
            new Form1();
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = saveFileDialog1.FileName;
                    pictureBox2.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }                
            }
        }     

        ///        
        /// <summary>
        ///Procesamiento. 
        /// </summary>      
        private void buttonQuality_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan stop;
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);

                string quality = _controler.FingerprintQuality(_image).ToString();

                stop = new TimeSpan(DateTime.Now.Ticks);

                MessageBox.Show("Quality: " + quality + "." + " Tiempo: " + stop.Subtract(start).TotalMilliseconds / 1000 + " segundos.");

                buttonQualityMap.Enabled = true;
                mapaToolStripMenuItem.Enabled = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }                       
        }

        private void buttonQualityMap_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox2.Image = _controler.FingerprintQualityMap();            
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }  

        private void calidadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan stop;
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);

                string quality = _controler.FingerprintQuality(_image).ToString();

                stop = new TimeSpan(DateTime.Now.Ticks);

                MessageBox.Show("Quality: " + quality + "." + " Tiempo: " + stop.Subtract(start).TotalMilliseconds / 1000 + " segundos.");
                buttonQualityMap.Enabled = true;
                mapaToolStripMenuItem.Enabled = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox2.Image = _controler.FingerprintQualityMap();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void calidadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan stop;
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog1.SelectedPath;
                    string[] ficheros = Directory.GetFiles(selectedPath);

                    _controler.DatasetQuality(ficheros, selectedPath);

                    stop = new TimeSpan(DateTime.Now.Ticks);

                    MessageBox.Show("Calculada correctamente la calidd del set de huellas en " + stop.Subtract(start).TotalMilliseconds / 1000 + " segundos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }        
        }

        private void mapasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan stop;
                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog1.SelectedPath;
                    string[] ficheros = Directory.GetFiles(selectedPath);

                    _controler.DatasetQualityMap(ficheros, selectedPath);

                    stop = new TimeSpan(DateTime.Now.Ticks);

                    MessageBox.Show("Generada correctamente la calidd del set de huellas en " + stop.Subtract(start).TotalMilliseconds / 1000 + " segundos." + " Gardado en: " + selectedPath + "\\Results", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = openFileDialog1.FileName;
                    _image = (Bitmap)Bitmap.FromFile(fileName);
                    pictureBox1.Image = _image;

                    buttonQuality.Enabled = true;
                    calidadToolStripMenuItem1.Enabled = true;

                    buttonQualityMap.Enabled = false;
                    mapaToolStripMenuItem.Enabled = false;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            new Form1();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {            
            CargarDataset formulario = new CargarDataset(_controler);            
            formulario.ShowDialog();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = saveFileDialog1.FileName;
                    pictureBox2.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    const string fic = @"d:\Alexei Alayo\Git\Results\1_2.wsq.qm";
        //    string[] texto = new string[60];

        //    System.IO.StreamReader sr = new System.IO.StreamReader(fic);
        //    //texto = sr.ReadToEnd();

        //    for (int i = 0; i < texto.Length; i++)
        //    {
        //        texto[i] = sr.ReadLine();
        //    }

        //    int[,] matrix = new int[60,80];;

        //    for (int i = 0; i < texto.Length; i++)
        //    {
        //        string[] elementos = texto[i].Split(' ');

        //        int k = 0;
        //        for (int j = 0; j < elementos.Length; j++)
        //        {
        //            if (elementos[j] != "")
        //            {
        //                int a = int.Parse(elementos[j]);

        //                if (int.Parse(elementos[j]) >= 0 && int.Parse(elementos[j]) <= 4)
        //                    matrix[i, k++] = int.Parse(elementos[j]);
        //            }
        //        }
        //    }

        //    pictureBox2.Image = LocalQualityRepresentacion(matrix);
        //}

        //private Bitmap LocalQualityRepresentacion(int[,] matrix)
        //{
        //    Bitmap image1 = new Bitmap(640, 480, PixelFormat.Format24bppRgb);
        //    int b = 0;

        //    for (int i = 0; i < 480; i += 8)
        //    {
        //        int c = 0;
        //        for (int j = 0; j < 640; j += 8)
        //        {
        //            int x = 0;

        //            if (matrix[b, c] == 0)
        //                for (int k = i; k < i + 8; k++)
        //                {
        //                    for (int l = j; l < j + 8; l++)
        //                        image1.SetPixel(l, k, Color.Black);
        //                    x++;
        //                }
        //            else if (matrix[b, c] == 1)
        //                for (int k = i; k < i + 8; k++)
        //                {
        //                    for (int l = j; l < j + 8; l++)
        //                        image1.SetPixel(l, k, Color.White);
        //                    x++;
        //                }
        //            else if (matrix[b, c] == 4)
        //                for (int k = i; k < i + 8; k++)
        //                {
        //                    for (int l = j; l < j + 8; l++)
        //                        image1.SetPixel(l, k, Color.FromArgb(100, 100, 100));
        //                    x++;
        //                }
        //            else if (matrix[b, c] == 3)
        //                for (int k = i; k < i + 8; k++)
        //                {
        //                    for (int l = j; l < j + 8; l++)
        //                        image1.SetPixel(l, k, Color.FromArgb(200, 200, 200));
        //                    x++;
        //                }
        //            else
        //                for (int k = i; k < i + 8; k++)
        //                {
        //                    for (int l = j; l < j + 8; l++)
        //                        image1.SetPixel(l, k, Color.FromArgb(150, 150, 150));
        //                    x++;
        //                }
        //            c++;
        //        }
        //        b++;
        //    }
        //    return image1;
        //}
    }
}
