using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class ResultsData
    {
        public double MaxBetValue { get; set; }
        public double TotalProfits { get; set; }
        public double RiskFactor { get; set; }
        public double ConsecutiveLosses { get; set; }

        public List<Operation> OperationsPerformed { get; set; }

        public int NumberOperations => OperationsPerformed.Count();

        public List<MonthlyProfit> MonthlyProfit { get; set; }
    }
}
