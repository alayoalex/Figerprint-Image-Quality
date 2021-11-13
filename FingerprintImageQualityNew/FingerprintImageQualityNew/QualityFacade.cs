using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FingerprintImageQualityNew
{
    public class QualityFacade
    {
        public static int GetQuality(Image img)
        {
            // 0 => Buena
            // 1 => Regular
            // 2 => Mala

            return 0;
        }

        ///<summary>
        /// Este método se encarga de establecer los parámetros que necesita el algoritmo para su ejecución
        ///</summary>
        public static void SetParams(Dictionary<string, dynamic> _params)
        {

        }

        ///<summary>
        ///Devuelve una colección de pares <nombre de parámetro, tipo de dato> que necesita el algoritmo.
        /// Si el parametro puede ser null entonces su tipo de dato será Nulleable<Tipo>
        ///</summary>
        public static Dictionary<string, Type> GetParams()
        {
            return new Dictionary<string, Type>();
        }

        ///<summary>
        ///Informa con que tipo biométrico trabaja el plugin
        ///</summary>
        public static int GetBiometricKind()
        {
            // Face = 0, FingerPrint = 1, Iris = 2, Voice = 3, Signature = 4

            return 1;
        }
    }
}
