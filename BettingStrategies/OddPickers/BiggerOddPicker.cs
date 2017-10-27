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
        public OddResult PickOdd(SportMatch sportMatch)
        {
            OddResult oddResult = new OddResult() { OddValue = -1, ExpectedResult = "I" };

            if (sportMatch.HtOdd > sportMatch.DrawOdd && sportMatch.HtOdd > sportMatch.AtOdd)
            {
                oddResult = new OddResult { OddValue = sportMatch.HtOdd, ExpectedResult = "H" };
            }

            if (sportMatch.DrawOdd > sportMatch.HtOdd && sportMatch.DrawOdd > sportMatch.AtOdd)
            {
                oddResult = new OddResult { OddValue = sportMatch.DrawOdd, ExpectedResult = "D" };
            }

            if (sportMatch.AtOdd > sportMatch.HtOdd && sportMatch.AtOdd > sportMatch.DrawOdd)
            {
                oddResult = new OddResult { OddValue = sportMatch.AtOdd, ExpectedResult = "A" };
            }

            return oddResult;
        }
    }
}
