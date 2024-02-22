using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.PrinterService
{
    public class AESPaperKind
    {
        public Dictionary<string, PaperKind> Paper { get; set; } = new Dictionary<string, PaperKind>();

        public AESPaperKind(string paperName)
        {
            MatchPaperNameFromPaperKind(paperName);
        }

        private void MatchPaperNameFromPaperKind(string paperName)
        {
            // Iterate through all PaperKind values
            foreach(PaperKind kind in Enum.GetValues(typeof(PaperKind)))
            {
                // If PaperKind name matches our paperName
                if (nameof(kind) == paperName)
                {
                    // Add new [key, value] to dictionary, if key doesn't already exist.
                    if (!Paper.ContainsKey(paperName)) Paper.Add(paperName, kind);
                }
            }
        }
    }
}
