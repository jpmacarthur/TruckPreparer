using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCP.Common.Json;

namespace TruckPreparer.SpecialArea
{
    public class AreasJSON : PersistableJson
    {
        public string Name { get; set; }
        public LTS Items { get; set; }
        public DateTime Until { get; set; }
    }
    public class AreasList : PersistableJson
    {
        public List<AreasJSON> Areas { get; set; }
    }
}
