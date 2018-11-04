using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCalcs
{
    public class Section
    {

        double Ix { get; set; }
        string Name { get; set; }
        
        public static List<Section> openSteelSectionFile(string filePath)
        {
            List<Section> returnList = new List<Section>();
            string line = "";
            string[] lineItems;
            var fs = File.OpenRead(filePath);
            var reader = new StreamReader(fs);
            reader.ReadLine();//assumes first row is headers
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                lineItems = line.Split(',');
                returnList.Add(
                    new Section()
                    {
                        Ix = Convert.ToDouble(lineItems[4]),
                        Name = lineItems[0]
                    }

                        );
            }
            return returnList;
        }
    }
}
