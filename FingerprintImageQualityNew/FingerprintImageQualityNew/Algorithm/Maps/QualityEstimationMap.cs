using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintImageQualityNew.Algorithm.Maps
{
    public class QualityEstimationMap
    {
        BlockQuality[,] block;

        public BlockQuality[,] Block
        {
            get { return block; }
            set { block = value; }
        }

        public QualityEstimationMap(int sizeX, int sizeY)
        {
            block = new BlockQuality[sizeX, sizeY];
        }

        public QualityEstimationMap() { }
    }

    public enum BlockQuality
    {
        good, normal, wet, dry, corrupted, background, canNotDetermine
    }
}
