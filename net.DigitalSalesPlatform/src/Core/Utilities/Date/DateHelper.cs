namespace Core.Utilities.Date;

public static class DateHelper
{
    public static DateTime GetCurrentDate() => DateTime.Now;
    public static DateTime GetPreviousDate() => DateTime.Now.AddDays(-1);
}