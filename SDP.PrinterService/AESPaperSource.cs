using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.PrinterService
{
    public class AESPaperSource
    {
        public Dictionary<string, PaperSource> Trays { get; set; } = new Dictionary<string, PaperSource>();

        public AESPaperSource(string trayName)
        {
            MatchPaperSourceFromTrayName(trayName);
        }

        private void MatchPaperSourceFromTrayName(string trayName)
        {

            foreach (PaperSource tray in Enum.GetValues(typeof(PaperSource)))
            {
                // If Tray Name matches PaperSourceKind name
                if(tray.SourceName == trayName)
                {
                    PaperSource ps = new PaperSource();
                    ps.SourceName = trayName;
                    ps.RawKind = tray.RawKind;

                    if (!Trays.ContainsKey(trayName)) Trays.Add(trayName, ps);
                }
            }
        }
    }
}
