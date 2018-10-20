using DataModels;
using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingStrategies.OddPickers
{
    /// <summary>
    /// Picks the odds on the middled of the probability of winning
    /// </summary>
    public class MiddleOddPicker : IOddPicker
    {
        public string Name { get { return "MiddleOddPicker"; } }
        public OddResult PickOdd(SportMatch sportMatch)
        {
            OddResult oddResult = new OddResult() { OddValue = -1, ExpectedResult = "I" };

            List<OddResult> values = new List<OddResult>() {
                new OddResult { OddValue = sportMatch.HtOdd, ExpectedResult = "H" },
                new OddResult { OddValue = sportMatch.DrawOdd, ExpectedResult = "D" },
                new OddResult { OddValue = sportMatch.AtOdd, ExpectedResult = "A" }
            }.OrderBy(o => o.OddValue).ToList();

            int middleIndex = (values.Count() - 1) / 2;
            oddResult = values.ToArray()[middleIndex];

            return oddResult;
        }
    }
}
