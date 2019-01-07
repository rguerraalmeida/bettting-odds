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
        ResultsData Calculate(List<SportMatch> sampleData, ISamplePicker samplePicker, double minValue, double maxValue, IOddPicker OddPicker, int initialValue, int profitOnBet);
    }
}
