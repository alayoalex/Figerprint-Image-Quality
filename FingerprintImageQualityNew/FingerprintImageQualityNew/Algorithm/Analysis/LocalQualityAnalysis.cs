using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;
using FingerprintImageQualityNew.Algorithm.Maps;
using FingerprintImageQualityNew.ImageProcessing;

namespace FingerprintImageQualityNew.Algorithm.Analysis
{
    public class LocalQualityAnalysis
    {
        #region atributos

        private Bitmap _image;
        private int[,] _imaMatrix;

        private int _imageMatrixHeight;
        private int _imageMatrixWidth;

        private int _resHeight;
        private int _resWidth;

        private int _filasMap;
        private int _columnasMap;

        private int _blocksize;

        //private float _inHomRange;
        //private float _varianceRange;
        //private float _stdDevRange;
        //private float _meanRange;
        //private float _stdDevAndMeanRange;
        //private double _coherenceRange;

        private MeanMap _meanMap;
        private VarianceMap _varianceMap;
        private StdDevMap _stdDevMap;
        private SmoothMap _smoothMap;
        private UniformityMap _unifMap;
        private InHomMap _inHomMap;        
        private OrientationCoherenceMap _orientationCoherenceMap;
        private QualityEstimationMap _qualityEstimationMap;
        //private ContrastMap _contrastMap;

        private Ratha1995OrImgExtractor _ratha1995Orientations;

        #endregion

        #region properties

        public InHomMap InHomMap
        {
            get { return _inHomMap; }
            set { _inHomMap = value; }
        }

        public VarianceMap VarianceMap
        {
            get { return _varianceMap; }
            set { _varianceMap = value; }
        }

        public int FilasMap
        {
            get { return _filasMap; }
            set { _filasMap = value; }
        }

        public int ColumnasMap
        {
            get { return _columnasMap; }
            set { _columnasMap = value; }
        }        

        #endregion

        #region constructores

        public LocalQualityAnalysis(Bitmap image)
        {
            this._image = image;

            _imageMatrixHeight = image.Height;
            _imageMatrixWidth = image.Width;

            _resHeight = 0;
            _resWidth = 0;

            _filasMap = 0;
            _columnasMap = 0;

            //TODO: si la huella tiene una determinada dimencion que los bloque sean de 8x8 y si no de 16x16.
            _blocksize = 16;            

            _meanMap = new MeanMap();
            _varianceMap = new VarianceMap();
            _stdDevMap = new StdDevMap();
            _smoothMap = new SmoothMap();
            _unifMap = new UniformityMap();
            _inHomMap = new InHomMap();            
            _orientationCoherenceMap = new OrientationCoherenceMap();
            _qualityEstimationMap = new QualityEstimationMap();

            _ratha1995Orientations = new Ratha1995OrImgExtractor();
        }

        #endregion        

        #region publicos
        
        public QualityEstimationMap LocalQuality()
        {   
            InicializationOfMaps();
            MeasuresComputationOfMaps();

            for (int i = 0; i < _filasMap; i++)            
                for (int j = 0; j < _columnasMap; j++)                
                    if (_orientationCoherenceMap.Block[i, j] == OrientationCoherenceMap.Null)                    
                        if (_meanMap.Block[i, j] <= 100)
                            _qualityEstimationMap.Block[i, j] = BlockQuality.corrupted;                            

                        else
                            _qualityEstimationMap.Block[i, j] = BlockQuality.background;                    

                    else if (_orientationCoherenceMap.Block[i, j] > 0 && _orientationCoherenceMap.Block[i, j] <= 0.25)                    
                        _qualityEstimationMap.Block[i, j] = BlockQuality.corrupted;                    

                    else if (_orientationCoherenceMap.Block[i, j] > 0.25 && _orientationCoherenceMap.Block[i, j] <= 0.50)                    
                        if ((_meanMap.Block[i, j] < 60 && _stdDevMap.Block[i, j] < 1300) || (_inHomMap.Block[i, j] > 500 && _inHomMap.Block[i, j] <= 1500))
                            _qualityEstimationMap.Block[i, j] = BlockQuality.wet;                       

                        else if (_inHomMap.Block[i, j] > 3000 || _meanMap.Block[i, j] > 160 || (_meanMap.Block[i, j] / _orientationCoherenceMap.Block[i, j] > 5 && _unifMap.Block[i, j] / _smoothMap.Block[i, j] > 20))
                            _qualityEstimationMap.Block[i, j] = BlockQuality.dry;

                        else                         
                            _qualityEstimationMap.Block[i, j] = BlockQuality.canNotDetermine;

                    else if (_orientationCoherenceMap.Block[i, j] > 0.50 && _orientationCoherenceMap.Block[i, j] <= 0.75)                   
                        _qualityEstimationMap.Block[i, j] = BlockQuality.normal; 

                    else if (_orientationCoherenceMap.Block[i, j] > 0.75)                     
                        _qualityEstimationMap.Block[i, j] = BlockQuality.good;                                     
            
            return _qualityEstimationMap;            
        }
        
