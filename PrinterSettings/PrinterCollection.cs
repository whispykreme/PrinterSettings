using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;

namespace PrinterSettings
{
    public class PrinterCollection
    {
        public PrinterCollection()
        {
            var server = new LocalPrintServer();
            var printerQueues = server.GetPrintQueues();

            List<LocalPrinter> localPrinters = new List<LocalPrinter>();

            foreach (var printer in printerQueues )
            {
                LocalPrinter currentPrinter = new LocalPrinter() { Name = printer.Name };
                List<Feature> currentFeaturesList = new List<Feature>();

                using (var ms = printer.GetPrintCapabilitiesAsXml())
                {
                    XmlReader reader = new XmlTextReader(ms);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.IsStartElement() && reader.Name == "psf:Feature")
                            {
                                reader.MoveToElement();
                                Feature currentPrinterFeatures = StepIntoFeature(reader);
                                currentFeaturesList.Add(currentPrinterFeatures);
                            }
                        }
                    }
                    currentPrinter.Features = currentFeaturesList;
                }

                localPrinters.Add(currentPrinter);
            }
            
        }

        private Feature StepIntoFeature(XmlReader reader)
        {
            string propertyName = "";
            List<string> options = new List<string>();

            XmlReader featureReader = reader.ReadSubtree();

            bool isOption = false;
            bool isDisplayName = false;

            while (featureReader.Read())
            {
                if (featureReader.IsStartElement())
                {
                    if (featureReader.Name == "psf:Option")
                    {
                        isOption = true;
                    }
                    if (featureReader.Name == "psf:Property")
                    {
                        featureReader.MoveToFirstAttribute();
                        if (featureReader.Value == "psk:DisplayName")
                        {
                            isDisplayName = true;
                        }
                    }

                    if (featureReader.Name == "psf:Value")
                    {
                        // Option
                        if (isDisplayName && isOption)
                        {
                            featureReader.MoveToFirstAttribute();
                            featureReader.MoveToContent();
                            string val = featureReader.ReadInnerXml();
                            options.Add(val);
                            //currentFeature.Options.Append(val);

                            isOption = false;
                            isDisplayName = false;
                        }
                        // Property
                        else if(isDisplayName && !isOption)
                        {
                            featureReader.MoveToFirstAttribute();
                            featureReader.MoveToContent();
                            propertyName = featureReader.ReadInnerXml();
                            //currentFeature.Property = featureReader.ReadInnerXml();

                            isDisplayName = false;
                        }
                    }
                }
            }

            return new Feature() 
            { 
                Property = propertyName,
                Options = options,
            };
        }

        private void WorkingExample()
        {
            var server = new LocalPrintServer();
            var printerQueues = server.GetPrintQueues();

            foreach (var printerQueue in printerQueues)
            {
                PrintTicket ticket = printerQueue.DefaultPrintTicket;
                PrintCapabilities capabilities = printerQueue.GetPrintCapabilities();


                if (printerQueue.FullName.Contains("Brother HL-L2360D"))
                {
                    using (var ms = printerQueue.GetPrintCapabilitiesAsXml())
                    {
                        XmlReader reader = new XmlTextReader(ms);

                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                // Find where an Element is named "psf:Feature"
                                case XmlNodeType.Element:
                                    if (reader.IsStartElement() && reader.Name == "psf:Feature")
                                    {
                                        // If the next attribute is "psk:PageInputBin" then we are in the right place.
                                        reader.MoveToNextAttribute();
                                        if (reader.Value == "psk:PageInputBin")
                                        {
                                            reader.MoveToElement();
                                            XmlReader featureReader = reader.ReadSubtree();

                                            while (featureReader.Read())
                                            {
                                                if (featureReader.HasAttributes)
                                                {
                                                    if (featureReader.Name == "psf:Option")
                                                    {
                                                        featureReader.MoveToElement();
                                                        XmlReader optionReader = featureReader.ReadSubtree();

                                                        while (optionReader.Read())
                                                        {
                                                            switch (optionReader.NodeType)
                                                            {
                                                                case XmlNodeType.Text:
                                                                    //printerTrays.Add(optionReader.Value);
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            // I only need to continue reading until printerTrays is filled with a value
                            //if (printerTrays.Count > 0) { break; }
                        }
                    }
                }
            }
        }
    }
}
