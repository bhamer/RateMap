using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace RateMapping.Test
{
    [TestClass]
    public class RateMapTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var parser = new CsvToRates();
            var rates = parser.GetRatesFromFile(@"C:\temp\RateMappingSet.csv");
            var map = new RateMap(rates);

            var rate = map.FindOptimalRate(1, 1, 1, 1);
            Console.WriteLine(rate.ToString());


            rate = map.FindOptimalRate(142, 1, 77, 4);
            Console.WriteLine(rate.Select(x=>x.ToString()));

            rate = map.FindOptimalRate(142, 1, 77, 0);
            Console.WriteLine(rate.Select(x => x.ToString()));

            rate = map.FindOptimalRate(0, 1, 77, 4);
            Console.WriteLine(rate.Select(x => x.ToString()));

            rate = map.FindOptimalRate(142, 0, 77, 4);
            Console.WriteLine(rate.Select(x => x.ToString()));

            // This one results in a tie so there should be multiple rates returned
            rate = map.FindOptimalRate(142, 0, 0, 4);
            Console.WriteLine(rate.Select(x => x.ToString()));
        }
    }
}
