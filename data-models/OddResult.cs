using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class OddResult 
    {
        public double OddValue { get; set; }
        public string ExpectedResult { get; set; }

        public bool Compare(SportMatch gameMatch)
        {
            return gameMatch.Result == this.ExpectedResult;
        }
    }
}
