using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TruckPreparer
{
    public partial class MainWindow : INotifyPropertyChanged
    {

        private bool checkboxstat;

        public bool Checkboxstat
        {
            get { return checkboxstat; }
            set
            {
                if (checkboxstat != value)
                {
                    checkboxstat = value;
                    OnPropertyChanged("Checkboxstat");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
    } }
