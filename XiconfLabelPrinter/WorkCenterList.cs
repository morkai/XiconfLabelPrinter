// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace MSYS.Xiconf.LabelPrinter
{
    public class WorkCenterList : List<WorkCenter>
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

                for (int row = 2, l = dimensions.End.Row; row <= l; ++row)
                {
                    var workCenter = WorkCenter.FromExcelRange(worksheet.Cells, row);

                    if (workCenter != null)
                    {
                        Add(workCenter);
                    }
                }
            }

            if (Count == 0)
            {
                throw new Exception("W wybranym arkuszu nie wykryto żadnych WorkCentrów.");
            }
        }
    }
}
