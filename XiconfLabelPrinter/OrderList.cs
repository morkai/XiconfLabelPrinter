// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace MSYS.Xiconf.LabelPrinter
{
    public class OrderList : List<Order>
    {
        public void ReadFromXlsxFile(string xlsxFilePath)
        {
            Clear();

            using (var package = new ExcelPackage(new FileInfo(xlsxFilePath)))
            {
                var worksheets = package.Workbook.Worksheets;

                if (worksheets.Count == 0)
                {
                    throw new Exception("Wybrany plik nie zawiera żadnych arkuszy!");
                }

                var worksheet = worksheets.First();
                var dimensions = worksheet.Dimension;

                if (dimensions == null)
                {
                    throw new Exception("Nie udało się rozpoznać wymiarów arkusza danych!");
                }

                var columns = Columns.FromExcelRange(worksheet.Cells);

                if (!columns.AllSet())
                {
                    throw new Exception("Nie znaleziono wszystkich wymaganych kolumn!");
                }

                var orderMap = new Dictionary<string, Order>();

                for (int row = 2, l = dimensions.End.Row; row <= l; ++row)
                {
                    var newOrder = Order.FromExcelRange(worksheet.Cells, row, columns);

                    if (newOrder == null)
                    {
                        continue;
                    }

                    var key = newOrder.No + newOrder.Nc12;

                    if (!orderMap.ContainsKey(key))
                    {
                        orderMap.Add(key, newOrder);
                    }
                    else if (newOrder.Date > orderMap[key].Date)
                    {
                        CopyTexts(orderMap[key], newOrder);

                        orderMap[key] = newOrder;
                    }
                    else
                    {
                        CopyTexts(newOrder, orderMap[key]);
                    }
                }

                AddRange(orderMap.Values);
            }

            if (Count == 0)
            {
                throw new Exception("W wybranym arkuszu nie wykryto żadnych zleceń.");
            }
        }

        private void CopyTexts(Order from, Order to)
        {
            if (string.IsNullOrEmpty(to.ProgramName) && !string.IsNullOrEmpty(from.ProgramName))
            {
                to.ProgramName = from.ProgramName;
            }

            if (string.IsNullOrEmpty(to.ResistText) && !string.IsNullOrEmpty(from.ResistText))
            {
                to.ResistText = from.ResistText;
            }
        }

        public IList<Order> FindByNo(string no)
        {
            return FindAll(order => order.No == no);
        }

        public IList<Order> FindByNc12(string nc12)
        {
            return FindAll(order => order.Nc12 == nc12);
        }

        public class Columns
        {
            public int Date { get; private set; }

            public int No { get; private set; }

            public int Nc12 { get; private set; }

            public int ProgramName { get; private set; }

            public int Quantity { get; private set; }

            public int WorkCenter { get; private set; }

            public static Columns FromExcelRange(ExcelRange excelRange)
            {
                var lastColumn = excelRange.Worksheet.Dimension.End.Column;
                var columns = new Columns();

                for (var column = 1; column < lastColumn && !columns.AllSet(); ++column)
                {
                    var columnName = excelRange[1, column].Text.Trim().ToUpperInvariant();
                    var firstValue = excelRange[2, column].Text.Trim();

                    if (columns.Date == -1 && columnName.Contains("REQ") && columnName.Contains("DATE"))
                    {
                        columns.Date = column;
                    }
                    else if (columns.No == -1 && columnName.Contains("ORDER") && firstValue.Length == 9)
                    {
                        columns.No = column;
                    }
                    else if (columns.ProgramName == -1 && columnName.Contains("MATERIAL") && columnName.Contains("DESC"))
                    {
                        columns.ProgramName = column;
                    }
                    else if (columns.Nc12 == -1 && columnName.Contains("MATERIAL") && firstValue.Length == 15)
                    {
                        columns.Nc12 = column;
                    }
                    else if (columns.Quantity == -1 && columnName.Contains("REQ") && columnName.Contains("QUANTITY") && Regex.IsMatch(firstValue, "^[0-9]+$"))
                    {
                        columns.Quantity = column;
                    }
                    else if (columns.WorkCenter == -1 && columnName.Contains("WORK") && columnName.Contains("CENTER") && !firstValue.Contains(' '))
                    {
                        columns.WorkCenter = column;
                    }
                }

                return columns;
            }

            public Columns()
            {
                Date = -1;
                No = -1;
                Nc12 = -1;
                ProgramName = -1;
                Quantity = -1;
                WorkCenter = -1;
            }

            public bool AllSet()
            {
                return Date != -1 && No != -1 && Nc12 != -1 && ProgramName != -1 && Quantity != -1 && WorkCenter != -1;
            }
        }
    }
}
