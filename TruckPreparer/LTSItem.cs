using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCP.Common.Json;

namespace TruckPreparer
{
    public class LTSItem : PersistableJson
    {
        public string Name { get; set; }
        public string Itemcode { get; set; }
        public string Size { get; set; }

    }
    public class LTS : PersistableJson
    {
        public List<LTSItem> Items { get; set; }
    }
}
