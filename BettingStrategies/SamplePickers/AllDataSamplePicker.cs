using DataModels.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels;

namespace BettingStrategies.SamplePickers
{
    /// <summary>
    /// Return games that have at least one odd bigger than minValue and at leats one odd smaller than maxValue
    /// </summary>
    public class AllDataSamplePicker : ISamplePicker
    {
        public string Name { get { return "AllDataSamplePicker"; } }
        public List<SportMatch> PickSampleData(List<SportMatch> sportMatches, double minValue, double maxValue)
        {
            var sampleData = sportMatches
                    .Where(w => w.HtOdd >= minValue || w.DrawOdd >= minValue || w.AtOdd >= minValue)
                    .Where(w => w.HtOdd <= maxValue || w.DrawOdd <= maxValue || w.AtOdd <= maxValue)
                    .OrderBy(o => o.Date).ToList();

            return sampleData;
        }
    }
}
