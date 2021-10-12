using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC.Models
{
    public class ViewTModel<T, Ts>
        where T : class where Ts : class
    {
        public List<T> Head { get; set; }
        public List<Ts> Body { get; set; }
    }
}
