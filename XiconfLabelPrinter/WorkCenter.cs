// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace MSYS.Xiconf.LabelPrinter
{
    public class WorkCenter
    {
        private static Regex IS_ACTIVE_REGEX = new Regex("^(1|T(ak|rue)?|Y(es)?)$", RegexOptions.IgnoreCase);

        public string Id { get; set; }

        public int ProdLineCount { get; set; }

        public int MinDivQuantity { get; set; }

        public static WorkCenter FromExcelRange(ExcelRange excelRange, int row)
        {
            var id = excelRange[row, 1].Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            var prodLineCount = 0;

            Int32.TryParse(excelRange[row, 2].Text.Trim(), out prodLineCount);

            if (prodLineCount < 1)
            {
                return null;
            }

            var minDivQuantity = 0;

            Int32.TryParse(excelRange[row, 3].Text.Trim(), out minDivQuantity);

            if (minDivQuantity < 1)
            {
                return null;
            }

            var isActive = excelRange[row, 4].Text.Trim().ToUpperInvariant();

            if (!IS_ACTIVE_REGEX.IsMatch(isActive))
            {
                return null;
            }

            return new WorkCenter()
            {
                Id = id,
                ProdLineCount = prodLineCount,
                MinDivQuantity = minDivQuantity
            };
        }
    }
}
