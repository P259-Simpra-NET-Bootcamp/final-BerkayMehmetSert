using Application.Services;
using Xunit;

namespace Application.Test.Services;

public class PointCalculationServiceTest
{
    private readonly PointCalculationService _pointCalculationService;

    public PointCalculationServiceTest()
    {
        _pointCalculationService = new PointCalculationService();
    }

    [Theory]
    [InlineData(99, 0.03)]
    [InlineData(100, 0.05)]
    [InlineData(499, 0.05)]
    [InlineData(500, 0.07)]
    [InlineData(999, 0.07)]
    [InlineData(1000, 0.1)]
    public void CalculateScorePercentageShouldReturnCorrectPercentageWhenPriceIsGiven(
        decimal price,
        decimal expectedPercentage)
    {
        var actualPercentage = _pointCalculationService.CalculateScorePercentage(price);
        Assert.Equal(expectedPercentage, actualPercentage);
    }

    [Theory]
    [InlineData(99, 2.97)]
    [InlineData(100, 5)]
    [InlineData(499, 24.95)]
    [InlineData(500, 35)]
    [InlineData(999, 69.93)]
    [InlineData(1000, 100)]
    public void CalculateMaxScoreShouldReturnCorrectMaxScoreWhenPriceAndScorePercentageIsGiven(
        decimal price,
        decimal expectedMaxScore)
    {
        var scorePercentage = _pointCalculationService.CalculateScorePercentage(price);
        var actualMaxScore = _pointCalculationService.CalculateMaxScore(price, scorePercentage);
        Assert.Equal(expectedMaxScore, actualMaxScore);
    }
}