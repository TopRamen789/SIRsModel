using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirsModel
{
    public class DataParser
    {
        public void parseCSV(string input, string output)
        {
            using (StreamWriter outputFile = new StreamWriter(output))
            {
                using (StreamReader inputFile = new StreamReader(input))
                {
                    int topLattitude = 500;
                    int bottomLattitude = 250;
                    int westernLongitude = 550; // actual -1250
                    int easternLongitude = 1140; // actual -660
                    string[] lineArr;
                    string line;
                    string writeLine = String.Empty;
                    int lineCounter = 850;
                    double populationSum = 0;
                    while ((line = inputFile.ReadLine()) != null && lineCounter > topLattitude)
                    {
                        lineCounter--; // remove the top data
                    }
                    while ((line = inputFile.ReadLine()) != null && lineCounter > bottomLattitude)
                    {
                        writeLine = String.Empty;
                        lineArr = line.Split(',');
                        for (int i = 0; i < lineArr.Length; i++)
                        {
                            if (i < westernLongitude)
                            {
                                // do nothing
                            }
                            else if (i > easternLongitude)
                            {
                                // do nothing
                            }
                            else
                            {
                                double stupid = 0;
                                if (lineArr[i] != "99999.0" )
                                {
                                    Double.TryParse(lineArr[i], out stupid);
                                    populationSum += stupid;
                                }
                                
                                writeLine += lineArr[i] + ",";

                            }
                        }
                        outputFile.WriteLine(writeLine);
                        lineCounter--;
                    }
                }
            }
        }
    }
}
