using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Strategies
{
    public interface IOddPicker
    {
        string Name { get; }
        //double OddValue { get; }
        //void SetOddValue(double oddValue);
        OddResult PickOdd(SportMatch sportMatch);
    }
}
