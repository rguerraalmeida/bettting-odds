using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class SportMatch
    {
        public int ID { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public string Competition { get; set; }
        public string Result { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public double HtOdd { get; set; }
        public double DrawOdd { get; set; }
        public double AtOdd { get; set; }
        public string Score { get; set; }
        public int HalfHGoals { get; set; }
        public int HalfAGoals { get; set; }
        public string HalfTimeResult { get; set; }
        
    }
}
