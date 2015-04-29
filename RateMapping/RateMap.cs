using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateMapping
{
    /// <summary>
    /// Trie-like data structure for finding a best-match Rate given a set of attributes.
    /// 
    /// Requirements:
    /// Fast search time. Search will be called many times in loop.
    /// 
    /// Details:
    /// A perfect match is not required and attribute types have a priority so order matters.
    /// In the table below, attributes { A, B, C, D } are ordered by priority from left to right. 
    /// If A matches, it would be a better match than matching on B, C, and D but not A.
    /// For example, if the table below is the universe of known Rates with their associated attribute set
    /// and we're trying to find a best-match rate for a set of attributes { 1, 5, 3, 2 }
    /// the best match would be { 1, 1, 3, 1, .03 } since this { 1, *, 3, * } is a better match than { 1, *, *, 2 }. 
    /// A, B, C, D, Rate
    /// 1, 1, 1, 4, .01
    /// 1, 2, 1, 2, .02
    /// 1, 1, 3, 1, .03
    /// 2, 4, 4, 4, .005
    /// 
    /// 
    /// Performance: where n = # of rates and k = # of attributes associated with rates (k = 4 as it is now)
    /// Search: O(k)
    /// Build: O(n * k)
    /// Space: O(n * k)
    /// </summary>

    public class RateMap
    {
        // Using -1 as generic path key
        private int genericKey = -1;

        private Dictionary<int, object> masterPath;

        public RateMap(IEnumerable<Rate> rates)
        {
            masterPath = BuildPathsOuter(rates);
        }

        public void RebuildMapping(IEnumerable<Rate> rates)
        {
            masterPath = BuildPathsOuter(rates);
        }

        // Add, remove, and prioritize the Rate keys in this method
        private Queue<int> BuildRateKeyQueue(Rate rate)
        {
            var keyQueue = new Queue<int>();

            // 1
            keyQueue.Enqueue(rate.MoneyManagerSymbol);

            // 2
            keyQueue.Enqueue(rate.ProductGroup);

            // 3
            keyQueue.Enqueue(rate.CountryCode);

            // 4
            keyQueue.Enqueue(rate.LineOfBusiness);

            return keyQueue;
        }


        /// <returns>List of Rates because it's possible to have a tie.</returns>
        public List<Rate> FindOptimalRate(int mm, int pg, int cc, int lob)
        {
            var temp = new Rate()
            {
                MoneyManagerSymbol = mm,
                ProductGroup = pg,
                CountryCode = cc,
                LineOfBusiness = lob
            };
            var keyQueue = BuildRateKeyQueue(temp);

            return FindOptimalRateInner(keyQueue, masterPath);
        }


        private List<Rate> FindOptimalRateInner(Queue<int> keyQueue, Dictionary<int, object> path)
        {
            int key = keyQueue.Dequeue();

            if (keyQueue.Count < 1)
            {
                object rate = null;
                path.TryGetValue(key, out rate);
                if (rate == null)
                {
                    path.TryGetValue(genericKey, out rate);
                }
                return (List<Rate>)rate;
            }

            if (path.ContainsKey(key))
            {
                return FindOptimalRateInner(keyQueue, (Dictionary<int, object>)path[key]);
            }
            return FindOptimalRateInner(keyQueue, (Dictionary<int, object>)path[genericKey]);
        }


        private Dictionary<int, object> BuildPathsOuter(IEnumerable<Rate> rates)
        {
            var master = new Dictionary<int, object>();
            master.Add(genericKey, new Dictionary<int, object>());

            foreach (var rate in rates)
            {
                var keyQueue = BuildRateKeyQueue(rate);

                int key = keyQueue.Dequeue();
                if (!master.ContainsKey(key))
                {
                    master.Add(key, new Dictionary<int, object>());
                }

                BuildPathsInner(rate, keyQueue, (Dictionary<int, object>)master[key], (Dictionary<int, object>)master[genericKey]);
            }

            return master;
        }


        private void BuildPathsInner(Rate rate, Queue<int> keyQueue, Dictionary<int, object> specificPath, Dictionary<int, object> genericPath)
        {
            int key = keyQueue.Dequeue();

            if (keyQueue.Count < 1)
            {
                FinalStop(rate, key, specificPath, genericPath);
                return;
            }

            // create paths if they don't exist
            if (!specificPath.ContainsKey(key))
            {
                specificPath.Add(key, new Dictionary<int, object>());
            }

            if (!specificPath.ContainsKey(genericKey))
            {
                specificPath.Add(genericKey, new Dictionary<int, object>());
            }

            if (!genericPath.ContainsKey(key))
            {
                genericPath.Add(key, new Dictionary<int, object>());
            }

            if (!genericPath.ContainsKey(genericKey))
            {
                genericPath.Add(genericKey, new Dictionary<int, object>());
            }

            BuildPathsInner(rate, new Queue<int>(keyQueue), (Dictionary<int, object>)specificPath[key], (Dictionary<int, object>)specificPath[genericKey]);
            BuildPathsInner(rate, new Queue<int>(keyQueue), (Dictionary<int, object>)genericPath[key], (Dictionary<int, object>)genericPath[genericKey]);
        }


        // Final stop contains the reference to the rate object
        private void FinalStop(Rate rate, int key, Dictionary<int, object> specificPath, Dictionary<int, object> genericPath)
        {
            if (!specificPath.ContainsKey(key))
            {
                specificPath.Add(key, new List<Rate>());
            }
            ((List<Rate>)specificPath[key]).Add(rate);

            if (!specificPath.ContainsKey(genericKey))
            {
                specificPath.Add(genericKey, new List<Rate>());
            }
            ((List<Rate>)specificPath[genericKey]).Add(rate);

            if (!genericPath.ContainsKey(key))
            {
                genericPath.Add(key, new List<Rate>());
            }
            ((List<Rate>)genericPath[key]).Add(rate);

            if (!genericPath.ContainsKey(genericKey))
            {
                genericPath.Add(genericKey, new List<Rate>());
            }
            ((List<Rate>)genericPath[genericKey]).Add(rate);
        }
    }
}
