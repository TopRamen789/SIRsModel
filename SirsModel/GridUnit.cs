using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    class GridUnit
    {
        public long S { get; set; } // number of susceptible individuals
        public long I { get; set; } // number of infected individuals
        public long R { get; set; } // number of recovered individuals
        public long N { get; set; } // total Population, initially set by population density info




    }
}
