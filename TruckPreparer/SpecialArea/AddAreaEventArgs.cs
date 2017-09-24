using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPreparer.SpecialArea
{
    public class AddAreaEventArgs : EventArgs
    {
        private readonly string filelocation;
        private readonly string name;

        public AddAreaEventArgs(string loc)
        {
            filelocation = loc;
        }
        public AddAreaEventArgs(string name, string loc)
        {
            this.name = name;
            filelocation = loc;
        }
        public string Filelocation
        {
            get { return filelocation; }
        }
        public string Name
        {
            get { return name; }
        }
    }
}
