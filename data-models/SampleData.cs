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
        public string SamplePickerName { get; set; }
        public string OddPickerName { get; set; }

        public int NumberOperastions {
            get {
                return OperationsPerformed.Count();
            }
        }
        public List<Operation> OperationsPerformed { get; set; }
        public List<double> MonthlyGains{ get; set; }
    }
}
