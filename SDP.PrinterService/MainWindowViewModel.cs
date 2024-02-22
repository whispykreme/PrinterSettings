using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SDP.PrinterService
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private PrinterService PrintersService;

        private List<string> _printers = new List<string>();
        public List<string> Printers
        {
            get => _printers;
            set
            {
                _printers = value;
                OnPropertyChanged(nameof(Printers));
            }
        }

        private string _selectedPrinter;
        public string SelectedPrinter
        {
            get => _selectedPrinter;
            set
            {
                _selectedPrinter = value;

                Trays = PrintersService.GetTraysFromPrinterName(_selectedPrinter);
                Paper = PrintersService.GetPaperFromPrinterName(_selectedPrinter);

                OnPropertyChanged(nameof(SelectedPrinter));
                OnPropertyChanged(nameof(Paper));
                OnPropertyChanged(nameof(Trays));
            }
        }

        private List<string> _trays;
        public List<string> Trays
        {
            get => _trays;
            set
            {
                _trays = value;
                OnPropertyChanged(nameof(Trays));
            }
        }

        private string _selectedTray;
        public string SelectedTray
        {
            get => _selectedTray;
            set
            {
                _selectedTray = value;
                OnPropertyChanged(nameof(SelectedTray));
            }
        }

        private List<string> _paper;
        public List<string> Paper
        {
            get => _paper;
            set
            {
                _paper = value;
                OnPropertyChanged(nameof(Paper));
            }
        }

        private string _selectedPaper;
        public string SelectedPaper
        {
            get => _selectedPaper;
            set
            {
                _selectedPaper = value;
                OnPropertyChanged(nameof(SelectedPaper));
            }
        }

        private bool _duplex;
        public bool Duplex
        {
            get => _duplex;
            set
            {
                _duplex = value;
                OnPropertyChanged(nameof(Duplex));
            }
        }


        private ICommand _printCommand;
        public ICommand PrintCommand
        {
            get
            {
                if (_printCommand == null)
                {
                    _printCommand = new RelayCommand(
                        p => true,
                        p => this.Print());
                }
                return _printCommand;
            }
        }

        private void Print()
        {
            string file = @"C:/AutoVote/Ballots/Bacavi.pdf";

            using (var document = PdfDocument.Load(file))
            {
                PrintDocument pd = document.CreatePrintDocument();

                // If none of the items are selected, don't do anything.
                if (pd == null || SelectedPrinter == null || SelectedTray == null || SelectedPaper == null) return;

                pd = PrintersService.GetPrintDocument(pd, SelectedPrinter, SelectedTray, SelectedPaper, Duplex);

                pd.Print();

                // Not using a USING statement; must Dispose().
                pd.Dispose();
            }
        }

        public MainWindowViewModel()
        {
            PrintersService = new PrinterService();
            Printers = PrintersService.GetPrinters();
            var trays = PrintersService.PrinterPaperSources;
            var papers = PrintersService.PrinterPaperKinds;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
