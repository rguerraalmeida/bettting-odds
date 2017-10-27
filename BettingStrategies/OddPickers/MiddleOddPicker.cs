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
        public OddResult PickOdd(SportMatch sportMatch)
        {
            OddResult oddResult = new OddResult() { OddValue = -1, ExpectedResult = "I" };

            if (sportMatch.HtOdd > sportMatch.DrawOdd && sportMatch.HtOdd < sportMatch.AtOdd )
            {
                oddResult = new OddResult { OddValue = sportMatch.HtOdd, ExpectedResult = "H" };
            }

            if (sportMatch.HtOdd > sportMatch.DrawOdd && sportMatch.DrawOdd > sportMatch.AtOdd)
            {
                oddResult = new OddResult { OddValue = sportMatch.DrawOdd, ExpectedResult = "D" };
            }

            if (sportMatch.AtOdd > sportMatch.DrawOdd && sportMatch.HtOdd > sportMatch.AtOdd)
            {
                oddResult = new OddResult { OddValue = sportMatch.AtOdd, ExpectedResult = "A" };
            }

            return oddResult;
        }
    }
}
