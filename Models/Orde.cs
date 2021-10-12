using System;
using System.Collections.Generic;

namespace POC.Models
{
    public partial class Orde
    {
        public string Noa { get; set; }
        public string Datea { get; set; }
        public string Odate { get; set; }
        public string Cust { get; set; }
        public string Gender { get; set; }
        public string Addr { get; set; }
        public string Trantype { get; set; }
        public bool? Enda { get; set; }
        public double? Total { get; set; }
        public double? Discount { get; set; }
        public string Memo { get; set; }
        public double? Money { get; set; }
    }
}
