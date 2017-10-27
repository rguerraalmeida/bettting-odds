using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Strategies
{
    public interface IStrategy
    {
        string Name { get; }
        void Calculate(List<SportMatch> sampleData, int initialValue, double oddValue, int profitOnBet, out double maxBetValue, out double totalProfits, out double riskFactor);

        void Calculate(List<SportMatch> sampleData, ISamplePicker samplePicker, double minValue, double maxValue, IOddPicker OddPicker, int initialValue, int profitOnBet, out double maxBetValue, out double totalProfits, out double riskFactor, out double consecutiveLosses, out List<Operation> operationsPerformed);
    }
}
