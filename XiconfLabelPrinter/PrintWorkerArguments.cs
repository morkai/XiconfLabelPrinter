// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public class PrintWorkerArguments
    {
        private LabelFormatDocument orderFormat;

        private LabelFormatDocument programFormat;

        private LabelFormatDocument serviceTagFormat;

        public IPrintJob PrintJob { get; private set; }

        public Engine Engine { get; private set; }

        public LabelFormatDocument LabelFormat
        {
            get
            {
                switch (PrintJob.LabelFormatType)
                {
                    case "order":
                        return orderFormat;

                    case "program":
                        return programFormat;

                    case "serviceTag":
                        return serviceTagFormat;

                    default:
                        return null;
                }
            }
        }

        public PrintWorkerArguments(IPrintJob printJob, Engine engine, LabelFormatDocument orderFormat, LabelFormatDocument programFormat, LabelFormatDocument serviceTagFormat)
        {
            this.orderFormat = orderFormat;
            this.programFormat = programFormat;
            this.serviceTagFormat = serviceTagFormat;
            PrintJob = printJob;
            Engine = engine;
        }
    }
}
