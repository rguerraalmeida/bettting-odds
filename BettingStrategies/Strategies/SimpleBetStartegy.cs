using BettingStrategies.Helper;
using DataModels;
using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingStrategies.Strategies
{
    /// <summary>
    /// This Strategy is simply betting on the game if we lose no double betting if wining just add to wallet
    public class SimpleBetStartegy // : IStrategy
    {
        public string Name { get { return "SimpleBetStartegy"; } }

        //public void Calculate(List<SportMatch> sampleData, ISamplePicker samplePicker, double minValue, double maxValue, IOddPicker OddPicker, int initialValue, int profitOnBet, out double maxBetValue, out double totalProfits, out double riskFactor, out double consecutiveLosses, out List<Operation> operationsPerformed)
        //{
        //    int i = 0;
        //    double currentMoney = initialValue;
        //    List<Operation> operations = new List<Operation>();

        //    var sportMatches = samplePicker.PickSampleData(sampleData, minValue, maxValue);

        //    if (sportMatches.Count() == 0)
        //    {
        //        maxBetValue = 0;
        //        totalProfits = 0;
        //        riskFactor = 0;
        //        operationsPerformed = new List<Operation>();
        //        consecutiveLosses = 0;
        //        return;
        //    }

        //    foreach (var gameMatch in sportMatches)
        //    {
        //        i++;
        //        //this just gets the odd we are supposed to bet into, in this one we are skipping draws, double bets on draws, and wins
        //        var bestOdd = OddPicker.PickOdd(gameMatch);

        //        if (bestOdd.OddValue == -1)
        //        {
        //            continue;
        //        }

        //        double waveLoss = operations.OrderByDescending(b => b.Id).TakeWhile(b => b.Win == false).Select(b => b.BetValue).Sum();
        //        var totalLossPlusBetProfit = waveLoss + profitOnBet;

        //        var betValue = Math.Round(totalLossPlusBetProfit / bestOdd.OddValue, 2);

        //        if (currentMoney < betValue)
        //        {
        //            currentMoney = 0;
        //            break;
        //        }

        //        var beforeMoney = currentMoney;
        //        var risk = betValue / currentMoney * 100;

        //        if (gameMatch.Result == bestOdd.ExpectedResult)
        //        {
        //            currentMoney += betValue * bestOdd.OddValue;
        //            operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = true, RiskFactor = risk });
        //        }
        //        else if (gameMatch.Result != bestOdd.ExpectedResult)
        //        {
        //            currentMoney -= betValue;
        //            operations.Add(new Operation() { Id = i, OperationDate = gameMatch.Date, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.OddValue, BetValue = betValue, AfterBetMoney = currentMoney, Win = false, RiskFactor = risk });
        //        }



        //        if (currentMoney <= 0)
        //        {
        //            break;
        //        }
        //    }

        //    if (currentMoney <= 0)
        //    {
        //        maxBetValue = 0;
        //        totalProfits = 0;
        //        riskFactor = 100;
        //    }
        //    else
        //    {
        //        maxBetValue = Math.Round(operations.Max(s => s.BetValue), 2);
        //        totalProfits = Math.Round(operations.Last().AfterBetMoney - initialValue, 2);
        //        riskFactor = operations.Max(o => o.RiskFactor);
        //    }

        //    operationsPerformed = operations;


        //    //Count consecutives losses 
        //    var groupedResults = operations.GroupWhile((prev, current) => prev.Win == current.Win)
        //        .Where(group => group.Count() > 1)
        //        .Select(group => new
        //        {
        //            OperationResult = group.First(),
        //            Count = group.Count(),
        //        });

        //    consecutiveLosses = groupedResults.Where(w => w.OperationResult.Win == false).Max(m => m.Count);
        //}
    }
}
