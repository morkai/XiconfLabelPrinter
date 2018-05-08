// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public class ResistPrintJob : APrintJob
    {
        public override string LabelFormatType { get { return "resist"; } }

        public override string AdditionalInfo
        {
            get
            {
                return string.Format("Nr zlecenia: {0}; Tekst: {1}", orderNo, text);
            }
        }

        private string orderNo;

        private string text;

        public ResistPrintJob(string orderNo, string text)
        {
            this.orderNo = orderNo;
            this.text = text;
        }

        protected override void SetSubStrings(LabelFormat labelFormat)
        {
            labelFormat.SubStrings.SetSubString("OrderNo", orderNo);
            labelFormat.SubStrings.SetSubString("Text", text);
        }
    }
}
