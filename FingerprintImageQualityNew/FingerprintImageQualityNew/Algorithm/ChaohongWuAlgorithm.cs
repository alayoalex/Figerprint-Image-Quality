/*
 * Created by: Alexei Alayo Rondón (aalayo@estudiantes.uci.cu)
 * Created: 
 * Comments by: Alexei Alayo Rondón (aalayo@estudiantes.uci.cu)
 */

/*Paper: Image Quality Measures for Fingerprint Image Enhancement
  Authors: Chaohong Wu, Sergey Tulyakov and Venu Govindaraju
  Place: Center for Unified Biometrics and Sensors (CUBS) SUNY at Buffalo, USA
 
  Abstract: Fingerprint image quality is an important factor in the performance
of Automatic Fingerprint Identification Systems(AFIS). It is used to evaluate
the system performance, assess enrollment acceptability, and evaluate fingerprint
sensors. This paper presents a novel methodology for fingerprint image quality
measurement. We propose limited ring-wedge spectral measure to estimate the
global fingerprint image features, and inhomogeneity with directional contrast to
estimate local fingerprint image features. Experimental results demonstrate the
effectiveness of our proposal.*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using FingerprintImageQualityNew.Algorithm.Analysis;
using FingerprintImageQualityNew.Algorithm.Maps;

namespace FingerprintImageQualityNew.Algorithm
{
    public class ChaohongWuAlgorithm
    {
        private GlobalQualityAnalysis _globalAnalisys;
        private LocalQualityAnalysis _localAnalisys;

        public ChaohongWuAlgorithm() { }

        public int FingerprintQuality(Bitmap image)
        {
            _globalAnalisys = new GlobalQualityAnalysis(image);
            _localAnalisys = new LocalQualityAnalysis(image);

            var localQuality = _localAnalisys.LocalQuality();
            var globalQuality = _globalAnalisys.GlobalQuality();

            var blocksAmount = localQuality.Block.GetLength(0) * localQuality.Block.GetLength(1);
            int goodBlockCount = 0, normalBlockCount = 0, wetBlockCount = 0, dryBlockCount = 0, corruptedBlockCount = 0, backgroundBlockCount = 0, canNotDetermineBlockCount = 0;
            int goodCount = 0, normalCount = 0, wetCount = 0, dryCount = 0, spoiledCount = 0;

            float goodBlockPercentage = 0, normalBlockPercentage = 0, wetBlockPercentage = 0, dryBlockPercentage = 0, corruptedBlockPercentage = 0, canNotDetermineBlockPercentage = 0;

            for (int i = 0; i < localQuality.Block.GetLength(0); i++)
                for (int j = 0; j < localQuality.Block.GetLength(1); j++)
                    switch (localQuality.Block[i, j])
                    {
                        case BlockQuality.good:
                            goodBlockCount++;
                            break;
                        case BlockQuality.normal:
                            normalBlockCount++;
                            break;
                        case BlockQuality.wet:
                            wetBlockCount++;
                            break;
                        case BlockQuality.dry:
                            dryBlockCount++;
                            break;
                        case BlockQuality.corrupted:
                            corruptedBlockCount++;
                            break;
                        case BlockQuality.background:
                            backgroundBlockCount++;
                            break;
                        default:
                            canNotDetermineBlockCount++;
                            break;
                    }

            goodBlockPercentage = (float)((float)goodBlockCount / (float)(blocksAmount - backgroundBlockCount)) * 100;
            normalBlockPercentage = (float)((float)normalBlockCount / (float)(blocksAmount - backgroundBlockCount)) * 100;
            wetBlockPercentage = (float)((float)wetBlockCount / (float)(blocksAmount - backgroundBlockCount)) * 100;
            dryBlockPercentage = (float)((float)dryBlockCount / (float)(blocksAmount - backgroundBlockCount)) * 100;
            corruptedBlockPercentage = (float)((float)corruptedBlockCount / (float)(blocksAmount - backgroundBlockCount)) * 100;

            if (goodBlockPercentage + normalBlockPercentage >= 90)
                if (goodBlockPercentage > normalBlockPercentage)
                    goodCount++;                

            else if (wetBlockPercentage + dryBlockPercentage + corruptedBlockPercentage > 20)
                if (corruptedBlockPercentage > dryBlockPercentage || corruptedBlockPercentage > wetBlockPercentage)
                    spoiledCount++;
                else if (dryBlockPercentage > corruptedBlockPercentage || dryBlockPercentage > wetBlockPercentage)
                    dryCount++;
                else
                    wetCount++;
            else
                normalCount++;

            if (normalBlockPercentage + dryBlockPercentage >= 70)            
                if (normalBlockPercentage > dryBlockPercentage)
                    normalCount++;
                else
                    dryCount++;            

            if (normalBlockPercentage + wetBlockPercentage >= 70)            
                if (normalBlockPercentage > wetBlockPercentage)
                    normalCount++;
                else
                    wetCount++;            

            if (dryBlockPercentage + wetBlockPercentage + corruptedBlockPercentage > normalBlockPercentage + goodBlockPercentage)            
                if (corruptedBlockPercentage > dryBlockPercentage && corruptedBlockPercentage > wetBlockPercentage)
                    spoiledCount++;

                else if (dryBlockPercentage > corruptedBlockPercentage && dryBlockPercentage > wetBlockPercentage)
                    dryCount++;

                else if (wetBlockPercentage > corruptedBlockPercentage && wetBlockPercentage > dryBlockPercentage)
                    wetCount++;            

            //if (smudgePercentage > 10 || globalQuality > 5.15)
            //{
            //    if (wetBlockPercentage > 10 || wetBlockPercentage > corruptedBlockPercentage)
            //        wetCount++;
            //    else
            //        spoiledCount++;
            //}

            if (goodBlockPercentage >= 70 && globalQuality >= 4.90)
                goodCount++;

            else if (goodBlockPercentage < 70 && globalQuality >= 4.90)            
                //if (goodBlockPercentage >= 65)
                //    return ImageQuality.Good;
                if (goodBlockPercentage >= 50 && goodBlockPercentage > wetBlockPercentage && goodBlockPercentage > dryBlockPercentage)
                    normalCount++;

                else if (wetBlockPercentage >= 30 && globalQuality > 5.20 /*goodBlockPercentage < wetBlockPercentage && corruptedBlockPercentage < 30*/)
                    wetCount++;

                else if (dryBlockPercentage >= 30 && globalQuality <= 5.20)
                    dryCount++;
                
                else if (goodBlockPercentage <= 30 && corruptedBlockPercentage >= 30)
                    spoiledCount++;            

            else if (goodBlockPercentage >= 70 && globalQuality < 4.90)            
                //if (globalQuality >= 4.5)
                //    return ImageQuality.Good;
                if (globalQuality >= 4.0)
                    normalCount++;
                else
                    dryCount++;            

            if (goodBlockPercentage >= 50 && goodBlockPercentage < 70 && globalQuality >= 4.0 && globalQuality < 4.90)
                normalCount++;

            else if (goodBlockPercentage < 50 && globalQuality >= 4.0 && globalQuality < 4.90)            
                //if (goodBlockPercentage >= 45)
                //    return ImageQuality.Normal;

                if (goodBlockPercentage >= 30)
                    if (dryBlockPercentage > 30 /*dryBlockPercentage > wetBlockPercentage && dryBlockPercentage > corruptedBlockPercentage*/)
                        dryCount++;

                    //else if (dryBlockPercentage > wetBlockPercentage && dryBlockPercentage < corruptedBlockPercentage)
                    //return ImageQuality.Spoiled;

                    //else if (dryBlockPercentage < wetBlockPercentage && wetBlockPercentage > corruptedBlockPercentage)
                    //return ImageQuality.SmudgeAndWet;

                    else if (corruptedBlockPercentage > 30)
                        spoiledCount++;

                    //else
                    //    return ImageQuality.CanNotDetermine;

                //else if (goodBlockPercentage >= 30 && goodBlockPercentage < 45)
                    //return ImageQuality.DryAndLightlyInked;

                    else if (goodBlockPercentage < 30)                    
                        //if (corruptedBlockPercentage >= 30)
                        //return ImageQuality.Spoiled;

                        //else if (wetBlockPercentage > dryBlockPercentage && wetBlockPercentage > 30)
                        //return ImageQuality.SmudgeAndWet;

                        if (dryBlockPercentage >= 30)
                            dryCount++;
                        else 
                            spoiledCount++;                                

            else if (goodBlockPercentage >= 50 && goodBlockPercentage < 70 && globalQuality < 4.0)
                dryCount++;

            else if (goodBlockPercentage < 50 && globalQuality < 4.0)
                dryCount++;

            if (globalQuality < 4.0 && goodBlockPercentage <= 30 && wetBlockPercentage >= 30)
                wetCount++;

            if (globalQuality <= 4.0 && goodBlockPercentage <= 30 && dryBlockPercentage >= 30)
                dryCount++;

            if (corruptedBlockPercentage >= 30)
                spoiledCount++;

            int[] clasificaciones = { goodCount, normalCount, wetCount, dryCount, spoiledCount };
            float[] porcentage = { goodBlockPercentage, normalBlockPercentage, wetBlockPercentage, dryBlockPercentage, corruptedBlockPercentage };

            var mayor = 0;
            var posMayor = -1;
            for (int i = 0; i < clasificaciones.Length; i++)            
                if (clasificaciones[i] > mayor)
                {
                    posMayor = i;
                    mayor = clasificaciones[i];
                }            

            if (posMayor == -1)
            {
                float mayorPorcentage = 0;
                int posMayorPorcentage = -1;

                for (int i = 0; i < porcentage.Length; i++)                
                    if (porcentage[i] > mayorPorcentage)
                    {
                        posMayorPorcentage = i;
                        mayorPorcentage = porcentage[i];
                    }
                
                return posMayorPorcentage + 1;
            }

            return posMayor + 1;
        }        

        public Bitmap FingerprintQualityMap()
        {
            return _localAnalisys.LocalQualityRepresentacion();
        }

        //TODO: Programar recursivo para todos los subdirectorios dentro de un directorio. Mas adelante.
        public void QualityDataset(string[] ficheros, string selectedPath) 
        {
            //Proceso las huellas que hayan en el directorio indicado.                    
            System.IO.StreamWriter sw = new System.IO.StreamWriter(selectedPath + "\\DatasetQuality.txt");
            
            int contador = 0;
            foreach (var fichero in ficheros)
            {
                Bitmap huella = (Bitmap)Bitmap.FromFile(fichero);
                string fich = ficheros[contador++].Split('\\').Last();

                //sw.WriteLine(fich);                
                sw.WriteLine(FingerprintQuality(huella).ToString());
            }
            sw.Close();           
        }

        public void QualityDatasetMaps(string[] ficheros, string selectedPath) 
        {
            //Proceso las huellas que hayan en el directorio indicado.
            ficheros = Directory.GetFiles(selectedPath);  

            // Specify a "currently active folder"
            string activeDir = selectedPath;

            //Create a new subfolder under the current active folder
            string newPath = Path.Combine(activeDir, "Results");
            Directory.CreateDirectory(newPath);
            
            foreach (var fich in ficheros)
            {                
                Bitmap image = (Bitmap)Bitmap.FromFile(fich);
                _localAnalisys = new LocalQualityAnalysis(image);

                QualityEstimationMap qualityEstimationMap = _localAnalisys.LocalQuality();
                Bitmap qualityMap = _localAnalisys.LocalQualityRepresentacion();
                
                string newPathAux = Path.Combine(newPath, fich.Split('\\').Last());
                qualityMap.Save(newPathAux, System.Drawing.Imaging.ImageFormat.Tiff);
            }            
        }                      
    }

    public enum ImageQuality
    {
        Good, Normal, SmudgeAndWet, DryAndLightlyInked, Spoiled, CanNotDetermine
    }
}
