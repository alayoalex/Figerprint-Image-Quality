using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FingerprintImageQualityNew.Algorithm;

namespace FingerprintImageQualityNew
{
    public class Controler
    {
        private ChaohongWuAlgorithm _algorithm;

        public Controler() 
        {
            _algorithm = new ChaohongWuAlgorithm();
        }

        //public ImageQuality FingerprintQuality(Bitmap image) 
        //{
        //    return _algorithm.FingerprintQuality(image);
        //}

        public int FingerprintQuality(Bitmap image)
        {
            return _algorithm.FingerprintQuality(image);
        }

        public Bitmap FingerprintQualityMap() 
        {
            return _algorithm.FingerprintQualityMap();
        }
        
        public void DatasetQuality(string[] ficheros, string selectedPath) 
        {
            _algorithm.QualityDataset(ficheros, selectedPath);
        }

        public void DatasetQualityMap(string[] ficheros, string selectedPath)
        {
            _algorithm.QualityDatasetMaps(ficheros, selectedPath);
        }
    }
}
