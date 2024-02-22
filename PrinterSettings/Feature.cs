using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterSettings
{
    public class Feature
    {
        public string Property { get; set; }
        public IEnumerable<string> Options { get; set; }
    }
}
