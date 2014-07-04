// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public interface IPrintJob
    {
        string LabelFormatType { get; }

        int LabelQuantity { get; }

        string AdditionalInfo { get; }

        string PrinterName { get; }

        string PrintJobName { get; }

        int SeriesNo { get; }

        int SeriesGroup { get; }

        string SeriesId { get; }

        void Print(LabelFormatDocument labelFormat);
    }
}
