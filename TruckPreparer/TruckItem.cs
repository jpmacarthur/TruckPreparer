using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TruckPreparer
{
    public class TruckItem
    {
        public string fiveweek { get; set; }

        public string Name { get; set; }
        public string size { get; set; }
        public Quantity inStore { get; set; }
        public Quantity onTruck { get; set; }
        public string itemcode { get; set; }
        public string location { get; set; }
    }
}
