using System;
using System.Collections.Generic;

namespace POC.Models
{
    public partial class Ordes
    {
        public string Noa { get; set; }
        public string Noq { get; set; }
        public string Pno { get; set; }
        public string Product { get; set; }
        public double? Sprice { get; set; }
        public double? Mount { get; set; }
        public double? Total { get; set; }
        public string Flavor { get; set; }
        public string Memo { get; set; }
    }
}
