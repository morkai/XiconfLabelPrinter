// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;

namespace MSYS.Xiconf.LabelPrinter
{
    public class PrintException : Exception
    {
        public PrintException()
            : base()
        {
        }

        public PrintException(string message)
            : base(message)
        {
        }
    }
}
