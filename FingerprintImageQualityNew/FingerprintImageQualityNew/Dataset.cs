using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO: Posiblemente esta clase Dataset no sirve para nada y será mejor borrarla.
namespace FingerprintImageQualityNew
{
    public class Dataset
    {
        private string addressDataset;
        private string nombreDataset;

        public string AddressDataset
        {
            get { return addressDataset; }
            set { addressDataset = value; }
        }       

        public string NombreDataset
        {
            get { return nombreDataset; }
            set { nombreDataset = value; }
        }

        public Dataset(string Address)
        {
            this.addressDataset = Address;
            this.nombreDataset = Address.Split('\\').Last();
        }
    }
}
