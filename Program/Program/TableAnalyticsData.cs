using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class TableAnalyticsData
    {
        public int TableID { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public int TimesUsed { get; set; }
    }
}
