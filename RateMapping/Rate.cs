using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMapping
{
    public class Rate
    {
        public int MoneyManagerSymbol { get; set; }
        public int ProductGroup { get; set; }
        public int CountryCode { get; set; }
        public int LineOfBusiness { get; set; }
        public decimal rate { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1} - {2} - {3} - {4}  - {5}", Id, MoneyManagerSymbol, ProductGroup, CountryCode, LineOfBusiness, rate);
        }
    }
}
