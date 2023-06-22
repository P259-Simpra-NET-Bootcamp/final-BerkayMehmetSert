using Application.Contracts.Constants.Point;
using Application.Contracts.Services;

namespace Application.Services;

public class PointCalculationService : IPointCalculationService
{
    private const int LessThan100Threshold = 100;
    private const int LessThan500Threshold = 500;
    private const int LessThan1000Threshold = 1000;

    public decimal CalculateScorePercentage(decimal price)
    {
        var percentageValue = price switch
        {
            < LessThan100Threshold => ProductPointConfiguration.LessThan100,
            < LessThan500Threshold => ProductPointConfiguration.LessThan500,
            < LessThan1000Threshold => ProductPointConfiguration.LessThan1000,
            _ => ProductPointConfiguration.Default
        };

        return GetPercentage(percentageValue);
    }

    public decimal CalculateMaxScore(decimal price, decimal scorePercentage)
    {
        var maxScore = price * scorePercentage;
        return Math.Round(maxScore, 2);
    }

    private static decimal GetPercentage(int percentageValue) => percentageValue / ProductPointConfiguration.Hundred;
}