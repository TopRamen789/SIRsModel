using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    class GridUnit
    {

        private double _s;
        private double _i;
        private double _r;

        public long S { get; set; } // number of susceptible individuals
        public long I { get; set; } // number of infected individuals
        public long R { get; set; } // number of recovered individuals
        public long D { get; set; } // number of deaths
        public long N { get; set; } // total Population, initially set by population density info
        public int xCoord;
        public int yCoord;

        public double s
        {
            get { return _s; }
            set
            {
                _s = value;
                if (value > 1)
                {
                    S = N;
                }
                else
                {
                    S = Convert.ToInt64(value * N);
                }
                
            } 
        }

        public double i {
            get { return _i; }
            set
            {
                _i = value;
                if (value > 1)
                {
                    I = N;
                }
                else
                {
                    I = Convert.ToInt64(value * N);
                }
            }
        }
        public double r {
            get { return _r; }
            set
            {
                _r = value;
                if (value > 1)
                {
                    R = N;
                }
                else
                {
                    R = Convert.ToInt64(value * N);
                }
            }
        }

        public GridUnit(long S, long I, long R, long N, int xCoord, int yCoord)
        {
            this.s = S / N;
            this.i = I / N;
            this.r = R / N;
            this.N = N;
            D = 0;
            this.xCoord = xCoord;
            this.yCoord = yCoord;

        }

    }
}
