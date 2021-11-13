using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintImageQualityNew.Algorithm.Maps
{
    public class UniformityMap
    {
        float[,] block;

        public float[,] Block
        {
            get { return block; }
            set { block = value; }
        }

        public UniformityMap(int sizeX, int sizeY)
        {
            block = new float[sizeX, sizeY];
        }

        public UniformityMap() { }
    }
}
