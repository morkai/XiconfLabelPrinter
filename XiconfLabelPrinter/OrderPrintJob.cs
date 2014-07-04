// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public class OrderPrintJob : APrintJob
    {
        public override string LabelFormatType { get { return "order"; } }

        public override string AdditionalInfo
        {
            get
            {
                return string.Format("Nr zlecenia: {0}; Ilość: {1}", orderNo, orderQuantity);
            }
        }

        private string orderNo;

        private int orderQuantity;

        public OrderPrintJob(string orderNo, int orderQuantity)
        {
            this.orderNo = orderNo;
            this.orderQuantity = orderQuantity;
        }

        protected override void SetSubStrings(LabelFormat labelFormat)
        {
            labelFormat.SubStrings.SetSubString("nrzlec_ilosc", orderNo + "-" + orderQuantity.ToString().PadLeft(3, '0'));
        }
    }
}
