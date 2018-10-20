using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;

namespace BettingStrategies.OddPickers
{
    /// <summary>
    /// Picks the odds based on return value, so we choose the odd with bigger racio to profit value
    /// </summary>
    public class BiggerOddPicker : IOddPicker
    {
        public string Name { get { return "BiggerOddPicker"; } }

        public OddResult PickOdd(SportMatch sportMatch)
        {
            OddResult oddResult = new OddResult() { OddValue = -1, ExpectedResult = "I" };

            List<OddResult> values = new List<OddResult>() {
                new OddResult { OddValue = sportMatch.HtOdd, ExpectedResult = "H" },
                new OddResult { OddValue = sportMatch.DrawOdd, ExpectedResult = "D" },
                new OddResult { OddValue = sportMatch.AtOdd, ExpectedResult = "A" }
            }.OrderByDescending(o => o.OddValue).ToList();

            oddResult = values.ToArray()[0];

            return oddResult;
        }
    }
}
