using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.PrinterService
{
    public class AESPrinter
    {
        public string PrinterName { get; set; }
        public List<AESPaperKind> Paper { get; set; }
        public List<AESPaperSource> Trays { get; set; }
    }
}
