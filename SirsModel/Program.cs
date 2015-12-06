using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    class Program
    {

        public static double deltaT { get; set; } = 1;
        public static double totalT { get; set; } = 10;
        public static double sumT = 0;
        public static float k { get; set; } = .05F; // fraction of infected group that will recover
        public static float b { get; set; } = 20;
        public static float deathRate { get; set; } = .02F;// odds of an infected person dying after leaving the infected group (infectious, really)
        public static float borderTravelRate = 01F;
        public static List<GridUnit> activeGridUnits = new List<GridUnit>();
        public static int gridXLength = 3;
        public static int gridYLength = 3;
        public static GridUnit[,] array = new GridUnit[gridXLength, gridYLength];

        static void Main(string[] args)
        {

            // fill the array
            
            for (int i = 0; i < gridXLength; i++)
            {
                for (int j = 0; j < gridYLength; j++)
                {
                    array[i, j] = new GridUnit(10000, 0, 0, 10000, i, j);
                }
            }

            array[1, 2].i = Convert.ToDouble(1000.0 / array[1, 2].N);
            activeGridUnits.Add(array[1, 2]);

        


            // figure out the beginning GridUnit and add it to activeGridUnits


            while (sumT < totalT) 
            {

                //for each grid, call doIteration if I is not 0;
                for (int i = 0; i < activeGridUnits.Count - 1; i++)
                {
                    GridUnit temp = doIteration(activeGridUnits.ElementAt(i), deltaT);
                    overWriteGrid(i, temp);
                }

                //for each grid, call calculateSpreadToNeighbors() on it
                for (int i = 0; i <activeGridUnits.Count - 1; i++)
                {
                    calculateSpreadToNeighbors(activeGridUnits.ElementAt(i), borderTravelRate);
                }

                
                sumT += deltaT;
            }


        }

        public static GridUnit doIteration(GridUnit grid, double deltaT)
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
            grid.N -= theDead; // remove losses from total population, needs testing.]

            if (grid.I <= 0 || grid.N <= 0 || grid.R == grid.N)
            {
                activeGridUnits.Remove(grid);
            }

            return grid;
        }

        public static void calculateSpreadToNeighbors(GridUnit centerGrid, float borderTravelRate) // spreads infection out, one way, from each infected grid
        {


            long infectedOut = Convert.ToInt64(centerGrid.i * borderTravelRate * centerGrid.N);
            long recoveredOut = Convert.ToInt64(centerGrid.r * borderTravelRate * centerGrid.N);
            long susceptableOut = Convert.ToInt64(centerGrid.r * borderTravelRate * centerGrid.N);

            // readjust populations due to people leaving
            
            // calculate each direction
            if (centerGrid.yCoord <= 0) // go down
            {
                GridUnit target = array[centerGrid.xCoord, centerGrid.yCoord - 1];
                if (target.N > 0)
                {
                    if (!(activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += infectedOut / target.N;
                    target.r += recoveredOut / target.N;
                    target.s += susceptableOut / target.N;

                    overWriteGrid(target);

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= infectedOut / centerGrid.N;
                    centerGrid.r -= recoveredOut / centerGrid.N;
                    centerGrid.s -= susceptableOut / centerGrid.N;

                    overWriteGrid(centerGrid);
                }
            }
            if (centerGrid.yCoord >= gridYLength - 1) // go up
            {
                GridUnit target = array[centerGrid.xCoord, centerGrid.yCoord + 1];
                if (target.N > 0)
                {

                    if (!(activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += infectedOut / target.N;
                    target.r += recoveredOut / target.N;
                    target.s += susceptableOut / target.N;

                    overWriteGrid(target);

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= infectedOut / centerGrid.N;
                    centerGrid.r -= recoveredOut / centerGrid.N;
                    centerGrid.s -= susceptableOut / centerGrid.N;

                    overWriteGrid(centerGrid);
                }
            }
            if (centerGrid.xCoord <= 0) // go left
            {
                GridUnit target = array[centerGrid.xCoord - 1, centerGrid.yCoord];
                if (target.N > 0)
                {
                    if (!(activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += infectedOut / target.N;
                    target.r += recoveredOut / target.N;
                    target.s += susceptableOut / target.N;

                    overWriteGrid(target);

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= infectedOut / centerGrid.N;
                    centerGrid.r -= recoveredOut / centerGrid.N;
                    centerGrid.s -= susceptableOut / centerGrid.N;

                    overWriteGrid(centerGrid);
                }
            }
            if (centerGrid.xCoord >= gridXLength - 1) // go right
            {
                GridUnit target = array[centerGrid.xCoord + 1, centerGrid.yCoord];
                if (target.N > 0)
                {
                    if (!(activeGridUnits.Contains(target)))
                    {
                        activeGridUnits.Add(target);
                    }

                    target.N += infectedOut + recoveredOut + susceptableOut;
                    target.i += infectedOut / target.N;
                    target.r += recoveredOut / target.N;
                    target.s += susceptableOut / target.N;

                    overWriteGrid(target);

                    centerGrid.N -= infectedOut + recoveredOut + susceptableOut;
                    centerGrid.i -= infectedOut / centerGrid.N;
                    centerGrid.r -= recoveredOut / centerGrid.N;
                    centerGrid.s -= susceptableOut / centerGrid.N;

                    overWriteGrid(centerGrid);
                }
            }
        }
        public static void overWriteGrid(int i, GridUnit temp) 
        {
            activeGridUnits.ElementAt(i).N = temp.N;
            activeGridUnits.ElementAt(i).i = temp.i;
            activeGridUnits.ElementAt(i).r = temp.r;
            activeGridUnits.ElementAt(i).s = temp.s;
            activeGridUnits.ElementAt(i).D = temp.D;
        } 
        public static void overWriteGrid(GridUnit temp)
        {
            array[temp.xCoord, temp.yCoord].N = temp.N;
            array[temp.xCoord, temp.yCoord].i = temp.i;
            array[temp.xCoord, temp.yCoord].r = temp.r;
            array[temp.xCoord, temp.yCoord].s = temp.s;
            array[temp.xCoord, temp.yCoord].D = temp.D;
        }
    }
}
