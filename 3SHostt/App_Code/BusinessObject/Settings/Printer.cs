using System;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Settings
{
    public class Printer : EntityClass
    {
        public string ShareName { get; set; }
        public int PrinterType { get; set; }
        public int CodePage { get; set; }
        public int CharsPerLine { get; set; }
        public int PageHeight { get; set; }

        public Printer()
        {
            CharsPerLine = 42;
            CodePage = 857;
        }
    }
}
