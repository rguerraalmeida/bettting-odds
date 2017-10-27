using System.Collections.Generic;

namespace DataModels.Strategies
{
    public interface ISamplePicker
    {
        List<SportMatch> PickSampleData(List<SportMatch> sportMatches, double minValue, double maxValue);
    }
}