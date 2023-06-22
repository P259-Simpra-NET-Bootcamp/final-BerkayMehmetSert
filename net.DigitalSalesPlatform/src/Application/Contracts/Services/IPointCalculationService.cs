namespace Application.Contracts.Services;

public interface IPointCalculationService
{
    decimal CalculateScorePercentage(decimal price);
    decimal CalculateMaxScore(decimal price, decimal scorePercentage);
}