// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace MSYS.Xiconf.LabelPrinter
{
    public class Order
    {
        private static Regex NO_REGEX = new Regex("^[0-9]{9}$", RegexOptions.None);

        private static Regex NC12_REGEX = new Regex("^(?:000)([0-9]{12})$", RegexOptions.None);

        private static Regex PROGRAM_NAME_REGEX = new Regex("^LABEL.*?PROGRAM(.*?)(?:\"|')?$", RegexOptions.IgnoreCase);

        public DateTime Date { get; set; }

        public string No { get; set; }

        public string Nc12 { get; set; }

        public string ProgramName { get; set; }

        public int Quantity { get; set; }

        public string WorkCenter { get; set; }

        public static Order FromExcelRange(ExcelRange excelRange, int row, OrderList.Columns columns)
        {
            var dateOrTime = excelRange[row, columns.Date].Value;
            DateTime date;

            if (dateOrTime is DateTime)
            {
                date = (DateTime)dateOrTime;
            }
            else if (dateOrTime is double)
            {
                date = DateTime.FromOADate((double)dateOrTime);
            }
            else
            {
                return null;
            }

            var no = excelRange[row, columns.No].Text.Trim();

            if (!NO_REGEX.IsMatch(no))
            {
                return null;
            }

            var nc12Matches = NC12_REGEX.Match(excelRange[row, columns.Nc12].Text.Trim());

            if (!nc12Matches.Success)
            {
                return null;
            }

            var nc12 = nc12Matches.Groups[1].Value;

            var programNameMatches = PROGRAM_NAME_REGEX.Match(excelRange[row, columns.ProgramName].Text.Trim());

            if (!programNameMatches.Success)
            {
                return null;
            }

            var programName = "PROGRAM " + programNameMatches.Groups[1].Value.Trim();

            if (programName.Contains("\""))
            {
                programName = programName.Substring(0, programName.IndexOf('"'));
            }
            else if (programName.Contains("'"))
            {
                programName = programName.Substring(0, programName.IndexOf('\''));
            }

            var quantity = 0;

            Int32.TryParse(excelRange[row, columns.Quantity].Text.Trim(), out quantity);

            if (quantity < 1)
            {
                return null;
            }

            var workCenter = excelRange[row, columns.WorkCenter].Text.Trim();

            return new Order()
            {
                Date = date,
                No = no,
                Nc12 = nc12,
                ProgramName = programName,
                Quantity = quantity,
                WorkCenter = workCenter
            };
        }
    }
}
