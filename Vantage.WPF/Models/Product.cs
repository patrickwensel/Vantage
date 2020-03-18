using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Vantage.WPF.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int productID { get; set; }

        private string name { get; set; }

        private string version { get; set; }

        public int ProductID
        {
            get
            {
                return productID;
            }
            set
            {
                productID = value;
                NotifyPropertyChanged("ProductID");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
                NotifyPropertyChanged("Version");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
