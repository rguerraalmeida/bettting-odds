using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Operation 
    {
        public int Id { get; set; }
        public OperationType OperationType { get; set; }
        public double InitialMoney { get; set; }
        public double Odd { get; set; }
        public double BetValue { get; set; }
        public bool Win { get; set; }
        public double AfterBetMoney { get; set; }
        public double Profit { get; set; }
        public double Recharged { get; set; }
    }
}