        public Bitmap LocalQualityRepresentacion()
        {
            Bitmap image1 = new Bitmap(_image.Width, _image.Height, PixelFormat.Format24bppRgb);
            int b = 0;

            for (int i = 0; i < _imageMatrixHeight - _resHeight; i += _blocksize)
            {
                int c = 0;
                for (int j = 0; j < _imageMatrixWidth - _resWidth; j += _blocksize)
                {
                    int x = 0;
                    
                    if (_qualityEstimationMap.Block[b, c] == BlockQuality.background)
                        for (int k = i; k < i + _blocksize; k++)
                        {
                            for (int l = j; l < j + _blocksize; l++)
                                image1.SetPixel(l, k, Color.Black);
                            x++;
                        }
                    else if (_qualityEstimationMap.Block[b, c] == BlockQuality.good)
                        for (int k = i; k < i + _blocksize; k++)
                        {
                            for (int l = j; l < j + _blocksize; l++)
                                image1.SetPixel(l, k, Color.White);
                            x++;
                        }
                    else if (_qualityEstimationMap.Block[b, c] == BlockQuality.corrupted)
                        for (int k = i; k < i + _blocksize; k++)
                        {
                            for (int l = j; l < j + _blocksize; l++)
                                image1.SetPixel(l, k, Color.FromArgb(100, 100, 100));
                            x++;
                        }
                    else if (_qualityEstimationMap.Block[b, c] == BlockQuality.dry)
                        for (int k = i; k < i + _blocksize; k++)
                        {
                            for (int l = j; l < j + _blocksize; l++)
                                image1.SetPixel(l, k, Color.FromArgb(200, 200, 200));
                            x++;
                        }
                    else
                        for (int k = i; k < i + _blocksize; k++)
                        {
                            for (int l = j; l < j + _blocksize; l++)
                                image1.SetPixel(l, k, Color.FromArgb(150, 150, 150));
                            x++;
                        }
                    c++;
                }
                b++;
            }
            return image1;
        }        

        #endregion  
      
        #region privados

        private void InicializationOfMaps()
        {
            _imaMatrix = new int[_imageMatrixHeight, _imageMatrixWidth];

            if (_image.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                Bitmap ima = ImageMatrix.Convert(_image, PixelFormat.Format8bppIndexed);                
                _imaMatrix = ImageMatrix.GetImageMatrix(ima);
            }
            else
            {
                _imaMatrix = ImageMatrix.GetImageMatrix(_image);
            }

            _resHeight = _imageMatrixHeight % _blocksize;
            _resWidth = _imageMatrixWidth % _blocksize;

            _filasMap = _imageMatrixHeight / _blocksize;
            _columnasMap = _imageMatrixWidth / _blocksize;

            _meanMap = new MeanMap(_filasMap, _columnasMap);
            _varianceMap = new VarianceMap(_filasMap, _columnasMap);
            _stdDevMap = new StdDevMap(_filasMap, _columnasMap);
            _smoothMap = new SmoothMap(_filasMap, _columnasMap);
            _unifMap = new UniformityMap(_filasMap, _columnasMap);
            //_contrastMap = new ContrastMap(filasMap, columnasMap);
            _inHomMap = new InHomMap(_filasMap, _columnasMap);
            _orientationCoherenceMap = new OrientationCoherenceMap(_filasMap, _columnasMap);
            _qualityEstimationMap = new QualityEstimationMap(_filasMap, _columnasMap);
        }

        private void MeasuresComputationOfMaps()
        {
            int[,] block = new int[_blocksize, _blocksize];
            int b = 0;

            for (int i = 0; i < _imageMatrixHeight - _resHeight; i += _blocksize)
            {
                int c = 0;
                for (int j = 0; j < _imageMatrixWidth - _resWidth; j += _blocksize)
                {
                    int x = 0;
                    for (int k = i; k < i + _blocksize; k++)
                    {
                        int y = 0;
                        for (int l = j; l < j + _blocksize; l++)
                            block[x, y++] = _imaMatrix[k, l];
                        x++;
                    }

                    int[] hist = ProcessingBlock.Histogram(block);
                    _meanMap.Block[b, c] = ProcessingBlock.HistMean(hist, block);
                    _varianceMap.Block[b, c] = ProcessingBlock.Variance(block, _meanMap.Block[b, c]);
                    _stdDevMap.Block[b, c] = ProcessingBlock.HistStdDev(hist, _meanMap.Block[b, c]);
                    _smoothMap.Block[b, c] = ProcessingBlock.Smoothness(_stdDevMap.Block[b, c]);
                    _unifMap.Block[b, c] = ProcessingBlock.Uniformity(hist);
                    //_contrastMap.Block[b, c] = ProcessingBlock.Contrast(meanMap.Block[b, c], varianceMap.Block[b, c]);
                    _inHomMap.Block[b, c] = ProcessingBlock.Inhomogeneity(_meanMap.Block[b, c], _stdDevMap.Block[b, c], _unifMap.Block[b, c], _smoothMap.Block[b, c]);                    
                    c++;
                }
                b++;
            }            
            _orientationCoherenceMap = _ratha1995Orientations.ComputingCoherenceMap(_image);
            
        }                

        #endregion
    }
}
