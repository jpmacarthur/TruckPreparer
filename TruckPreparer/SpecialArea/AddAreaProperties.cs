using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPreparer.SpecialArea
{
    public class AddAreaProp : INotifyPropertyChanged
    {
        private string filelocation;
        public string FileLocation
        {
            get { return this.filelocation; }
            set
            {
                if (this.filelocation != value)
                {
                    this.filelocation = value;
                    this.NotifyPropertyChanged("Filelocation");
                }
            }
        }
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }
        private DateTime startDateTime;
        public DateTime StartDateTime
        {
            get { return this.startDateTime; }
            set
            {
                if (this.startDateTime != value)
                {
                    this.startDateTime = value;
                    this.NotifyPropertyChanged("StartDateTime");
                }
            }
        }
        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            get { return this.endDateTime; }
            set
            {
                if (this.endDateTime != value)
                {
                    this.endDateTime = value;
                    this.NotifyPropertyChanged("EndDateTime");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
