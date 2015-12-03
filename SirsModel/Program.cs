using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    class Program
    {

        public static double deltaT { get; set; }
        public static double totalT { get; set; } = 0;
        public static double sumT = 0;
        public float k { get; set; } // fraction of infected group that will recover
        public float b { get; set; }
        public float deathRate { get; set; } // odds of an infected person dying after leaving the infected group (infectious, really)
        public float borderTravelRate;

        static void Main(string[] args)
        {
            while (sumT < totalT) 
            {


                //for each grid, call doIteration if I is not 0;


                //for each grid, call calculateSpreadToNeighbors() on it and its right and then bottom neighbor after checking for edges


                sumT += deltaT;
            }


        }

        public void doIteration(ref GridUnit grid, double deltaT)
        {

            /*
            S = S(t)	is the number of susceptible individuals,
            I = I(t)	is the number of infected individuals, and
            R = R(t)	is the number of recovered individuals.

            s(t) = S(t)/N,	the susceptible fraction of the population,
            i(t) = I(t)/N,	the infected fraction of the population, and
            r(t) = R(t)/N,	the recovered fraction of the population.

            S' = -b s(t)I(t)
            s' = -b s(t)i(t)
            r' = k i(t)
            s' + i' + r' = 0
            i' = b s(t) i(t) - k i(t)

            */


            // calculate the rate of change for this iteration
            double sPrime = -(b) * grid.s * grid.i;
            double iPrime = b * grid.s * grid.i - (k * grid.i);
            double rPrime = k * grid.i;

            // apply the change to the population 
            grid.s += Convert.ToInt64(sPrime * deltaT);
            grid.i += Convert.ToInt64(iPrime * deltaT);
            grid.r += Convert.ToInt64(rPrime * deltaT);

            // calculate losses
            long theDead = Convert.ToInt64(rPrime * deltaT * deathRate);
            grid.D += theDead;
            grid.N -= theDead; // remove losses from total population, needs testing.


        }

        public void calculateSpreadToNeighbors(ref GridUnit unitOne, ref GridUnit unitTwo, float borderTravelRate)
        {
            long infectedToOne = Convert.ToInt64(unitTwo.i * borderTravelRate * unitTwo.N); // actual number of people moving
            long infectedToTwo = Convert.ToInt64(unitOne.i * borderTravelRate * unitOne.N);

            long susceptableToOne = Convert.ToInt64(unitTwo.s * borderTravelRate * unitTwo.N);
            long susceptableToTwo = Convert.ToInt64(unitOne.s * borderTravelRate * unitOne.N);

            long recoveredToOne = Convert.ToInt64(unitTwo.r * borderTravelRate * unitTwo.N);
            long recoveredToTwo = Convert.ToInt64(unitOne.r * borderTravelRate * unitOne.N);


            // readjust populations due to people leaving
            long unitOneNTemp = unitOne.N;
            unitOne.N = unitOne.N + (infectedToOne + susceptableToOne + recoveredToOne - infectedToTwo - susceptableToTwo - recoveredToTwo); 
            unitTwo.N = unitTwo.N + (infectedToTwo + susceptableToTwo + recoveredToTwo - infectedToOne - susceptableToOne - recoveredToOne);


            // update fractions
            unitOne.i += infectedToOne / unitOne.N;
            unitTwo.i += infectedToTwo / unitTwo.N;

            unitOne.r += recoveredToOne / unitOne.N;
            unitTwo.r += recoveredToTwo / unitTwo.N;

            unitOne.s += susceptableToOne / unitOne.N;
            unitTwo.s += susceptableToTwo / unitTwo.N;


        }


    }
}
