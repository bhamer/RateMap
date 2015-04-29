using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMapping
{
    public class CsvToRates
    {
        public List<Rate> GetRatesFromFile(string path)
        {
            var rates = new List<Rate>();
            using (var reader = new StreamReader(path))
            {
                reader.ReadLine().Split(',');
                var row = new Dictionary<string, string>();
                while (!reader.EndOfStream)
                {
                    var rowData = reader.ReadLine().Split(',');
                    rates.Add(new Rate()
                        {
                            MoneyManagerSymbol = Convert.ToInt32(rowData[5]),
                            ProductGroup = Convert.ToInt32(rowData[2]),
                            CountryCode = Convert.ToInt32(rowData[1]),
                            LineOfBusiness = Convert.ToInt32(rowData[3]),
                            Id = Convert.ToInt32(rowData[0]),
                            rate = Convert.ToDecimal(rowData[4])
                        });
                }
            }
            return rates;
        }
    }
}
