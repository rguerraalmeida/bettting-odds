using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class SampleData
    {
        public int InitialValue { get; set; }
        public double OddValue { get; set; }
        public int ProfitOnBet { get; set; }
        public double MaxBetValue { get; set; }
        public double TotalProfits { get; set; }
        public double RiskFactor { get; set; }

        public string StrategyName { get; set; }
    }
}
