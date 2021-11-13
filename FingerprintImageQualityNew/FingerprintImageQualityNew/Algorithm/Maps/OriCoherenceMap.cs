using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FingerprintImageQualityNew.Algorithm.Maps
{
    public class OrientationCoherenceMap
    {
        double[,] block;

        public const double Null = 0.0;

        public double[,] Block
        {
            get { return block; }
            set { block = value; }
        }

        public OrientationCoherenceMap(int sizeX, int sizeY)
        {
            block = new double[sizeX, sizeY];
        }

        public OrientationCoherenceMap() { }
    }
}
