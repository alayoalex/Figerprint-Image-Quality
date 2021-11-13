using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FingerprintImageQualityNew;
using System.IO;

namespace TestingInterface
{
    public partial class CargarDataset : Form
    {
        private Controler _controler;
        private Dataset _dataset;

        public CargarDataset(Controler _cont)
        {            
            InitializeComponent();

            _controler = _cont;                               
        } 

        private void buttonDataset_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialogCargarDataset.ShowDialog() == DialogResult.OK)
                {
                    string nombreBaseDeDatos = folderBrowserDialogCargarDataset.SelectedPath.Split('\\').Last();
                    int cantidadItems = Directory.GetDirectories(folderBrowserDialogCargarDataset.SelectedPath).Length;
                    string[] directories = Directory.GetDirectories(folderBrowserDialogCargarDataset.SelectedPath);

                    comboBox1.Enabled = true;
                    textBoxAddress.Text = folderBrowserDialogCargarDataset.SelectedPath;                    
                    comboBox1.Items.Clear();

                    int cantControls = panel1.Controls.Count;
                    if (cantControls > 0)
                        panel1.Controls.Clear();

                    if (cantidadItems > 0)
                        for (int i = 0; i < cantidadItems; i++)
                            comboBox1.Items.Add(directories[i].Split('\\').Last());
                    else
                    {
                        label1.Visible = false;
                        label3.Visible = true;
                        progressBar1.Visible = true;
                        comboBox1.Enabled = false;

                        string[] rutas = Directory.GetFiles(folderBrowserDialogCargarDataset.SelectedPath);
                        int cantidadHuellas = rutas.Length;

                        progressBar1.Visible = true;
                        progressBar1.Maximum = cantidadHuellas;
                        int x = 10, y = 10;

                        for (int i = 0; i < cantidadHuellas; i++)
                        {
                            Bitmap img_i = new Bitmap(rutas[i]);
                            PictureBox contenedorImg = new PictureBox();

                            contenedorImg.SizeMode = PictureBoxSizeMode.StretchImage;

                            contenedorImg.Click += delegate(object sender1, EventArgs e1)
                            {
                                TimeSpan stop;
                                TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
                                
                                pictureBoxHuellaSeleccionada.Image = contenedorImg.Image;
                                pictureBoxHuellaSeleccionada.Refresh();

                                labelValor.Text = _controler.FingerprintQuality((Bitmap)pictureBoxHuellaSeleccionada.Image).ToString();
                                pictureBoxMapaHuellaSeleccinada.Image = _controler.FingerprintQualityMap();
                                
                                stop = new TimeSpan(DateTime.Now.Ticks);
                                
                                labelValorTiempo.Text = (stop.Subtract(start).TotalMilliseconds / 1000).ToString();
                            };

                            contenedorImg.Visible = true;
                            contenedorImg.Size = new Size(100, 95);
                            contenedorImg.Image = img_i;

                            panel1.Controls.Add(contenedorImg);

                            if (x + contenedorImg.Width > panel1.Width)
                            {
                                y += contenedorImg.Height + 5;
                                x = 10;
                            }

                            contenedorImg.Location = new Point(x, y);
                            x += contenedorImg.Width + 5;
                            progressBar1.Value = i;

                            float porc = (i * 100) / cantidadHuellas;

                            if (porc == 99)
                            {
                                progressBar1.Value = cantidadHuellas;
                            }
                            label3.Text = porc.ToString() + "%";
                            label3.Refresh();
                        }

                        label1.Visible = true;
                        label1.Text = "Cantidad de huellas en la base de datos: " + cantidadHuellas.ToString();

                        progressBar1.Visible = false;
                        label3.Visible = false;
                        label3.Text = "";
                        panel1.Refresh();
                    }                    
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void CargarDataset_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            label1.Visible = false;
            comboBox1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarImagenes();            
        }

        private void CargarImagenes()
        {
            try
            {
                label1.Visible = false;
                label3.Visible = true;
                progressBar1.Visible = true;

                _dataset = new Dataset(folderBrowserDialogCargarDataset.SelectedPath);
                textBoxAddress.Text = _dataset.AddressDataset;

                int cantControls = panel1.Controls.Count;
                if (cantControls > 0)
                    panel1.Controls.Clear();

                string nombreDataset = _dataset.AddressDataset;

                int cantidadDirectorios = Directory.GetDirectories(folderBrowserDialogCargarDataset.SelectedPath).Length;
                string[] directorios = Directory.GetDirectories(folderBrowserDialogCargarDataset.SelectedPath);

                int cantidadRutas = 0;
                string[] rutas = Directory.GetFiles(directorios[0]);

                for (int i = 0; i < cantidadDirectorios; i++)
                {
                    if (comboBox1.SelectedItem.ToString() == directorios[i].Split('\\').Last())
                    {
                        rutas = Directory.GetFiles(directorios[i]);
                        cantidadRutas = rutas.Length;
                    }
                }

                progressBar1.Visible = true;
                progressBar1.Maximum = cantidadRutas;
                int x = 10, y = 10;

                for (int i = 0; i < cantidadRutas; i++)
                {
                    Bitmap img_i = new Bitmap(rutas[i]);
                    PictureBox contenedorImg = new PictureBox();

                    contenedorImg.SizeMode = PictureBoxSizeMode.StretchImage;

                    contenedorImg.Click += delegate(object sender1, EventArgs e1)
                    {
                        TimeSpan stop;
                        TimeSpan start = new TimeSpan(DateTime.Now.Ticks);

                        pictureBoxHuellaSeleccionada.Image = contenedorImg.Image;
                        pictureBoxHuellaSeleccionada.Refresh();

                        labelValor.Text = _controler.FingerprintQuality((Bitmap)pictureBoxHuellaSeleccionada.Image).ToString();
                        pictureBoxMapaHuellaSeleccinada.Image = _controler.FingerprintQualityMap();

                        stop = new TimeSpan(DateTime.Now.Ticks);

                        labelValorTiempo.Text = (stop.Subtract(start).TotalMilliseconds / 1000).ToString();
                    };

                    contenedorImg.Visible = true;
                    contenedorImg.Size = new Size(100, 95);
                    contenedorImg.Image = img_i;

                    panel1.Controls.Add(contenedorImg);

                    if (x + contenedorImg.Width > panel1.Width)
                    {
                        y += contenedorImg.Height + 5;
                        x = 10;
                    }

                    contenedorImg.Location = new Point(x, y);
                    x += contenedorImg.Width + 5;
                    progressBar1.Value = i;

                    float porc = (i * 100) / cantidadRutas;

                    if (porc == 99)
                    {
                        progressBar1.Value = cantidadRutas;
                    }
                    label3.Text = porc.ToString() + "%";
                    label3.Refresh();
                }

                label1.Visible = true;
                label1.Text = "Cantidad de huellas en la base de datos: " + cantidadRutas.ToString();

                progressBar1.Visible = false;
                label3.Visible = false;
                label3.Text = "";
                panel1.Refresh();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  
    }
}
