using DataModels;
using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingStrategies.OddPickers
{
    /// Picks the odds based on probability of winning, so we choose the odd with lower racio to profit value
    public class EasierOddPicker : IOddPicker
    {
        public string Name { get { return "EasierOddPicker"; } }
        public OddResult PickOdd(SportMatch sportMatch)
        {
            OddResult oddResult = new OddResult() { OddValue = -1, ExpectedResult = "I" };

            List<OddResult> values = new List<OddResult>() {
                new OddResult { OddValue = sportMatch.HtOdd, ExpectedResult = "H" },
                new OddResult { OddValue = sportMatch.DrawOdd, ExpectedResult = "D" },
                new OddResult { OddValue = sportMatch.AtOdd, ExpectedResult = "A" }
            }.OrderBy(o => o.OddValue).ToList();

            oddResult = values.ToArray()[0];

            return oddResult;
        }
    }
}
