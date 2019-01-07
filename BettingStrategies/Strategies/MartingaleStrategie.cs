using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;
using BettingStrategies.Helper;

namespace BettingStrategies.Strategies
{
    /// <summary>
    /// This Strategy is based on Martingale startegy and doubles the bet when a previous bet is lost, in this case we bet only on the best odd
    /// considering bets that have one of the three odds better than the minimum oddValue
    public class MartingaleStrategie : IStrategy
    {
        public string Name { get { return "MartingaleStrategie"; } }


        public ResultsData Calculate(List<SportMatch> sampleData, ISamplePicker samplePicker, double minValue, double maxValue, IOddPicker OddPicker, int initialValue, int profitOnBet )
        {
            int i = 0;
            double currentMoney = initialValue;
            double wallet = 0;

            List<Operation> operations = new List<Operation>();

            var sportMatches = samplePicker.PickSampleData(sampleData, minValue, maxValue);

            if (!sportMatches.Any())
            {
                return new ResultsData()
                {
                    OperationsPerformed = new List<Operation>(),
                };
            }

            var currentMonth = sportMatches.First().Date.ToString("yyyyMM");


            foreach (var gameMatch in sportMatches)
            {
                i++;

                var gameMonth = gameMatch.Date.ToString("yyyyMM");

                if (currentMonth != gameMonth)
                {
                    currentMonth = gameMonth;

                    if (currentMoney > initialValue)
                    {
                        var profitsSoFar = currentMoney - initialValue;
                        wallet += profitsSoFar;
                        currentMoney -= profitsSoFar;


                    }
                }


                //this just gets the odd we are supposed to bet into, in this one we are skipping draws, double bets on draws, and wins
                var selectedOdd = OddPicker.PickOdd(gameMatch);

                if (selectedOdd.OddValue == -1)
                {
                    continue;
                }

                double waveLoss = operations.OrderByDescending(b => b.Id).TakeWhile(b => b.Win == false).Select(b => b.BetValue).Sum();
                var totalLossPlusBetProfit = waveLoss + profitOnBet;

                var betValue = Math.Round(totalLossPlusBetProfit / selectedOdd.OddValue, 2);

                if (currentMoney < betValue)
                {
                    currentMoney = 0;
                    break;
                }

                var beforeMoney = Math.Round(currentMoney, 2);
                var risk = Math.Round(betValue / currentMoney * 100, 2);

                if (gameMatch.Result == selectedOdd.ExpectedResult)
                {
                    currentMoney += betValue * selectedOdd.OddValue;
                    operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = selectedOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = true, RiskFactor = risk });
                }
                else if (gameMatch.Result != selectedOdd.ExpectedResult)
                {
                    currentMoney -= betValue;
                    operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = selectedOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = false, RiskFactor = risk });
                }



                if (currentMoney <= 0)
                {
                    break;
                }
            }




            //Count consecutives losses 
            var groupedResults = operations.GroupWhile((prev, current) => prev.Win == current.Win)
                .Where(group => group.Count() > 1)
                .Select(group => new
                {
                    OperationResult = group.First(),
                    Count = group.Count(),
                });

            var consecutiveLosses = groupedResults.Where(w => w.OperationResult.Win == false).Max(m => m.Count);


            if (currentMoney <= 0)
            {
                return new ResultsData()
                {
                    OperationsPerformed = operations,
                    RiskFactor = 100,
                    ConsecutiveLosses = consecutiveLosses,
                };
            }
            else
            {
                return new ResultsData()
                {
                    MaxBetValue = Math.Round(operations.Max(s => s.BetValue), 2),
                    OperationsPerformed = operations,
                    RiskFactor = operations.Max(o => o.RiskFactor),
                    TotalProfits = wallet + currentMoney,
                    ConsecutiveLosses = consecutiveLosses,

                };
            }

        }

    }
}
