// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public class ServiceTagPrintJob : APrintJob
    {
        public override string LabelFormatType { get { return "serviceTag"; } }

        public override string AdditionalInfo
        {
            get
            {
                return string.Format("Nr zlecenia: {0}; Nr początkowy: {1}", orderNo, firstSerialNo);
            }
        }

        private string orderNo;

        private int firstSerialNo;

        public ServiceTagPrintJob(string orderNo, int firstSerialNo)
        {
            this.orderNo = orderNo;
            this.firstSerialNo = firstSerialNo;
        }

        protected override void SetPrintSetup(LabelFormatDocument labelFormat)
        {
            labelFormat.PrintSetup.PrinterName = PrinterName;
            labelFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
            labelFormat.PrintSetup.NumberOfSerializedLabels = LabelQuantity;
        }

        protected override void SetSubStrings(LabelFormat labelFormat)
        {
            labelFormat.SubStrings.SetSubString("OrderNo", orderNo);
            labelFormat.SubStrings.SetSubString("FirstSerialNo", firstSerialNo.ToString().PadLeft(4, '0'));
        }
    }
}
