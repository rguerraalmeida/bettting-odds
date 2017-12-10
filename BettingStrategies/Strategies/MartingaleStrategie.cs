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


        /// <summary>
        /// Calculates the strategy
        /// </summary>
        /// <param name="sampleData"></param>
        /// <param name="initialValue"></param>
        /// <param name="oddValue"></param>
        /// <param name="profitOnBet"></param>
        /// <param name="maxBetValue"></param>
        /// <param name="totalProfits"></param>
        public void Calculate(List<SportMatch> sampleData,  int initialValue, double oddValue, int profitOnBet, out double maxBetValue, out double totalProfits, out double riskFactor)
        {
            int i = 0;
            double currentMoney = initialValue;
            List<Operation> operations = new List<Operation>();

            var sportMatches = sampleData;
            foreach (var gameMatch in sportMatches)
            {
                i++;
                //this just gets the odd we are supposed to bet into, in this one we are skipping draws double bets on draws and wins
                dynamic bestOdd = new { Odd = -1, ExpectedResult = "I" }; ;

                if (gameMatch.HtOdd > gameMatch.DrawOdd && gameMatch.HtOdd > gameMatch.AtOdd)
                {
                    bestOdd = new { Odd = gameMatch.HtOdd, ExpectedResult = "H" };
                }

                if (gameMatch.DrawOdd > gameMatch.HtOdd && gameMatch.DrawOdd > gameMatch.AtOdd)
                {
                    bestOdd = new { Odd = gameMatch.DrawOdd, ExpectedResult = "D" };
                }

                if (gameMatch.AtOdd > gameMatch.HtOdd && gameMatch.AtOdd > gameMatch.DrawOdd)
                {
                    bestOdd = new { Odd = gameMatch.AtOdd, ExpectedResult = "A" };
                }

                if (bestOdd.Odd == -1)
                {
                    continue;
                }

                double waveLoss = operations.OrderByDescending(b => b.Id).TakeWhile(b => b.Win == false).Select(b => b.BetValue).Sum();
                var totalLossPlusBetProfit = waveLoss + profitOnBet;

                var betValue = Math.Round(totalLossPlusBetProfit / bestOdd.Odd, 2);

                if (currentMoney < betValue)
                {
                    currentMoney = 0;
                    break;
                }

                var beforeMoney = currentMoney;
                var risk = betValue / currentMoney * 100;

                if (gameMatch.Result == bestOdd.ExpectedResult)
                {
                    currentMoney += betValue * bestOdd.Odd;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = true, RiskFactor = risk });
                }
                else if (gameMatch.Result != bestOdd.ExpectedResult)
                {
                    currentMoney -= betValue;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = false, RiskFactor = risk });
                }

                if (currentMoney <= 0)
                {
                    break;
                }
            }

            if (currentMoney <= 0)
            {
                maxBetValue = 0;
                totalProfits = 0;
                riskFactor = 100;
            }
            else
            { 
                maxBetValue = Math.Round(operations.Max(s => s.BetValue), 2);
                totalProfits = Math.Round(operations.Last().AfterBetMoney - initialValue, 2);
                riskFactor = operations.Max(o => o.RiskFactor);
            }
        }

        public void Calculate(List<SportMatch> sampleData, ISamplePicker samplePicker, double minValue, double maxValue, IOddPicker OddPicker, int initialValue, int profitOnBet, out double maxBetValue, out double totalProfits, out double riskFactor, out double consecutiveLosses, out List<Operation> operationsPerformed)
        {
            int i = 0;
            double currentMoney = initialValue;
            List<Operation> operations = new List<Operation>();

            var sportMatches = samplePicker.PickSampleData(sampleData, minValue, maxValue);

            if (sportMatches.Count() == 0)
            {
                maxBetValue = 0;
                totalProfits = 0;
                riskFactor = 0;
                operationsPerformed = new List<Operation>();
                consecutiveLosses = 0;
                return;
            }

            foreach (var gameMatch in sportMatches)
            {
                i++;
                //this just gets the odd we are supposed to bet into, in this one we are skipping draws, double bets on draws, and wins
                var bestOdd = OddPicker.PickOdd(gameMatch);
    
                if (bestOdd.OddValue == -1)
                {
                    continue;
                }

                double waveLoss = operations.OrderByDescending(b => b.Id).TakeWhile(b => b.Win == false).Select(b => b.BetValue).Sum();
                var totalLossPlusBetProfit = waveLoss + profitOnBet;

                var betValue = Math.Round(totalLossPlusBetProfit / bestOdd.OddValue, 2);

                if (currentMoney < betValue)
                {
                    currentMoney = 0;
                    break;
                }

                var beforeMoney = currentMoney;
                var risk = betValue / currentMoney * 100;
                 
                if (gameMatch.Result == bestOdd.ExpectedResult)
                {
                    currentMoney += betValue * bestOdd.OddValue;
                    operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = true, RiskFactor = risk });
                }
                else if (gameMatch.Result != bestOdd.ExpectedResult)
                {
                    currentMoney -= betValue;
                    operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = false, RiskFactor = risk });
                }

                

                if (currentMoney <= 0)
                {
                    break;
                }
            }

            if (currentMoney <= 0)
            {
                maxBetValue = 0;
                totalProfits = 0;
                riskFactor = 100;
            }
            else
            {
                maxBetValue = Math.Round(operations.Max(s => s.BetValue), 2);
                totalProfits = Math.Round(operations.Last().AfterBetMoney - initialValue, 2);
                riskFactor = operations.Max(o => o.RiskFactor);
            }

            operationsPerformed = operations;


            //Count consecutives losses 
            var groupedResults = operations.GroupWhile((prev, current) => prev.Win == current.Win)
                .Where(group => group.Count() > 1)
                .Select(group => new
                {
                    OperationResult = group.First(),
                    Count = group.Count(),
                });
            
            consecutiveLosses = groupedResults.Where(w => w.OperationResult.Win == false).Max(m => m.Count);
        }
    }
}
