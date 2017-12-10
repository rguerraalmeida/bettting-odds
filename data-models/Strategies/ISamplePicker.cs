using System.Collections.Generic;

namespace DataModels.Strategies
{
    public interface ISamplePicker
    {
        string Name { get; }
        List<SportMatch> PickSampleData(List<SportMatch> sportMatches, double minValue, double maxValue);
    }
}