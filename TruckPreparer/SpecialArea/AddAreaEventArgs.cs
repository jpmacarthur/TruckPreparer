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
        private readonly DateTime start;
        private readonly DateTime end;

        public AddAreaEventArgs(string loc)
        {
            filelocation = loc;
        }
        public AddAreaEventArgs(string name, string loc)
        {
            this.name = name;
            filelocation = loc;
        }
        public AddAreaEventArgs(string name, string loc, DateTime start, DateTime end)
        {
            this.name = name;
            filelocation = loc;
            this.start = start;
            this.end = end;
        }
        public string Filelocation
        {
            get { return filelocation; }
        }
        public string Name
        {
            get { return name; }
        }
        public DateTime Start
        {
            get { return start; }
        }
        public DateTime End
        {
            get { return end; }
        }
    }
}
