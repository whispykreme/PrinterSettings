using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Drawing.Printing.PrinterSettings;

namespace SDP.PrinterService
{
    public class PrinterService
    {
        public List<string> Printers { get; set; } = new List<string>();
        public Dictionary<string, List<string>> PrinterPaperSources { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> PrinterPaperKinds { get; set; } = new Dictionary<string, List<string>>();

        public List<string> GetPrinters()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Printers.Add(printer);
                PrinterPaperSources.Add(printer, GetTraysFromPrinterName(printer));
                PrinterPaperKinds.Add(printer, GetPaperFromPrinterName(printer));
            }

            return Printers;
        }

        public List<string> GetTraysFromPrinterName(string printerName)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings = new PrinterSettings();
            pd.PrinterSettings.PrinterName = printerName;

            List<string> paperSources = new List<string>();
            
            foreach (PaperSource tray in pd.PrinterSettings.PaperSources)
            {
                paperSources.Add(tray.SourceName);
            }

            return paperSources;
        }

        public List<string> GetPaperFromPrinterName(string  printerName)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings = new PrinterSettings();
            pd.PrinterSettings.PrinterName = printerName;

            List<string> paper = new List<string>();

            foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
            {
                paper.Add(ps.PaperName);
            }

            return paper;
        }

        public PrintDocument GetPrintDocument(PrintDocument pd, string printerName, string trayName, string paperKind, bool duplex)
        {
            pd.PrinterSettings.PrinterName = printerName;
            PrinterSettings ps = pd.PrinterSettings;

            // if duplex is true, set duplex to flip page vertically
            if (duplex) { ps.Duplex = Duplex.Vertical; }
            // else, just print simplex (one-sided)
            else { ps.Duplex = Duplex.Simplex; }

            PaperSource tray = MatchTray(ps.PaperSources, trayName);
            PaperSize paper = MatchPaperKind(ps, paperKind);

            // Get DefaultPageSettings, then update the settings
            PageSettings printPage = ps.DefaultPageSettings;
            printPage.PaperSize = paper;
            printPage.PaperSource = tray;
            printPage.Margins = new Margins(0,0,0,0); // Set margins to 0, so it prints across the whole page.
            printPage.Landscape = false; // Print in Portrait mode.
            // Set PrinterResolution?
            // printPage.PrinterResolution = ps.PrinterResolutions;

            return pd;
        }

        private PaperSource MatchTray(PaperSourceCollection psc, string trayName)
        {
            // Iterate through PaperSourceCollection until we find the correct PaperSource
            foreach (PaperSource ps in psc)
            {
                if (ps.SourceName == trayName)
                {
                    return ps;
                }
            }

            // Return null if we never find the right PaperSource
            return null;
        }

        private PaperSize MatchPaperKind(PrinterSettings ps, string paperKind)
        {
            // Iterate through PaperSizes until we find the correct PaperSize
            foreach (PaperSize paper in  ps.PaperSizes)
            {
                if (paper.PaperName == paperKind)
                {
                    return paper;
                }
            }

            // Return null if we never find the right PaperSize
            return null;
        }
    }
}
