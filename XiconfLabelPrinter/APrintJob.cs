// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Linq;
using System.Text;
using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    abstract public class APrintJob : IPrintJob
    {
        abstract public string LabelFormatType { get; }

        abstract public string AdditionalInfo { get; }

        abstract protected void SetSubStrings(LabelFormat labelFormat);

        private static int JOB_COUNTER = 1;

        private int jobId = 0;

        public string PrinterName { get; set; }

        public int LabelQuantity { get; set; }

        public int SeriesNo { get; set; }

        public int SeriesGroup { get; set; }

        public string SeriesId
        {
            get
            {
                return SeriesNo == 0 ? "0" : Convert.ToChar(64 + SeriesGroup).ToString() + SeriesNo;
            }
        }

        public string PrintJobName
        {
            get
            {
                if (jobId == 0)
                {
                    jobId = JOB_COUNTER++;
                }

                return LabelFormatType + "-" + jobId;
            }
        }

        public void Print(LabelFormatDocument labelFormat)
        {
            SetPrintSetup(labelFormat);
            SetSubStrings(labelFormat);

            Messages messages;

            var result = labelFormat.Print(PrintJobName, out messages);

            if (result == Result.Success)
            {
                return;
            }

            if (result == Result.Timeout)
            {
                throw new PrintException("Upłynął czas oczekiwania na odpowiedź.");
            }

            HandleFailure(messages);
        }

        protected virtual void SetPrintSetup(LabelFormatDocument labelFormat)
        {
            labelFormat.PrintSetup.PrinterName = PrinterName;
            labelFormat.PrintSetup.IdenticalCopiesOfLabel = LabelQuantity;
        }

        private void HandleFailure(Messages messages)
        {
            var errorList = messages.Where(message => message.Severity == MessageSeverity.Error);
            var errorCount = errorList.Count();

            if (errorCount == 0)
            {
                throw new PrintException();
            }

            var errorMessage = new StringBuilder();
            var i = 0;

            foreach (var error in errorList)
            {
                if (++i == errorCount)
                {
                    errorMessage.Append(error.Text);
                }
                else
                {
                    errorMessage.AppendLine(error.Text);
                }
            }

            throw new PrintException(errorMessage.ToString());
        }
    }
}
