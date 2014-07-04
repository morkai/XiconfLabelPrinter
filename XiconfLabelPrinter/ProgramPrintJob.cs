// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using Seagull.BarTender.Print;

namespace MSYS.Xiconf.LabelPrinter
{
    public class ProgramPrintJob : APrintJob
    {
        public override string LabelFormatType { get { return "program"; } }

        public override string AdditionalInfo
        {
            get
            {
                return string.Format("12NC: {0}; Program: {1}", nc12, programName);
            }
        }

        private string nc12;

        private string programName;

        public ProgramPrintJob(string nc12, string programName)
        {
            this.nc12 = nc12;
            this.programName = programName;
        }

        protected override void SetSubStrings(LabelFormat labelFormat)
        {
            labelFormat.SubStrings.SetSubString("Program_12NC", nc12);
            labelFormat.SubStrings.SetSubString("Program_Name", programName);
        }
    }
}
